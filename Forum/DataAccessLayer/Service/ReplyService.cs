using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.DataAccessLayer.IService;
using Forum.Models;
using static Forum.CommonClasses.BaseClass;

namespace Forum.DataAccessLayer.Service
{
    public class ReplyService :IReplyService
    {
        public ReplyService()
        {
        }

        public Task<DbActionsResponse> CreateReply(ThreadReply reply)
        {
            throw new NotImplementedException();
        }

        public Task<DbActionsResponse> DeleteReply(long replyId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Thread> GetAllRepliesToThread(long threadId)
        {
            throw new NotImplementedException();
        }

        public Thread GetReplyById(long replyId)
        {
            throw new NotImplementedException();
        }

        public Task<DbActionsResponse> UpdateReply(long replyId, string newContent)
        {
            throw new NotImplementedException();
        }
    }
}
