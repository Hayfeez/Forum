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


        public UserPeopleInfo GetUserPeopleInfo(long subscriberUserId, long personId)
        {
            try
            {
                var data = _dbContext.UserPeopleInfos
                    .Where(a=>a.SubscriberUserId == subscriberUserId && a.PersonId == personId)
                      //  .Include(a => a.UserFollowing)
                       // .Include(a => a.SubscriberUser)
                        .FirstOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<UserPeopleInfo> GetUserPeopleInfos(long subscriberUserId)
        {
            try
            {
                var data = _dbContext.UserPeopleInfos
                    .Where(a => a.SubscriberUserId == subscriberUserId);
                      //  .Include(a => a.UserFollowing);
                      //  .Include(a => a.SubscriberUser);

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
                    .Where(a => a.SubscriberUserId == subscriberUserId && a.ThreadId == threadId)
                      //  .Include(a => a.Threads)
                       // .Include(a => a.SubscriberUser)
                        .FirstOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> SaveUserFollowing(SaveUserAction model)
        {
            try
            {
                var dt = _dbContext.UserPeopleInfos.FirstOrDefault(a => a.SubscriberUserId == model.SubscriberUserId && a.PersonId == model.UserFollowingId);

                if (dt == null)
                {
                    _dbContext.UserPeopleInfos.Add(new UserPeopleInfo
                    {   PersonId = model.UserFollowingId,
                        SubscriberUserId = model.SubscriberUserId,
                        Followed = (model.Action == UserAction.Follow) });

                    if (await _dbContext.SaveChangesAsync() > 0)
                        return DbActionsResponse.Success;

                    return DbActionsResponse.Failed;
                }

                else
                {
                    if (model.Action == UserAction.Follow)
                        dt.Followed = !dt.Followed;

                    _dbContext.UserPeopleInfos.Update(dt);
                    if (await _dbContext.SaveChangesAsync() > 0)
                        return DbActionsResponse.Success;

                    return DbActionsResponse.Failed;
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> SaveUserThreadInfo(SaveUserAction model)
        {
            try
            {
                var dt = _dbContext.UserThreadInfos.FirstOrDefault(a => a.SubscriberUserId == model.SubscriberUserId && a.ThreadId == model.ThreadId);

                if (dt == null)
                {
                    _dbContext.UserThreadInfos.Add(new UserThreadInfo
                    {
                        ThreadId = model.ThreadId,
                        SubscriberUserId = model.SubscriberUserId,
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
