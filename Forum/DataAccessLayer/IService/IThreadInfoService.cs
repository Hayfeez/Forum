using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Helpers;
using Forum.ViewModels;

namespace Forum.DataAccessLayer.IService
{
    public interface IThreadInfoService
    {
        ThreadInfoVM GetThreadInfo (long threadId);
        int GetThreadViews(long threadId);
        Task<DbActionsResponse> IncreaseThreadView(long threadId);
  
        Task<DbActionsResponse> SaveThreadReplyInfo(SaveUserAction model, long subscriberUserId);

    }
}
