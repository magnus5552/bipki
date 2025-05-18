using Bipki.Database.Models;
using Bipki.Database.Models.UserModels;
using Bipki.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Poll = Bipki.App.Features.Chats.PollModels.Poll;
using DbPoll = Bipki.Database.Models.Poll;
using PollOption = Bipki.App.Features.Chats.PollModels.PollOption;
using DbPollOption = Bipki.Database.Models.PollOption;

namespace Bipki.App.Features.Chats.Hubs;

[Authorize]
public class ChatHub : Hub<IChatClient>
{
    private readonly IChatRepository chatRepository;
    private readonly UserManager<User> userManager;
    private readonly IMessageRepository messageRepository;
    private readonly IActivityRepository activityRepository;
    private readonly IActivityRegistrationRepository activityRegistrationRepository;
    private readonly IPollRepository pollRepository;
    private readonly IPollOptionRepository pollOptionRepository;
    private readonly IVoteRepository voteRepository;

    public ChatHub(IChatRepository chatRepository, UserManager<User> userManager,
        IMessageRepository messageRepository,
        IActivityRepository activityRepository,
        IActivityRegistrationRepository activityRegistrationRepository,
        IPollRepository pollRepository,
        IPollOptionRepository pollOptionRepository,
        IVoteRepository voteRepository)
    {
        this.chatRepository = chatRepository;
        this.userManager = userManager;
        this.messageRepository = messageRepository;
        this.activityRepository = activityRepository;
        this.activityRegistrationRepository = activityRegistrationRepository;
        this.pollRepository = pollRepository;
        this.pollOptionRepository = pollOptionRepository;
        this.voteRepository = voteRepository;
    }

    public async Task SendMessage(Guid chatId, string message)
    {
        if (Context.User is null)
        {
            return;
        }

        var user = await userManager.GetUserAsync(Context.User);

        if (user is null)
        {
            return;
        }

        var chat = await chatRepository.GetById(chatId);
        if (chat is null)
        {
            return;
        }

        if (!await IsChatAllowedForUser(chat, user))
        {
            return;
        }

        var messageModel = new Message
        {
            Timestamp = DateTime.UtcNow,
            Text = message,
            ChatId = chatId,
            SenderId = user.Id,
            SenderName = user.Name +" " + user.Surname,
            Id = Guid.NewGuid()
        };

        await messageRepository.Add(messageModel);
        await Clients.Group(chatId.ToString()).ReceiveMessage(messageModel);
    }

    public async Task CreatePoll(Guid chatId, Poll poll)
    {
        if (Context.User is null)
        {
            return;
        }

        var user = await userManager.GetUserAsync(Context.User);

        if (user is null)
        {
            return;
        }

        if (!await userManager.IsInRoleAsync(user, Roles.Admin))
        {
            return;
        }

        var chat = await chatRepository.GetById(chatId);
        if (chat is null)
        {
            return;
        }

        var dbPoll = new DbPoll
        {
            ChatId = chatId,
            TimeStamp = DateTime.UtcNow,
            Title = poll.Text,
            Id = Guid.NewGuid()
        };

        await pollRepository.AddPollAsync(dbPoll);

        var pollOptions = poll.Options.Select(x =>
        {
            x.Id = Guid.NewGuid();
            return new DbPollOption
            {
                PollId = dbPoll.Id,
                Text = x.Text,
                Id = x.Id
            };
        });

        await pollOptionRepository.AddPollOptionRangeAsync(pollOptions);
        poll.Id = dbPoll.Id;
        await Clients.Group(chatId.ToString()).PollUpdated(poll);
    }

    public async Task Vote(Guid pollId, Guid optionId)
    {
        if (Context.User is null)
        {
            return;
        }

        var user = await userManager.GetUserAsync(Context.User);

        if (user is null)
        {
            return;
        }

        if (await userManager.IsInRoleAsync(user, Roles.Admin))
        {
            return;
        }

        var poll = await pollRepository.GetByIdAsync(pollId);
        if (poll is null)
        {
            return;
        }
        var chat = await chatRepository.GetById(poll.ChatId);
        if (chat is null)
        {
            return;
        }

        var option = await pollOptionRepository.GetById(optionId);
        if (option is null)
        {
            return;
        }

        var vote = new Vote
        {
            PollOptionId = optionId,
            UserId = user.Id,
            Id = Guid.NewGuid()
        };

        await voteRepository.AddAsync(vote);

        var bdOptions = pollOptionRepository.GetByPollId(poll.Id);
        var options = new List<PollOption>();
        foreach (var opt in bdOptions)
        {
            var votes = voteRepository.GetByPollOptionId(opt.Id);
            options.Add(new PollOption
            {
                Text = opt.Text,
                VotesCount = votes.Count(),
                Selected = opt.Id == pollId,
                Id = opt.Id
            });
        }

        var pollModel = new Poll
        {
            Id = poll.Id,
            Text = poll.Title,
            SenderName = "Admin",
            TimeStamp = poll.TimeStamp,
            TotalVotesCount = options.Sum(x => x.VotesCount),
            Options = options
        };


        await Clients.Client(Context.ConnectionId).PollUpdated(pollModel);
        
        var selectedOpt = pollModel.Options.First(x => x.Selected);
        selectedOpt.Selected = false;
        await Clients.Group(chat.Id.ToString()).PollUpdated(pollModel);
    }

    public async Task EnterChat(Guid chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task ExitChat(Guid chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
    
    private async Task<bool> IsChatAllowedForUser(Chat chat, User user)
    {
        if (await userManager.IsInRoleAsync(user, Roles.Admin) ||
            chat.Type == ChatType.Conference)
        {
            return true;
        }

        var activity = await activityRepository.GetByChatId(chat.Id);
        if (activity is null)
        {
            return false;
        }

        var activityRegistration =
            await activityRegistrationRepository.GetVerifiedByActivityAndUserId(activity.Id, user.Id);

        return activityRegistration is not null;
    }
}