using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Helpers;

namespace Forum.DataAccessLayer.IService
{
    public interface IAccountService
    {
        IEnumerable<SubscriberUser> GetAllSubscriberUsers(int subscriberId); 
        SubscriberUser GetSubscriberUser(string userId, int subscriberId);
        bool IsUserASubscriberUser(string email, int subscriberId);
        Task<DbActionsResponse> UpdateUserRating(int subscriberId, string ApplicationUserId, double rating);

        Task<DbActionsResponse> CreateSubscriberUser(SubscriberUser user);
        Task<DbActionsResponse> UpdateSubscriberUser(SubscriberUser user);
        Task<DbActionsResponse> DeleteSubscriberUser(long userId);


        IEnumerable<SubscriberInvite> GetAllSubscriberUserInvites(int subscriberId);
        SubscriberInvite GetSubscriberInvite(string Email, int subscriberId);
        Task<DbActionsResponse> CreateSubscriberInvite(SubscriberInvite invite);
        Task<DbActionsResponse> DeleteSubscriberInvite(SubscriberInvite invite);


        ResetPasswordCode GetResetPasswordCode(string Email, int subscriberId);
        Task<DbActionsResponse> CreateResetPasswordCode(ResetPasswordCode reset);
        Task<DbActionsResponse> DeleteResetPasswordCode(ResetPasswordCode reset);

    }
}
