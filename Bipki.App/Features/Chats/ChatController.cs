using Bipki.App.Features.Chats.ChatModels;
using Bipki.Database.Models;
using Bipki.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DbPoll = Bipki.Database.Models.Poll;
using Poll = Bipki.App.Features.Chats.PollModels.Poll;
using PollOption = Bipki.App.Features.Chats.PollModels.PollOption;

namespace Bipki.App.Features.Chats;

[Authorize]
[Route("chats")]
public class ChatController: ControllerBase
{
    private readonly IChatRepository chatRepository;
    private readonly IMessageRepository messageRepository;
    private readonly IPollRepository pollRepository;
    private readonly IPollOptionRepository pollOptionRepository;
    private readonly IVoteRepository voteRepository;

    public ChatController(IChatRepository chatRepository, IMessageRepository messageRepository, IPollRepository pollRepository,
        IPollOptionRepository pollOptionRepository, IVoteRepository voteRepository)
    {
        this.chatRepository = chatRepository;
        this.messageRepository = messageRepository;
        this.pollRepository = pollRepository;
        this.pollOptionRepository = pollOptionRepository;
        this.voteRepository = voteRepository;
    }

    [HttpGet]
    [Route("{chatId}")]
    public async Task<IActionResult> GetAllChats([FromRoute] Guid chatId)
    {
        var chat = await chatRepository.GetById(chatId);
        if (chat is null)
        {
            return NotFound();
        }

        var messages = messageRepository.GetByChatId(chatId);

        var polls = pollRepository.GetByChatId(chatId);

        var generatedPolls = new List<Poll>();
        
        foreach (var poll in polls)
        {
            generatedPolls.Add(await GeneratePoll(poll));
        }

        return Ok(new ChatModel
        {
            Id = chat.Id,
            Title = chat.Title,
            Messages = messages.ToArray(),
            Polls = generatedPolls.ToArray()
        });
    }

    private async Task<Poll> GeneratePoll(DbPoll poll)
    {
        var bdOptions = pollOptionRepository.GetByPollId(poll.Id);
        var options = new List<PollOption>();
        foreach (var opt in bdOptions)
        {
            var votes = voteRepository.GetByPollOptionId(opt.Id);
            options.Add(new PollOption
            {
                Text = opt.Text,
                VotesCount = votes.Count(),
                Selected = opt.Id == poll.Id,
                Id = opt.Id
            });
        }

        return new Poll
        {
            Id = poll.Id,
            Text = poll.Title,
            SenderName = "Admin",
            TimeStamp = poll.TimeStamp,
            TotalVotesCount = options.Sum(x => x.VotesCount),
            Options = options
        };
    }
}