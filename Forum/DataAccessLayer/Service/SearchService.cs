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
    public class SearchService :ISearchService
    {
        private readonly ApplicationDbContext _dbContext;
        public SearchService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
       
        public IEnumerable<Thread> SearchAllThreads(int subscriberId, string searchQuery)
        {
            try
            {
                var topic = _dbContext.Threads
                    .Where(a => a.Title.Contains(searchQuery) || a.Content.Contains(searchQuery))
                   .Include(a => a.ThreadReplies).ThenInclude(b => b.SubscriberUser)
                   .Include(a => a.SubscriberUser) //.ThenInclude(b=>b.Subscriber)
                   .Where(c=>c.SubscriberUser.SubscriberId == subscriberId);

                return topic.OrderByDescending(a => a.DateCreated);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

       
    }
}
