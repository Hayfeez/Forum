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

        IEnumerable<SubCategory> GetAllCategoriesInChannel(int channelId);
        SubCategory GetSubCategoryById(int subcategoryId);
        Task<DbActionsResponse> CreateSubCategory(SubCategory subCat);
        Task<DbActionsResponse> UpdateSubCategory(SubCategory subCat);
        Task<DbActionsResponse> DeleteSubCategory(int subCategoryId);
    }

}
