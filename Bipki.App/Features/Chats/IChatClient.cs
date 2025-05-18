using Bipki.Database.Models;
using Poll = Bipki.App.Features.Chats.PollModels.Poll;

namespace Bipki.App.Features.Chats;

public interface IChatClient
{
    Task ReceiveMessage(Message message);

    Task PollUpdated(Poll poll);
}