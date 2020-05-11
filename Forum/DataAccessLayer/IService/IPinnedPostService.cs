using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Helpers;

namespace Forum.DataAccessLayer.IService
{
    public interface IPinnedPostService
    {
        IEnumerable<PinnedPost> GetPinnedPosts(int subscriberId);
        PinnedPost GetPinnedPostById(int postId);

        Task<DbActionsResponse> CreatePinnedPost(PinnedPost post);
        Task<DbActionsResponse> UpdatePost(long postId, string newContent, int tenantId);
        Task<DbActionsResponse> DeletePost(long postId, int tenantId);
        Task<DbActionsResponse> ToggleActive(long postId, int tenantId);

    }
}
