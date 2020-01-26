using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.DataAccessLayer.IService;
using Forum.Models;
using Microsoft.EntityFrameworkCore;
using static Forum.CommonClasses.BaseClass;

namespace Forum.DataAccessLayer.Service
{
    public class ChannelService : IChannelService
    {
        private ApplicationDbContext _dbContext;
        public ChannelService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DbActionsResponse> CreateChannel(Channel channel)
        {
            try
            {
                try
                {
                    if (_dbContext.Channels.Any(a => a.Title == channel.Title && a.Subscriber.Id == channel.Subscriber.Id))
                        return DbActionsResponse.DuplicateExist;

                    _dbContext.Channels.Add(channel);
                    await _dbContext.SaveChangesAsync();

                    return DbActionsResponse.Success;
                    
                }
                catch (Exception ex)
                {
                    return DbActionsResponse.Error;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DbActionsResponse> CreateSubCategory(SubCategory subCat)
        {
            try
            {
                if (_dbContext.SubCategories.Any(a => a.Title == subCat.Title && a.Channel.Id == subCat.Channel.Id))
                    return DbActionsResponse.DuplicateExist;

                _dbContext.SubCategories.Add(subCat);
                await _dbContext.SaveChangesAsync();
                return DbActionsResponse.Success;
            }
            catch (Exception ex)
            {
                return DbActionsResponse.Error;
            }
        }

        public async Task<DbActionsResponse> DeleteChannel(int channelId)
        {
            try
            {
                var channel = _dbContext.Channels.Where(a => a.Id == channelId).Include(a=>a.SubCategories).FirstOrDefault();
                if (channel == null) return DbActionsResponse.DuplicateExist;

                if (channel.SubCategories.Count() > 0)
                    return DbActionsResponse.DeleteDenied;

                _dbContext.Channels.Remove(channel);
                await _dbContext.SaveChangesAsync();
                return DbActionsResponse.Success;
            }
            catch (Exception ex)
            {
                return DbActionsResponse.Error;
            }
        }

        public async Task<DbActionsResponse> DeleteSubCategory(int subCatId)
        {
            try
            {
                var cat = _dbContext.SubCategories.Where(a => a.Id == subCatId).Include(a => a.Threads).FirstOrDefault();
                if (cat == null) return DbActionsResponse.DuplicateExist;

                if (cat.Threads.Count() > 0)
                    return DbActionsResponse.DeleteDenied;

                _dbContext.SubCategories.Remove(cat);
                 await _dbContext.SaveChangesAsync();
                return DbActionsResponse.Success;
            }
            catch (Exception ex)
            {
                return DbActionsResponse.Error;
            }
        }

        public IEnumerable<SubCategory> GetAllCategoriesInChannel(int channelId)
        {
            try
            {
                return _dbContext.SubCategories
                    .Include(f => f.Threads)
                    .Include(a => a.Channel)
                    .Where(b => b.Channel.Id == channelId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<Channel> GetAllChannels(int subscriberId)
        {
            try
            {
                return _dbContext.Channels
                    .Include(f => f.SubCategories)
                    .Include(a => a.Subscriber)
                    .Where(b => b.Subscriber.Id == subscriberId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Channel GetChannelById(int channelId)
        {
            try
            {
                var f =  _dbContext.Channels.Where(a=>a.Id == channelId)
                    .Include(f => f.SubCategories).ThenInclude(a=>a.Threads).ThenInclude(b=>b.AppUser)
                    .Include(f=>f.SubCategories).ThenInclude(f => f.Threads).ThenInclude(a => a.ThreadReplies).ThenInclude(r => r.AppUser)
                    .FirstOrDefault();

                return f;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SubCategory GetSubCategoryById(int subCatId)
        {
            try
            {
                var f = _dbContext.SubCategories.Where(a => a.Id == subCatId)
                    .Include(f => f.Threads).ThenInclude(a => a.AppUser)
                    .Include(f => f.Threads).ThenInclude(a => a.ThreadReplies).ThenInclude(r => r.AppUser)
                    .FirstOrDefault();

                return f;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<DbActionsResponse> UpdateSubCategory(SubCategory subCat)
        {
            try
            {
                var cat = _dbContext.SubCategories.Where(a => a.Id == subCat.Id).FirstOrDefault();
                if (cat == null) return DbActionsResponse.NotFound;

                if (_dbContext.SubCategories.Any(a => a.Id != subCat.Id && a.Title == subCat.Title))
                    return DbActionsResponse.DuplicateExist;

                cat.Title = subCat.Title;
                cat.Description = subCat.Description;                

                _dbContext.SubCategories.Update(cat);
                 await _dbContext.SaveChangesAsync();
                return DbActionsResponse.Success;
            }
            catch (Exception ex)
            {
                return DbActionsResponse.Error;
            }
        }

        public async Task<DbActionsResponse> UpdateChannel(Channel channel)
        {
            try
            {
                var  chann = _dbContext.Channels.Where(a => a.Id == channel.Id).FirstOrDefault();
                if (chann == null) return DbActionsResponse.NotFound;

                if (_dbContext.Channels.Any(a => a.Id != channel.Id && a.Title == channel.Title))
                    return DbActionsResponse.DuplicateExist;

                chann.Title = channel.Title;
                chann.Description = channel.Description;
                chann.LogoUrl = channel.LogoUrl;


                _dbContext.Channels.Update(chann);
                await _dbContext.SaveChangesAsync();
                return DbActionsResponse.Success;
            }
            catch (Exception ex)
            {
                return DbActionsResponse.Error;
            }
        }
    }
}
