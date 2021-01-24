using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Forum.Models;
using Forum.ViewModels;

namespace Forum.Helpers
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterVM, SubscriberUser>()
               .ForMember(dest => dest.UserRole, opt => { opt.MapFrom(d => UserRoles.User); })
               .ForMember(dest => dest.DateJoined, opt => { opt.MapFrom(d => DateTime.Now); })
               .ForMember(dest => dest.IsActive, opt => { opt.MapFrom(d => true); })
               .ForMember(dest => dest.Rating, opt => { opt.MapFrom(d => 0.1); });

            CreateMap<SubscriberUser, TenantUsersList>();
            CreateMap<SubscriberInvite, TenantInviteList>();
            CreateMap<SaveChannelVM, Channel>().ForMember(dest => dest.DateCreated, opt => { opt.MapFrom(d => DateTime.Now); });
            CreateMap<SaveCategoryVM, Category>().ForMember(dest => dest.DateCreated, opt => { opt.MapFrom(d => DateTime.Now); });
            CreateMap<SaveThreadVM, Thread>().ForMember(dest => dest.DateCreated, opt => { opt.MapFrom(d => DateTime.Now); });
            CreateMap<SaveThreadReplyVM, ThreadReply>().ForMember(dest => dest.DateCreated, opt => { opt.MapFrom(d => DateTime.Now); });
            CreateMap<SavePinnedPostVM, PinnedPost>().ForMember(dest => dest.DateCreated, opt => { opt.MapFrom(d => DateTime.Now); });

            CreateMap<SaveSubscriberInfo, Subscriber>()
                .ForMember(dest => dest.DateCreated, opt => { opt.MapFrom(d => DateTime.Now); })
                .ForMember(dest => dest.IsActive, opt => { opt.MapFrom(d => true); });

            CreateMap<PinnedPost, ThreadVM>()
               .ForMember(dest => dest.DatePosted, opt => { opt.MapFrom(d => d.DateCreated); })
               .ForMember(dest => dest.AuthorName, opt => { opt.MapFrom(d => "Administrator"); });

            CreateMap<Thread, ThreadVM>()
                   .ForMember(dest => dest.DatePosted, opt => { opt.MapFrom(d => d.DateCreated); })
                   .ForMember(dest => dest.AuthorId, opt => { opt.MapFrom(d => d.SubscriberUser.Id); })
                    .ForMember(dest => dest.AuthorImageUrl, opt => { opt.MapFrom(d => d.SubscriberUser.ProfileImageUrl); })
                    .ForMember(dest => dest.AuthorName, opt => { opt.MapFrom(d => d.SubscriberUser.ApplicationUser.UserName); })
                    .ForMember(dest => dest.AuthorRating, opt => { opt.MapFrom(d => d.SubscriberUser.Rating); })
                    .ForMember(dest => dest.Category, opt => { opt.MapFrom(d => d.Category); });

            CreateMap<ThreadReply, ThreadReplyVM>()
                   .ForMember(dest => dest.DatePosted, opt => { opt.MapFrom(d => d.DateCreated); })
                   .ForMember(dest => dest.AuthorId, opt => { opt.MapFrom(d => d.SubscriberUser.ApplicationUserId); })
                    .ForMember(dest => dest.AuthorName, opt => { opt.MapFrom(d => d.SubscriberUser.ApplicationUser.UserName); })
                    .ForMember(dest => dest.AuthorRating, opt => { opt.MapFrom(d => d.SubscriberUser.Rating); })
                    .ForMember(dest => dest.ThreadId, opt => { opt.MapFrom(d => d.ThreadId); })
                    .ForMember(dest => dest.ThreadReplyInfo, opt => { opt.MapFrom(d => d.ThreadReplyInfo); })
                   .ForMember(dest => dest.ReplyInfoUserIds, opt => { opt.MapFrom(d => d.UserThreadReplyInfos.Select(a=>a.SubscriberUserId)); });

            CreateMap<ThreadInfo, ThreadInfoVM>();
            CreateMap<ThreadReplyInfo, ThreadReplyInfoVM>();
            CreateMap<UserThreadInfo, UserThreadInfoVM>();

            CreateMap<SaveUserAction, UserThreadInfo>();
            CreateMap<SaveUserAction, UserFollower>();
            CreateMap<SaveUserAction, ThreadInfo>();
            CreateMap<SaveUserAction, ThreadReplyInfo>();

            CreateMap<UserFollower, UserPeopleInfoVM>()
                 .ForMember(dest => dest.PersonId, opt => { opt.MapFrom(d => d.SubscriberUser.Id); })
                 .ForMember(dest => dest.PersonImageUrl, opt => { opt.MapFrom(d => d.SubscriberUser.ProfileImageUrl); })
                 .ForMember(dest => dest.PersonName, opt => { opt.MapFrom(d => d.SubscriberUser.ApplicationUser.UserName); })
                 .ForMember(dest => dest.DatePersonJoined, opt => { opt.MapFrom(d => d.SubscriberUser.DateJoined); });


        CreateMap<SubscriberUser, ProfileVM>()
                .ForMember(dest => dest.SubscriberUserId, opt => { opt.MapFrom(d => d.Id); })
                .ForMember(dest => dest.Username, opt => { opt.MapFrom(d => d.ApplicationUser.UserName); })
                .ForMember(dest => dest.Email, opt => { opt.MapFrom(d => d.ApplicationUser.Email); })
                .ForMember(dest => dest.ThreadCount, opt => { opt.MapFrom(d => d.Threads.Count()); })
                 .ForMember(dest => dest.ReplyCount, opt => { opt.MapFrom(d => d.ThreadReplies.Count()); })
                .ForMember(dest => dest.FollowingCount, opt => { opt.MapFrom(d => d.UserFollowings.Count()); })
                .ForMember(dest => dest.FollowerCount, opt => { opt.MapFrom(d => 0); })
                .ForMember(dest => dest.Flags, opt => { opt.MapFrom(d => d.UserThreadInfos.Count(a=>a.Flagged)); })
                 .ForMember(dest => dest.Bookmarks, opt => { opt.MapFrom(d => d.UserThreadInfos.Count(a => a.Bookmarked)); })
                .ForMember(dest => dest.Follows, opt => { opt.MapFrom(d => d.UserThreadInfos.Count(a => a.Followed)); })
                .ForMember(dest => dest.Likes, opt => { opt.MapFrom(d => d.UserThreadInfos.Count(a => a.Liked)); });


        CreateMap<Category,  CategoryVM>()
                   .ForMember(dest => dest.Name, opt => { opt.MapFrom(d => d.Title); })
                    .ForMember(dest => dest.Threads, opt => { opt.MapFrom(d => d.Threads); })
                   .ForMember(dest => dest.ThreadCount, opt => { opt.MapFrom(d => d.Threads.Count()); });

            CreateMap<Channel, ChannelVM>()
                    .ForMember(dest => dest.Name, opt => { opt.MapFrom(d => d.Title); })
                     .ForMember(dest => dest.Categories, opt => { opt.MapFrom(d => d.Categories); })
                    .ForMember(dest => dest.CategoryCount, opt => { opt.MapFrom(d => d.Categories.Count()); });

            CreateMap<Channel, ChannelThreadCount>()
                    .ForMember(dest => dest.ChannelTitle, opt => { opt.MapFrom(d => d.Title); })
                     .ForMember(dest => dest.ChannelId, opt => { opt.MapFrom(d => d.Id); })
                    .ForMember(dest => dest.ThreadCount, opt => { opt.MapFrom(d => d.Categories.Sum(a=>a.Threads.Count())); });
        }

    }
}
