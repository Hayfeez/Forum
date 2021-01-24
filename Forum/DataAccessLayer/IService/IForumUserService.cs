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
        Task<DbActionsResponse> SaveUserThreadInfo(SaveUserAction model, long subscriberUserId);

        IEnumerable<UserFollower> GetUserFollowings(long subscriberUserId);

        IEnumerable<UserFollower> GetUserFollowers(long subscriberUserId);

        bool IsUserFollowingPerson(long subscriberUserId, long personId);

        Task<DbActionsResponse> FollowUser(long userId, long personId);
        Task<DbActionsResponse> UnfollowUser(long userId, long personId);

        SubscriberUser GetUserProfile(long userId);

    }
}
