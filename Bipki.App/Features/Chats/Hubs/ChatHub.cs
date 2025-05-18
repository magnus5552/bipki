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

    public ChatHub(IChatRepository chatRepository, UserManager<User> userManager, IMessageRepository messageRepository)
    {
        this.chatRepository = chatRepository;
        this.userManager = userManager;
        this.messageRepository = messageRepository;
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

    // TODO
    private Task<bool> IsChatAllowedForUser(Guid chatId, User user)
    {
        throw new NotImplementedException();
    }
}