using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.DataAccessLayer.IService;
using Forum.Models;
using Microsoft.EntityFrameworkCore;
using static Forum.Helpers.BaseClass;

namespace Forum.DataAccessLayer.Service
{
    public class ReplyService :IReplyService
    {
        private readonly ApplicationDbContext _dbContext;
        public ReplyService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ThreadReply> GetAllRepliesToThread(long threadId)
        {
            try
            {
                var replies = _dbContext.ThreadReplies
                    .Include(b=>b.SubscriberUser)
                        .Include(a => a.Thread)
                            .ThenInclude(b => b.SubscriberUser)
                        .Where(a => a.Thread.Id == threadId);
               
                return replies;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> CreateReply(ThreadReply reply)
        {
            try
            {
                _dbContext.ThreadReplies.Add(reply);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> DeleteReply(long replyId)
        {
            try
            {
                var reply = _dbContext.ThreadReplies.Where(a => a.Id == replyId).SingleOrDefault();
                if (reply == null) return DbActionsResponse.NotFound;

               
                _dbContext.ThreadReplies.Remove(reply);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> UpdateReply(long replyId, string newContent)
        {
            try
            {
                var oldReply = _dbContext.ThreadReplies.Where(a => a.Id == replyId).FirstOrDefault();
                if (oldReply == null) return DbActionsResponse.NotFound;

                oldReply.Content = newContent;

                _dbContext.ThreadReplies.Update(oldReply);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
