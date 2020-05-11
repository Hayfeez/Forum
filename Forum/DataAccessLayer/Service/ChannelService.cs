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
    public class ChannelService : IChannelService
    {
        private ApplicationDbContext _dbContext;
        public ChannelService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        #region Channel
        public IEnumerable<Channel> GetAllChannels(int subscriberId)
        {
            try
            {
                return _dbContext.Channels
                    .Include(f => f.Categories).ThenInclude(b=>b.Threads)
                    .Include(a => a.Subscriber)
                    .Where(b => b.Subscriber.Id == subscriberId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Channel GetChannelById(int channelId)
        {
            try
            {
                var f = _dbContext.Channels.Where(a => a.Id == channelId)
                    .Include(f => f.Categories).ThenInclude(a => a.Threads).ThenInclude(b => b.SubscriberUser)
                    .Include(f => f.Categories).ThenInclude(f => f.Threads).ThenInclude(a => a.ThreadReplies).ThenInclude(r => r.SubscriberUser)
                    .FirstOrDefault();

                return f;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Channel GetChannelByName(string name)
        {
            try
            {
                var f = _dbContext.Channels.Where(a => a.Title.ToLower() == name.ToLower())
                    .Include(f => f.Categories).ThenInclude(a => a.Threads).ThenInclude(b => b.SubscriberUser)
                    .Include(f => f.Categories).ThenInclude(f => f.Threads).ThenInclude(a => a.ThreadReplies).ThenInclude(r => r.SubscriberUser)
                    .FirstOrDefault();

                return f;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> CreateChannel(Channel channel)
        {
            try
            {
                if (_dbContext.Channels.Any(a => a.Title == channel.Title && a.Subscriber.Id == channel.Subscriber.Id))
                    return DbActionsResponse.DuplicateExist;

                _dbContext.Channels.Add(channel);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public async Task<DbActionsResponse> UpdateChannel(Channel channel)
        {
            try
            {
                var chann = _dbContext.Channels.Where(a => a.Id == channel.Id && a.SubscriberId == channel.SubscriberId).FirstOrDefault();
                if (chann == null) return DbActionsResponse.NotFound;

                if (_dbContext.Channels.Any(a => a.Id != channel.Id && a.Title == channel.Title && a.SubscriberId == channel.SubscriberId))
                    return DbActionsResponse.DuplicateExist;

                chann.Title = channel.Title;
                chann.Description = channel.Description;

                _dbContext.Channels.Update(chann);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DbActionsResponse> DeleteChannel(int channelId, int tenantId)
        {
            try
            {
                var channel = _dbContext.Channels.Where(a => a.Id == channelId && a.SubscriberId == tenantId)
                    .Include(a => a.Categories).FirstOrDefault();
                if (channel == null) return DbActionsResponse.NotFound;

                if (channel.Categories.Count() > 0)
                    return DbActionsResponse.DeleteDenied;

                _dbContext.Channels.Remove(channel);
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


        #region Category
        public IEnumerable<Category> GetAllCategories(int subscriberId)
        {
            try
            {
                return _dbContext.Categories
                    .Include(f => f.Threads)
                    .Include(a => a.Channel).ThenInclude(b=>b.Subscriber)
                    .Where(b => b.Channel.Subscriber.Id == subscriberId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Category> GetAllCategoriesInChannel(int channelId, bool useInclude = true)
        {
            try
            {
                if (useInclude)
                    return _dbContext.Categories
                        .Where(b => b.ChannelId == channelId)
                        .Include(f => f.Threads)
                        .Include(a => a.Channel);

                else
                    return _dbContext.Categories.Where(b => b.ChannelId == channelId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Category GetCategoryById(int categoryId)
        {
            try
            {
                var f = _dbContext.Categories.Where(a => a.Id == categoryId)
                    .Include(a=>a.Channel)
                    .Include(f => f.Threads).ThenInclude(a => a.SubscriberUser)
                    .Include(f => f.Threads).ThenInclude(a => a.ThreadReplies).ThenInclude(r => r.SubscriberUser)
                    .FirstOrDefault();

                return f;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Category GetCategoryByName(string name)
        {
            try
            {
                var f = _dbContext.Categories.Where(a => a.Title.ToLower() == name.ToLower())
                    .Include(a=>a.Channel)
                    .Include(f => f.Threads).ThenInclude(a => a.SubscriberUser)
                    .Include(f => f.Threads).ThenInclude(a => a.ThreadReplies).ThenInclude(r => r.SubscriberUser)
                    .FirstOrDefault();

                return f;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DbActionsResponse> CreateCategory(Category category)
        {
            try
            {
                if (_dbContext.Categories.Any(a => a.Title == category.Title && a.Channel.Id == category.ChannelId))
                    return DbActionsResponse.DuplicateExist;

                _dbContext.Categories.Add(category);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DbActionsResponse> UpdateCategory(Category category)
        {
            try
            {
                var existingCategory = _dbContext.Categories.FirstOrDefault(a => a.Id == category.Id && a.Channel.Id == category.ChannelId);
                if (existingCategory == null) return DbActionsResponse.NotFound;

                if (_dbContext.Categories.Any(a => a.Id != category.Id && a.Title == category.Title))
                    return DbActionsResponse.DuplicateExist;

                existingCategory.Title = category.Title;
                existingCategory.Description = category.Description;

                _dbContext.Categories.Update(existingCategory);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return DbActionsResponse.Success;

                return DbActionsResponse.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DbActionsResponse> DeleteCategory(int categoryId, int tenantId)
        {
            try
            {
                var cat = _dbContext.Categories
                    .Include(a => a.Channel)
                    .Include(a => a.Threads)
                    .FirstOrDefault(a => a.Id == categoryId && a.Channel.SubscriberId == tenantId);

                if (cat == null) return DbActionsResponse.NotFound;

                if (cat.Threads.Count() > 0)
                    return DbActionsResponse.DeleteDenied;

                _dbContext.Categories.Remove(cat);
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
