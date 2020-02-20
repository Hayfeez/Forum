using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Models;
using static Forum.Helpers.BaseClass;

namespace Forum.DataAccessLayer.IService
{
    public interface IThreadService
    {
        IEnumerable<Thread> GetAllThreads(int subscriberId);
        IEnumerable<Thread> GetFilteredThreads(string  searchQuery);
        IEnumerable<Thread> GetAllThreadsInChannel(int channelId, int? subCategoryId);
        Thread GetThreadById(long threadId);

        Task<DbActionsResponse> CreateThread(Thread thread);
        Task<DbActionsResponse> UpdateThread(long threadId, string newContent);
        Task<DbActionsResponse> DeleteThread(long threadId);

    }
}
