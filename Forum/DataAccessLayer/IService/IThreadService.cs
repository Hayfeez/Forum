using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Helpers;

namespace Forum.DataAccessLayer.IService
{
    public interface IThreadService
    {
        IEnumerable<Thread> GetAllThreads(int subscriberId);
        IEnumerable<Thread> SearchThread(string  searchQuery);
        IEnumerable<Thread> GetAllThreadsInChannelOrCategory(int channelId, int? categoryId);
        Thread GetThreadById(long threadId);
        Thread GetGuideline();

        Task<DbActionsResponse> CreateThread(Thread thread);
        Task<DbActionsResponse> UpdateThread(long threadId, string newContent);
        Task<DbActionsResponse> DeleteThread(long threadId);

    }
}
