using Bipki.Database.Models;

namespace Bipki.App.Features.Chats;

public interface IChatClient
{
    Task ReceiveMessage(Message message);
}