using Bipki.Database.Models;
using Poll = Bipki.App.Features.Chats.PollModels.Poll;

namespace Bipki.App.Features.Chats.ChatModels;

public class ChatModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public Message[] Messages { get; set; } = null!;

    public Poll[] Polls { get; set; } = null!;
}