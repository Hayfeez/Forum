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
        IEnumerable<Thread> GetLatestThreads(int subscriberId, int num = 10);
        IEnumerable<Thread> GetAllThreadsInChannelOrCategory(int channelId, int? categoryId);
        IEnumerable<Thread> GetAllThreadsByUser(long subscriberUserId);
        Thread GetThreadById(string title);
       
        Task<DbActionsResponse> CreateThread(Thread thread);
        Task<DbActionsResponse> UpdateThread(long threadId, string newContent);
        Task<DbActionsResponse> DeleteThread(long threadId);


        IEnumerable<ThreadReply> GetAllRepliesToThread(long threadId);

        Task<DbActionsResponse> CreateReply(ThreadReply reply);
        Task<DbActionsResponse> UpdateReply(long replyId, string newContent);
        Task<DbActionsResponse> DeleteReply(long replyId);

    }
}
