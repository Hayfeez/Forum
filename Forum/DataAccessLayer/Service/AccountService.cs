using System;
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

        public async Task<DbActionsResponse> CreateSubscriberInvite(SubscriberInvite invite, bool isAdmin)
        {
            try
            {
                if (!isAdmin &&_dbContext.Tenants.SingleOrDefault(a => a.Id == invite.SubscriberId)?.AllowJoinNow != true)
                {
                    return DbActionsResponse.DeleteDenied;
                }

                if (_dbContext.SubscriberInvites.Any(a => a.SubscriberId == invite.SubscriberId && a.Email == invite.Email))
                    return DbActionsResponse.DuplicateExist;


                _dbContext.SubscriberInvites.Add(invite);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<DbActionsResponse> DeleteSubscriberInvite(SubscriberInvite invite)
        {
            try
            {
                var existingInvite = _dbContext.SubscriberInvites.SingleOrDefault(a => a.SubscriberId == invite.SubscriberId && a.Email == invite.Email);
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


        #region Subscriber User

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

        public async Task<DbActionsResponse> CreateSubscriberUser(SubscriberUser user)
        {
            try
            {
                if (_dbContext.SubscriberUsers.Any(a => a.SubscriberId == user.SubscriberId
                            && a.ApplicationUserId == user.ApplicationUserId && a.IsActive))
                    return DbActionsResponse.DuplicateExist;

                _dbContext.SubscriberUsers.Add(user);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        public async Task<DbActionsResponse> UpdateSubscriberUser(SubscriberUser user)
        {
            try
            {
                var existingUser = _dbContext.SubscriberUsers.SingleOrDefault(a => a.Id == user.Id && a.IsActive);
                if (existingUser == null)
                    return DbActionsResponse.NotFound;
                
                existingUser.ProfileImageUrl = user.ProfileImageUrl;
                
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

        public SubscriberUser GetSubscriberUserWithEmail(string email, int subscriberId)
        {
            try
            {
                return _dbContext.SubscriberUsers
                      .SingleOrDefault(a => a.Email == email && a.SubscriberId == subscriberId && a.IsActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DbActionsResponse> ResetSubscriberUserPassword(string email, string newPassword)
        {
            try
            {
                var existingUser = _dbContext.SubscriberUsers.SingleOrDefault(a => a.Email == email && a.IsActive);
                if (existingUser == null)
                    return DbActionsResponse.NotFound;

                existingUser.Password = newPassword;

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
        public async Task<DbActionsResponse> ChangeSubscriberUserPassword(string email, string oldPassword, string newPassword)
        {
            try
            {
                var existingUser = _dbContext.SubscriberUsers.SingleOrDefault(a => a.Email == email && a.IsActive);
                if (existingUser == null)
                    return DbActionsResponse.NotFound;

                if (existingUser.Password != oldPassword) return DbActionsResponse.DeleteDenied;

                existingUser.Password = newPassword;

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

        #endregion


        #region ResetPassword Code  

        public ResetPasswordCode GetResetPasswordCode(string Email, int subscriberId)
        {
            try
            {
                return _dbContext.ResetPasswordCodes
                    .SingleOrDefault(a => a.SubscriberId == subscriberId && a.Email == Email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> CreateResetPasswordCode(ResetPasswordCode reset)
        {
            try
            {
                if (_dbContext.ResetPasswordCodes.Any(a => a.SubscriberId == reset.SubscriberId && a.Email == reset.Email))
                    return DbActionsResponse.DuplicateExist;


                _dbContext.ResetPasswordCodes.Add(reset);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<DbActionsResponse> DeleteResetPasswordCode(ResetPasswordCode reset)
        {
            try
            {
                var existingCode = _dbContext.ResetPasswordCodes.SingleOrDefault(a => a.SubscriberId == reset.SubscriberId && a.Email == reset.Email);
                if (existingCode == null)
                    return DbActionsResponse.NotFound;


                _dbContext.ResetPasswordCodes.Remove(existingCode);
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
