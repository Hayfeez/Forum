using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.DataAccessLayer.IService;
using Forum.Helpers;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Forum.DataAccessLayer.Service
{
    public class TenantService : ITenantService
    {
        private ApplicationDbContext _dbContext;
        public TenantService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        #region Tenant

        public bool DoesTenantExist(string domain)
        {
            try
            {
                return _dbContext.Tenants.Any(t => t.Domain == domain && t.IsActive);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Subscriber> SearchSubscriber(string searchQuery)
        {
            try
            {
                return _dbContext.Tenants
                                  .Where(a => a.Name.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase)
                                        || a.Domain.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase)
                                        && a.IsPublic && a.IsActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> AddSubscriber(Subscriber model)
        {
            try
            {
                if (_dbContext.Tenants.Any(t => t.Domain.Equals(model.Domain, StringComparison.CurrentCultureIgnoreCase)
                         || t.Name.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase)))
                    return DbActionsResponse.DuplicateExist;

                model.IsActive = true;
                _dbContext.Tenants.Add(model);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Subscriber> GetAllSubscribers()
        {
            try
            {
                return _dbContext.Tenants.Include(a => a.SubscriberUsers);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Subscriber GetSubscriber(string name)
        {
            try
            {
                return _dbContext.Tenants
                                .SingleOrDefault(a => a.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<SubscriberInvite> GetAllTenantInvites(int subscriberId)
        {
            try
            {
                return _dbContext.SubscriberInvites
                    .Where(a => a.SubscriberId == subscriberId).Include(a => a.Subscriber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<SubscriberUser> GetAllTenantUsers(int subscriberId)
        {
            try
            {
                return _dbContext.SubscriberUsers
                     .Where(a => a.SubscriberId == subscriberId && a.IsActive).Include(a => a.Subscriber).Include(a => a.ApplicationUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> UpdateSubscriber(int subscriberId, SaveSubscriberInfo model)
        {
            try
            {
                var existing = _dbContext.Tenants.SingleOrDefault(a => a.Id == subscriberId);
                if (existing == null) return DbActionsResponse.NotFound;

                if (_dbContext.Tenants.Any(t => t.Name.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase)
                         && t.Id != existing.Id))
                    return DbActionsResponse.DuplicateExist;

                existing.Name = model.Name;
                existing.Description = model.Description;
                existing.HeaderImageUrl = model.HeaderImageUrl;
                existing.LogoImageUrl = model.LogoImageUrl;
                existing.IsPublic = model.IsPublic;
                existing.DownvoteLimit = model.DownvoteLimit;
                existing.FlagLimit = model.FlagLimit;

                _dbContext.Tenants.Update(existing);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> DeleteSubscriber(string domain)
        {
            try
            {
                var existing = _dbContext.Tenants.SingleOrDefault(a => a.Domain.Equals(domain, StringComparison.CurrentCultureIgnoreCase));
                if (existing == null) return DbActionsResponse.NotFound;

                existing.IsActive = false;

                _dbContext.Tenants.Update(existing);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DbActionsResponse> CancelInvite(int subscriberId, long inviteId)
        {
            try
            {
                var existingInvite = _dbContext.SubscriberInvites.SingleOrDefault(a => a.SubscriberId == subscriberId && a.Id == inviteId);
                if (existingInvite == null)
                    return DbActionsResponse.NotFound;


                _dbContext.SubscriberInvites.Remove(existingInvite);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region TenantUsers

        public async Task<DbActionsResponse> DeleteTenantUser(int subscriberId, string appUserId)
        {
            try
            {
                var existingUser = _dbContext.SubscriberUsers.SingleOrDefault(a => a.SubscriberId == subscriberId && a.ApplicationUserId == appUserId && a.IsActive);
                if (existingUser == null)
                    return DbActionsResponse.NotFound;

                existingUser.IsActive = false;
                _dbContext.SubscriberUsers.Update(existingUser);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> UpdateUserRating(int subscriberId, string appUserId, double rating)
        {
            try
            {
                var existingUser = _dbContext.SubscriberUsers.SingleOrDefault(a => a.SubscriberId == subscriberId && a.ApplicationUserId == appUserId && a.IsActive);
                if (existingUser == null)
                    return DbActionsResponse.NotFound;

                existingUser.Rating = rating;

                _dbContext.SubscriberUsers.Update(existingUser);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> UpdateUserRole(int subscriberId, string applicationUserId, string role)
        {
            try
            {
                var existing = _dbContext.SubscriberUsers.SingleOrDefault(a => a.ApplicationUserId == applicationUserId && a.SubscriberId == subscriberId);
                if (existing == null) return DbActionsResponse.NotFound;

                existing.UserRole = role;

                _dbContext.SubscriberUsers.Update(existing);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

    }
}
