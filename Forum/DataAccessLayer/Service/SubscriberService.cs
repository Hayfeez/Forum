using System;
using Forum.Data;
using Forum.DataAccessLayer.IService;

namespace Forum.DataAccessLayer.Service
{
    public class SubscriberService : ISubscriberService
    {
        private ApplicationDbContext _dbContext;
        public SubscriberService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public int GetSubscriberId()
        {
            return 1;
        }
    }
}
