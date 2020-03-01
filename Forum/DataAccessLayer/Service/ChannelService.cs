using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.DataAccessLayer.IService;
using Forum.Models;
using Microsoft.EntityFrameworkCore;
using static Forum.Helpers.BaseClass;

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
                    .Include(f => f.Categories)
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
                var chann = _dbContext.Channels.Where(a => a.Id == channel.Id).FirstOrDefault();
                if (chann == null) return DbActionsResponse.NotFound;

                if (_dbContext.Channels.Any(a => a.Id != channel.Id && a.Title == channel.Title))
                    return DbActionsResponse.DuplicateExist;

                chann.Title = channel.Title;
                chann.Description = channel.Description;
                chann.LogoUrl = channel.LogoUrl;

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
        public async Task<DbActionsResponse> DeleteChannel(int channelId)
        {
            try
            {
                var channel = _dbContext.Channels.Where(a => a.Id == channelId).Include(a => a.Categories).FirstOrDefault();
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
        public IEnumerable<Category> GetAllCategoriesInChannel(int channelId)
        {
            try
            {
                return _dbContext.Categories
                    .Include(f => f.Threads)
                    .Include(a => a.Channel)
                    .Where(b => b.Channel.Id == channelId);
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
                if (_dbContext.Categories.Any(a => a.Title == category.Title && a.Channel.Id == category.Channel.Id))
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
                var existingCategory = _dbContext.Categories.Where(a => a.Id == category.Id).FirstOrDefault();
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
        public async Task<DbActionsResponse> DeleteCategory(int categoryId)
        {
            try
            {
                var cat = _dbContext.Categories.Where(a => a.Id == categoryId).Include(a => a.Threads).FirstOrDefault();
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
