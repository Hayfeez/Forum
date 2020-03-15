using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Helpers;

namespace Forum.DataAccessLayer.IService
{
    public interface IChannelService
    {
        IEnumerable<Channel> GetAllChannels(int subscriberId);       
        Channel GetChannelById(int channelId);
        Channel GetChannelByName(string title);

        Task<DbActionsResponse> CreateChannel(Channel channel);
        Task<DbActionsResponse> UpdateChannel(Channel channel);
        Task<DbActionsResponse> DeleteChannel(int channelId);

        IEnumerable<Category> GetAllCategories(int subscriberd);
        IEnumerable<Category> GetAllCategoriesInChannel(int channelId, bool useInclude = true);
        Category GetCategoryById(int categoryId);
        Category GetCategoryByName(string title);
        Task<DbActionsResponse> CreateCategory(Category cat);
        Task<DbActionsResponse> UpdateCategory(Category cat);
        Task<DbActionsResponse> DeleteCategory(int categoryId);
    }

}
