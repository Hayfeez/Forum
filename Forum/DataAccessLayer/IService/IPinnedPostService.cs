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
        PinnedPost GetPinnedPostById(string title);

        Task<DbActionsResponse> CreatePinnedPost(PinnedPost post);
        Task<DbActionsResponse> UpdatePost(long postId, string newContent);
        Task<DbActionsResponse> DeletePost(long postId);

    }
}
