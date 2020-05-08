using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.DataAccessLayer.IService;
using Forum.Models;
using Microsoft.EntityFrameworkCore;
using Forum.Helpers;

namespace Forum.DataAccessLayer.Service
{
    public class PinnedPostService :IPinnedPostService
    {
        private readonly ApplicationDbContext _dbContext;
        public PinnedPostService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
  

        public IEnumerable<PinnedPost> GetPinnedPosts(int subscriberId)
        {
            try
            {
                IEnumerable<PinnedPost> pinnedPosts;

                pinnedPosts = _dbContext.PinnedPosts
                       // .Include(a => a.Subscriber)
                        .Where(a => a.SubscriberId == subscriberId)
                        .OrderByDescending(a => a.DateCreated).Take(2);

                if (pinnedPosts.Count() > 0)
                    return pinnedPosts;

                else
                {
                    pinnedPosts.ToList()
                        .Add(new PinnedPost
                        {
                            Title = "Welcome New Users! Please read this before posting!",
                            Content = "Congratulations oh, you have found the Community! Before you make a new topic or post, please read community guidelines.",
                            DateCreated = DateTime.Now
                        });

                    return pinnedPosts;
                }
               

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PinnedPost GetPinnedPostById(string title )
        {
            try
            {
                var topic = _dbContext.PinnedPosts.Where(a => a.Title == title)
                    //.Include(a => a.Subscriber)
                    .FirstOrDefault();

                return topic;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public async Task<DbActionsResponse> CreatePinnedPost(PinnedPost post)
        {
            try
            {
                if (_dbContext.PinnedPosts.Any(a => a.Title == post.Title && a.SubscriberId == post.SubscriberId))
                    return DbActionsResponse.DuplicateExist;

                _dbContext.PinnedPosts.Add(post);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> DeletePost(long postId)
        {
            try
            {
                var post = _dbContext.PinnedPosts.Where(a => a.Id == postId).FirstOrDefault();
                if (post == null) return DbActionsResponse.NotFound;

                
                _dbContext.PinnedPosts.Remove(post);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async  Task<DbActionsResponse> UpdatePost(long postId, string newContent)
        {
            try
            {
                var existingPost = _dbContext.PinnedPosts.Where(a => a.Id == postId).FirstOrDefault();
                if (existingPost == null) return DbActionsResponse.NotFound;
             
                existingPost.Content = newContent;

                _dbContext.PinnedPosts.Update(existingPost);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
