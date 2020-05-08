using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Helpers;

namespace Forum.DataAccessLayer.IService
{
    public interface ISearchService
    {
        IEnumerable<Thread> SearchAllThreads(int subscriberId, string  searchQuery);

    }
}
