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
    public class ForumUserService : IForumUserService
    {

        private readonly ApplicationDbContext _dbContext;
        public ForumUserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SubscriberUser GetUserProfile(long userId)
        {
            try
            {
                var data = _dbContext.SubscriberUsers
                    .Where(a => a.Id == userId)
                        .Include(a => a.ApplicationUser)
                        .Include(a => a.Threads).ThenInclude(b => b.ThreadInfo)
                        .Include(a => a.ThreadReplies)
                        .Include(a => a.UserFollowings)
                        .Include(a => a.UserThreadInfos)
                        .FirstOrDefault();


                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> FollowUser(long userId, long personId)
        {
            try
            {
                if (!_dbContext.UserFollowers.Any(a => a.SubscriberUserId == userId && a.PersonId == personId))
                {
                    _dbContext.UserFollowers.Add(new UserFollower
                    {
                        PersonId = personId,
                        SubscriberUserId = userId
                    });

                    if (await _dbContext.SaveChangesAsync() > 0)
                        return DbActionsResponse.Success;

                    return DbActionsResponse.Failed;
                }

                return DbActionsResponse.DuplicateExist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> UnfollowUser(long subscriberUserId, long personId)
        {
            try
            {
                var data = _dbContext.UserFollowers
                    .SingleOrDefault(a => a.SubscriberUserId == subscriberUserId && a.PersonId == personId);

                if (data == null) return DbActionsResponse.NotFound;
                var response = _dbContext.UserFollowers.Remove(data);

                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsUserFollowingPerson(long subscriberUserId, long personId)
        {
            try
            {
                return _dbContext.UserFollowers
                    .Any(a => a.SubscriberUserId == subscriberUserId && a.PersonId == personId);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public IEnumerable<UserFollower> GetUserFollowings(long subscriberUserId)
        {
            try
            {
                var data = _dbContext.UserFollowers
                    .Where(a => a.SubscriberUserId == subscriberUserId)
                        .Include(a => a.SubscriberUser).ThenInclude(b=>b.ApplicationUser);

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<UserFollower> GetUserFollowers(long subscriberUserId)
        {
            try
            {
                var data = _dbContext.UserFollowers
                    .Where(a => a.PersonId == subscriberUserId)
                        .Include(a => a.SubscriberUser).ThenInclude(b => b.ApplicationUser);

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public IEnumerable<UserThreadInfo> GetUserThreadActions(long subscriberUserId)
        {
            try
            {
                var data = _dbContext.UserThreadInfos
                    .Where(a => a.SubscriberUserId == subscriberUserId)
                        .Include(a => a.Thread);
                       // .Include(a => a.SubscriberUser);

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserThreadInfo GetUserThreadInfo(long subscriberUserId, long threadId)
        {
            try
            {
                var data = _dbContext.UserThreadInfos
                    .FirstOrDefault(a => a.SubscriberUserId == subscriberUserId && a.ThreadId == threadId);

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> SaveUserThreadInfo(SaveUserAction model, long userId)
        {
            try
            {
                var dt = _dbContext.UserThreadInfos.FirstOrDefault(a => a.SubscriberUserId == userId && a.ThreadId == model.ThreadId);

                if (dt == null)
                {
                    _dbContext.UserThreadInfos.Add(new UserThreadInfo
                    {
                        ThreadId = model.ThreadId,
                        SubscriberUserId = userId,
                        Followed = (model.Action == UserAction.Follow),
                        Flagged = model.Action == UserAction.Flag,
                        Bookmarked = model.Action == UserAction.Bookmark,
                        Liked = model.Action == UserAction.Like
                    });

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        return DbActionsResponse.Success;
                    }                       

                    return DbActionsResponse.Failed;
                }

                else
                {
                    if (model.Action == UserAction.Follow)
                        dt.Followed = !dt.Followed;
                    if (model.Action == UserAction.Flag)
                        dt.Flagged = !dt.Flagged;
                    if (model.Action == UserAction.Bookmark)
                        dt.Bookmarked = !dt.Bookmarked;
                    if (model.Action == UserAction.Like)
                        dt.Liked = !dt.Liked;

                    _dbContext.UserThreadInfos.Update(dt);
                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        return DbActionsResponse.Success;
                    }

                    return DbActionsResponse.Failed;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
