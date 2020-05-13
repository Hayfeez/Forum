using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Helpers;
using Forum.ViewModels;

namespace Forum.DataAccessLayer.IService
{
    public interface IForumUserService
    {
        IEnumerable<UserThreadInfo> GetUserThreadActions(long subscriberUserId);
        UserThreadInfo GetUserThreadInfo(long subscriberUserId, long threadId);              
        Task<DbActionsResponse> SaveUserThreadInfo(SaveUserAction model);

        IEnumerable<UserPeopleInfo> GetUserPeopleInfos(long subscriberUserId);
        UserPeopleInfo GetUserPeopleInfo(long subscriberUserId, long personId);
        Task<DbActionsResponse> SaveUserFollowing(SaveUserAction model);


    }
}
