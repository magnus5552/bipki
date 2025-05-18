using Bipki.Database.Models;
using Bipki.Database.Models.UserModels;
using Bipki.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Bipki.App.Features.Chats.Hubs;

[Authorize]
public class ChatHub : Hub<IChatClient>
{
    private readonly IChatRepository chatRepository;
    private readonly UserManager<User> userManager;
    private readonly IMessageRepository messageRepository;
    private readonly IActivityRepository activityRepository;
    private readonly IActivityRegistrationRepository activityRegistrationRepository;

    public ChatHub(IChatRepository chatRepository, UserManager<User> userManager,
        IMessageRepository messageRepository,
        IActivityRepository activityRepository,
        IActivityRegistrationRepository activityRegistrationRepository)
    {
        this.chatRepository = chatRepository;
        this.userManager = userManager;
        this.messageRepository = messageRepository;
        this.activityRepository = activityRepository;
        this.activityRegistrationRepository = activityRegistrationRepository;
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
            SenderName = user.Name + user.Surname,
            Id = Guid.NewGuid()
        };

        await messageRepository.Add(messageModel);
        await Clients.Group(chatId.ToString()).ReceiveMessage(messageModel);
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