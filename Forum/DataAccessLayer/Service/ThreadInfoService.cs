using System;
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
    public class ThreadInfoService :IThreadInfoService
    {
        private readonly ApplicationDbContext _dbContext;
        public ThreadInfoService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public ThreadInfoVM GetThreadInfo(long threadId)
        {
            try
            {
                var data = _dbContext.UserThreadInfos
                        .Where(a => a.ThreadId == threadId);

                return new ThreadInfoVM {
                    Flags = data.Count(a => a.Flagged),
                    Follows = data.Count(a => a.Followed),
                    Likes = data.Count(a => a.Liked),
                    Bookmarks = data.Count(a => a.Bookmarked)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetThreadViews(long threadId)
        {
            try
            {
                var data = _dbContext.ThreadInfos
                        .FirstOrDefault(a => a.ThreadId == threadId);

                return data.Views;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> IncreaseThreadView(long threadId)
        {
            try
            {
                var dt = _dbContext.ThreadInfos.FirstOrDefault(a => a.ThreadId == threadId);

                if (dt == null)
                {
                    _dbContext.ThreadInfos.Add(new ThreadInfo
                    {
                        ThreadId = threadId,
                        Views = 1
                    }); 

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        return DbActionsResponse.Success;
                    }

                    return DbActionsResponse.Failed;
                }

                else
                {
                    dt.Views = dt.Views++;

                    _dbContext.ThreadInfos.Update(dt);
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

        public async Task<DbActionsResponse> SaveThreadReplyInfo(SaveUserAction model, long userId)
        {
            try
            {
                var dt = _dbContext.ThreadReplyInfos.FirstOrDefault(a => a.ThreadReplyId == model.ThreadReplyId);

                if (dt == null)
                {
                    _dbContext.ThreadReplyInfos.Add(new ThreadReplyInfo
                    {
                        ThreadReplyId = model.ThreadReplyId,
                        Shares =  (model.Action == UserAction.Share ? 1 : 0),
                        Upvote = model.Action == UserAction.Upvote ? 1 : 0,
                        Downvote = model.Action == UserAction.Downvote ? 1 : 0,
                    });

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        return DbActionsResponse.Success;
                    }

                    return DbActionsResponse.Failed;
                }

                else
                {
                    if (model.Action == UserAction.Share)
                        dt.Shares = dt.Shares++;
                    if (model.Action == UserAction.Upvote)
                        dt.Upvote = dt.Upvote++;
                    if (model.Action == UserAction.Downvote)
                        dt.Downvote = dt.Downvote++;

                    _dbContext.ThreadReplyInfos.Update(dt);
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
