using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Helpers;

namespace Forum.DataAccessLayer.IService
{
    public interface IAccountService
    {
        SubscriberUser GetSubscriberUser(string userId, int subscriberId);
        bool IsUserASubscriberUser(string email, int subscriberId);
        Task<DbActionsResponse> CreateSubscriberUser(SubscriberUser user);
        Task<DbActionsResponse> UpdateSubscriberUser(SubscriberUser user);


        SubscriberInvite GetSubscriberInvite(string Email, int subscriberId);
        Task<DbActionsResponse> CreateSubscriberInvite(SubscriberInvite invite, bool isAdmin);
        Task<DbActionsResponse> DeleteSubscriberInvite(SubscriberInvite invite);


        ResetPasswordCode GetResetPasswordCode(string Email, int subscriberId);
        Task<DbActionsResponse> CreateResetPasswordCode(ResetPasswordCode reset);
        Task<DbActionsResponse> DeleteResetPasswordCode(ResetPasswordCode reset);

    }
}
