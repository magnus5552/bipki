namespace Bipki.App.Features.Chats;

public interface IChatClient
{
    Task ReceiveMessage();
}