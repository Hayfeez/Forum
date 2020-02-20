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
    public class ThreadService :IThreadService
    {
        private readonly ApplicationDbContext _ctx;
        public ThreadService(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<DbActionsResponse> CreateThread(Thread thread)
        {
            throw new NotImplementedException();
        }

        public Task<DbActionsResponse> DeleteThread(long threadId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Thread> GetAllThreads(int subscriberId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Thread> GetAllThreadsInChannel(int channelId, int? subCategoryId)
        {
            try
            {
                var topics = _ctx.SubCategories.Include(a => a.Channel).Where(a => a.Channel.Id == channelId).FirstOrDefault().Threads;
                if (subCategoryId != null)
                    topics = topics.Where(a => a.Id == subCategoryId);

                return topics;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Thread> GetFilteredThreads(string searchQuery)
        {
            throw new NotImplementedException();
        }

        public Thread GetThreadById(long threadId)
        {
            try
            {
                var topic = _ctx.Threads.Where(a => a.Id == threadId)
                    .Include(a => a.ThreadReplies).ThenInclude(b => b.AppUser)
                    .Include(a => a.AppUser).FirstOrDefault();

                return topic;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<DbActionsResponse> UpdateThread(long threadId, string newContent)
        {
            throw new NotImplementedException();
        }
    }
}
