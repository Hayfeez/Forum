using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Models;
using static Forum.Helpers.BaseClass;

namespace Forum.DataAccessLayer.IService
{
    public interface IReplyService
    {
        IEnumerable<ThreadReply> GetAllRepliesToThread(long threadId);

        Task<DbActionsResponse> CreateReply(ThreadReply reply);
        Task<DbActionsResponse> UpdateReply(long replyId, string newContent);
        Task<DbActionsResponse> DeleteReply(long replyId);

    }
}
