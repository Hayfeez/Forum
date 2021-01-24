using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.DataAccessLayer.IService;
using Forum.Models;
using Microsoft.EntityFrameworkCore;
using Forum.Helpers;

namespace Forum.DataAccessLayer.Service
{
    public class ThreadService :IThreadService
    {
        private readonly ApplicationDbContext _dbContext;
        public ThreadService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public IEnumerable<Thread> GetAllThreads(int subscriberId)
        {
            try
            {
                var threads = _dbContext.Threads
                        .Include(a => a.Category)
                            .ThenInclude(b => b.Channel)
                        .Include(a=>a.ThreadReplies)
                        .Where(a => a.Category.Channel.SubscriberId == subscriberId);

                return threads;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Thread> GetAllThreadsByUser(long subscriberUserId)
        {
            try
            {
                var threads = _dbContext.Threads
                    .Where(a=>a.SubscriberUserId == subscriberUserId)
                    .Include(a => a.ThreadReplies);

                return threads;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Thread> GetLatestThreads(int subscriberId, int num = 10)
        {
            try
            {
                var threads = _dbContext.Threads
                     .Include(a => a.SubscriberUser)
                        .Include(a => a.Category)
                            .ThenInclude(b => b.Channel)
                        .Where(a => a.Category.Channel.SubscriberId == subscriberId)
                        .OrderByDescending(a => a.DateCreated)
                        .Take(num);

                return threads;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Thread> GetAllThreadsInChannelOrCategory(int channelId, int? categoryId)
        {
            try
            {
                var threads = _dbContext.Threads
                        .Include(a => a.Category)
                           .ThenInclude(b => b.Channel)
                        .Include(a => a.SubscriberUser)
                        .Where(a => a.Category.ChannelId == channelId);

                if (categoryId != null)
                    threads = threads.Where(a => a.Id == categoryId);

                return threads.OrderByDescending(a => a.DateCreated);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public Thread GetThreadById(string title)
        {
            try
            {               
                var topic = _dbContext.Threads.Where(a => a.Title == title)
                    .Include(a=>a.ThreadInfo)
                    .Include(a=>a.Category).ThenInclude(b=>b.Channel)
                    .Include(a => a.SubscriberUser).FirstOrDefault();

                return topic;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> CreateThread(Thread thread)
        {
            try
            {
                if (_dbContext.Threads.Any(a => a.Title == thread.Title && a.CategoryId == thread.CategoryId))
                    return DbActionsResponse.DuplicateExist;

                _dbContext.Threads.Add(thread);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> DeleteThread(long threadId)
        {
            try
            {
                var thread = _dbContext.Threads.Where(a => a.Id == threadId).Include(a => a.ThreadReplies).FirstOrDefault();
                if (thread == null) return DbActionsResponse.NotFound;

                if (thread.ThreadReplies.Count() > 0)
                    return DbActionsResponse.DeleteDenied;

                _dbContext.Threads.Remove(thread);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async  Task<DbActionsResponse> UpdateThread(long threadId, string newContent, long userId)
        {
            try
            {
                var existingThread = _dbContext.Threads.Where(a => a.Id == threadId).FirstOrDefault();
                if (existingThread == null) return DbActionsResponse.NotFound;

                if (existingThread.SubscriberUserId != userId) return DbActionsResponse.DeleteDenied;

                existingThread.Content = newContent;

                _dbContext.Threads.Update(existingThread);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region ThreadReplies

        public IEnumerable<ThreadReply> GetAllRepliesToThread(long threadId)
        {
            try
            {
                var replies = _dbContext.ThreadReplies
                    .Where(a => a.ThreadId == threadId)
                    .Include(b => b.SubscriberUser).ThenInclude(a=>a.ApplicationUser)
                    .Include(a => a.UserThreadReplyInfos)
                    .Include(a => a.ThreadReplyInfo);
                        
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
        #endregion
    }
}
