﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.DataAccessLayer.IService;
using Forum.Helpers;
using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.DataAccessLayer.Service
{
    public class AccountService : IAccountService
    {
        private ApplicationDbContext _dbContext;
        public AccountService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       

        #region Subscriber Invite

        public IEnumerable<SubscriberInvite> GetAllSubscriberUserInvites(int subscriberId)
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

        public SubscriberInvite GetSubscriberInvite(string Email, int subscriberId)
        {
            try
            {
                return _dbContext.SubscriberInvites
                    .SingleOrDefault(a => a.SubscriberId == subscriberId && a.Email == Email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseClass.DbActionsResponse> CreateSubscriberInvite(SubscriberInvite invite)
        {
            try
            {
                if (_dbContext.SubscriberInvites.Any(a => a.SubscriberId == invite.SubscriberId && a.Email == invite.Email))
                    return BaseClass.DbActionsResponse.DuplicateExist;


                _dbContext.SubscriberInvites.Add(invite);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return BaseClass.DbActionsResponse.Success;

                return BaseClass.DbActionsResponse.Failed;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public async Task<BaseClass.DbActionsResponse> DeleteSubscriberInvite(SubscriberInvite invite)
        {
            try
            {
                var existingInvite = _dbContext.SubscriberInvites.SingleOrDefault(a => a.SubscriberId == invite.SubscriberId && a.Email == invite.Email);
                if (existingInvite == null)
                    return BaseClass.DbActionsResponse.NotFound;


                _dbContext.SubscriberInvites.Remove(existingInvite);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return BaseClass.DbActionsResponse.Success;

                return BaseClass.DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion


        #region Subscriber User

        public IEnumerable<SubscriberUser> GetAllSubscriberUsers(int subscriberId)
        {
            try
            {
                return _dbContext.SubscriberUsers
                     .Where(a => a.SubscriberId == subscriberId && a.IsActive).Include(a => a.Subscriber).Include(a=>a.ApplicationUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public SubscriberUser GetSubscriberUser(string userAppId, int subscriberId)
        {
            try
            {
                return _dbContext.SubscriberUsers
                    .Include(a => a.Subscriber).Include(a => a.ApplicationUser)
                      .SingleOrDefault(a => a.ApplicationUserId == userAppId && a.SubscriberId == subscriberId && a.IsActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsUserASubscriberUser(string Email, int subscriberId)
        {
            try
            {
                return _dbContext.SubscriberUsers
                      .Any(a => a.Email == Email && a.SubscriberId == subscriberId && a.IsActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<BaseClass.DbActionsResponse> CreateSubscriberUser(SubscriberUser user)
        {
            try
            {
                if (_dbContext.SubscriberUsers.Any(a => a.SubscriberId == user.SubscriberId
                            && a.ApplicationUserId == user.ApplicationUserId && a.IsActive))
                    return BaseClass.DbActionsResponse.DuplicateExist;

                _dbContext.SubscriberUsers.Add(user);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return BaseClass.DbActionsResponse.Success;

                return BaseClass.DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseClass.DbActionsResponse> DeleteSubscriberUser(long userId)
        {
            try
            {
                var existingUser = _dbContext.SubscriberUsers.SingleOrDefault(a => a.Id == userId && a.IsActive);
                if (existingUser == null)
                    return BaseClass.DbActionsResponse.NotFound;

                existingUser.IsActive = false;
                _dbContext.SubscriberUsers.Update(existingUser);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return BaseClass.DbActionsResponse.Success;

                return BaseClass.DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<BaseClass.DbActionsResponse> UpdateSubscriberUser(SubscriberUser user)
        {
            try
            {
                var existingUser = _dbContext.SubscriberUsers.SingleOrDefault(a => a.Id == user.Id && a.IsActive);
                if (existingUser == null)
                    return BaseClass.DbActionsResponse.NotFound;

                existingUser.HeaderImageUrl = user.HeaderImageUrl;
                existingUser.ProfileImageUrl = user.ProfileImageUrl;
                
                _dbContext.SubscriberUsers.Update(existingUser);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return BaseClass.DbActionsResponse.Success;

                return BaseClass.DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseClass.DbActionsResponse> UpdateUserRating(int subscriberId, string appUserId, double rating)
        {
            try
            {
                var existingUser = _dbContext.SubscriberUsers.SingleOrDefault(a => a.SubscriberId == subscriberId && a.ApplicationUserId == appUserId && a.IsActive);
                if (existingUser == null)
                    return BaseClass.DbActionsResponse.NotFound;

                existingUser.Rating = rating;

                _dbContext.SubscriberUsers.Update(existingUser);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return BaseClass.DbActionsResponse.Success;

                return BaseClass.DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


    }
}
