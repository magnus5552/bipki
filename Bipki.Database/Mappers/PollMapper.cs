using Bipki.Database.Models;
using DbPoll = Bipki.Database.Models.BusinessModels.Poll;

namespace Bipki.Database.Mappers;

public static class PollMapper
{
    public static Poll? Map(DbPoll? poll) => poll is null
        ? null
        : new Poll
        {
            ChatId = poll.ChatId,
            TimeStamp = poll.TimeStamp,
            Title = poll.Title,
            Id = poll.Id
        };
    
    public static DbPoll? Map(Poll? poll) => poll is null
        ? null
        : new DbPoll
        {
            ChatId = poll.ChatId,
            TimeStamp = poll.TimeStamp,
            Title = poll.Title,
            Id = poll.Id
        };
}