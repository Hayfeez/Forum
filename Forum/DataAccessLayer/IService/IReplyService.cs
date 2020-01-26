using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Models;
using static Forum.CommonClasses.BaseClass;

namespace Forum.DataAccessLayer.IService
{
    public interface IReplyService
    {
        IEnumerable<Thread> GetAllRepliesToThread(long threadId);
        Thread GetReplyById(long replyId);

        Task<DbActionsResponse> CreateReply(ThreadReply reply);
        Task<DbActionsResponse> UpdateReply(long replyId, string newContent);
        Task<DbActionsResponse> DeleteReply(long replyId);

    }
}
