using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Helpers;
using Forum.Models;
using Forum.ViewModels;

namespace Forum.DataAccessLayer.IService
{
    public interface ITenantService
    {
        #region Tenant

        bool DoesTenantExist(string domain);
        Subscriber GetSubscriber(string name);
        IEnumerable<Subscriber> GetAllSubscribers();
        IEnumerable<Subscriber> SearchSubscriber(string searchQuery);
        Task<DbActionsResponse> AddSubscriber(Subscriber model);


        Task<DbActionsResponse> UpdateSubscriber(int subscriberId, SaveSubscriberInfo model);     
        Task<DbActionsResponse> DeleteSubscriber(string domain);
        Task<DbActionsResponse> CancelInvite(int subscriberId, long inviteId);
        IEnumerable<SubscriberUser> GetAllTenantUsers(int subscriberId);
        IEnumerable<SubscriberInvite> GetAllTenantInvites(int subscriberId);

        #endregion

        #region Tenant Users

        Task<DbActionsResponse> UpdateUserRole(int subscriberId, string applicationUserId, string role);
        Task<DbActionsResponse> UpdateUserRating(int subscriberId, string ApplicationUserId, double rating);
        Task<DbActionsResponse> DeleteTenantUser(int subscriberId, string appUserId);


        #endregion

    }
}
