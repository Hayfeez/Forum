using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Models;
using static Forum.Helpers.BaseClass;

namespace Forum.DataAccessLayer.IService
{
    public interface IChannelService
    {
        IEnumerable<Channel> GetAllChannels(int subscriberId);       
        Channel GetChannelById(int channelId);

        Task<DbActionsResponse> CreateChannel(Channel channel);
        Task<DbActionsResponse> UpdateChannel(Channel channel);
        Task<DbActionsResponse> DeleteChannel(int channelId);

        IEnumerable<Category> GetAllCategories(int subscriberd);
        IEnumerable<Category> GetAllCategoriesInChannel(int channelId);
        Category GetCategoryById(int categoryId);
        Task<DbActionsResponse> CreateCategory(Category cat);
        Task<DbActionsResponse> UpdateCategory(Category cat);
        Task<DbActionsResponse> DeleteCategory(int categoryId);
    }

}
