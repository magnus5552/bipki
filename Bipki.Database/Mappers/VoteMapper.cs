using Bipki.Database.Models;
using DbVote =  Bipki.Database.Models.BusinessModels.Vote;

namespace Bipki.Database.Mappers;

public static class VoteMapper
{
    public static Vote? Map(DbVote? vote) => vote is null
        ? null
        : new Vote
        {
            PollOptionId = vote.PollOptionId,
            UserId = vote.UserId,
            Id = vote.Id
        };
    
    public static DbVote? Map(Vote? vote) => vote is null
        ? null
        : new DbVote
        {
            PollOptionId = vote.PollOptionId,
            UserId = vote.UserId,
            Id = vote.Id
        };
}