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
            CreateMap<SavePinnedPostVM, PinnedPost>().ForMember(dest => dest.DateCreated, opt => { opt.MapFrom(d => DateTime.Now); });

            CreateMap<SaveSubscriberInfo, Subscriber>()
                .ForMember(dest => dest.DateCreated, opt => { opt.MapFrom(d => DateTime.Now); })
                .ForMember(dest => dest.IsActive, opt => { opt.MapFrom(d => true); });

            CreateMap<PinnedPost, ThreadVM>()
               .ForMember(dest => dest.DatePosted, opt => { opt.MapFrom(d => d.DateCreated); })
               .ForMember(dest => dest.AuthorName, opt => { opt.MapFrom(d => "Administrator"); });

            CreateMap<Thread, ThreadVM>()
                   .ForMember(dest => dest.DatePosted, opt => { opt.MapFrom(d => d.DateCreated); })
                   .ForMember(dest => dest.AuthorId, opt => { opt.MapFrom(d => d.SubscriberUser.ApplicationUserId); })
                    .ForMember(dest => dest.AuthorImageUrl, opt => { opt.MapFrom(d => d.SubscriberUser.ProfileImageUrl); })
                    .ForMember(dest => dest.AuthorName, opt => { opt.MapFrom(d => d.SubscriberUser.ApplicationUser.UserName); })
                    .ForMember(dest => dest.AuthorRating, opt => { opt.MapFrom(d => d.SubscriberUser.Rating); })
                    .ForMember(dest => dest.Category, opt => { opt.MapFrom(d => d.Category); })
                     .ForMember(dest => dest.Replies, opt => { opt.MapFrom(d => d.ThreadReplies); })
                    .ForMember(dest => dest.RepliesCount, opt => { opt.MapFrom(d => d.ThreadReplies.Count()); });

            CreateMap<ThreadReply, ThreadReplyVM>()
                   .ForMember(dest => dest.DatePosted, opt => { opt.MapFrom(d => d.DateCreated); })
                   .ForMember(dest => dest.AuthorId, opt => { opt.MapFrom(d => d.SubscriberUser.ApplicationUserId); })
                    .ForMember(dest => dest.AuthorImageUrl, opt => { opt.MapFrom(d => d.SubscriberUser.ProfileImageUrl); })
                    .ForMember(dest => dest.AuthorName, opt => { opt.MapFrom(d => d.SubscriberUser.ApplicationUser.UserName); })
                    .ForMember(dest => dest.AuthorRating, opt => { opt.MapFrom(d => d.SubscriberUser.Rating); })
                    .ForMember(dest => dest.Thread, opt => { opt.MapFrom(d => d.Thread); });

            CreateMap<ThreadInfo, ThreadInfoVM>();
            CreateMap<ThreadReplyInfo, ThreadReplyInfoVM>();
            
            CreateMap<SaveUserAction, UserThreadInfo>();
            CreateMap<SaveUserAction, UserPeopleInfo>();
            CreateMap<SaveUserAction, ThreadInfo>();
            CreateMap<SaveUserAction, ThreadReplyInfo>();

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
