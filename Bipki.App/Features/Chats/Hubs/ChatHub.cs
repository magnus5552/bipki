using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Bipki.App.Features.Chats.Hubs;

[Authorize]
public class ChatHub : Hub<IChatClient>
{
    public async Task SendMessage(Guid chatId, string message)
    {
        if (Context.User is null)
        {
            return;
        }
        
    }
}