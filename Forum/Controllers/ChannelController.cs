using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Forum.Models;
using Forum.ViewModels;
using Forum.DataAccessLayer.IService;


namespace Forum.Controllers
{
    public class ChannelController : Controller
    {
       // private readonly ILogger<SubCategory> _logger;
        private readonly IChannelService _channelService;
        private readonly IThreadService _threadService;
        private readonly int _subscriberId = 1;

        public ChannelController(IChannelService channelService, IThreadService threadService)
        {
          //  _logger = logger;
            _channelService = channelService;
            _threadService = threadService;
        }

        public IActionResult Index()
        {
            try
            {
                var channels = _channelService.GetAllChannels(_subscriberId)
                    .Select(a => new ChannelVM
                    {
                        Id = a.Id,
                        Name = a.Title,
                        Description = a.Description,
                        LogoUrl = a.LogoUrl,
                        SubCategoryCount = a.SubCategories.Count(),
                        SubCategory = a.SubCategories.Select(b=>new SubCategoryVM {
                            Id = b.Id,
                            Description = b.Description,
                            Name = b.Title,
                            ThreadCount = b.Threads.Count(),
                            Threads = b.Threads.Select(c=> new ThreadVM
                            {
                                Id = c.Id,
                                AuthorId = c.AppUser.Id,
                                AuthorImageUrl = c.AppUser.ProfileImageUrl,
                                AuthorName = c.AppUser.UserName,
                                AuthorRating = c.AppUser.Rating,
                                DatePosted = c.DateCreated,
                                Title = c.Title,
                                Content = c.Content,
                               // SubCategory = null,
                               // Replies = null,
                               // RepliesCount = null
                            })                            
                        })                    
                    });

                return View(new ChannelList { Channels = channels });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ChannelThread(int channelId)
        {
            try
            {
                var channel = _channelService.GetChannelById(channelId);
                var threads = _threadService.GetAllThreadsInChannel(channelId, null);
                var threadList = threads.Select(a => new ChannelThreads
                {
                    Channel = new ChannelVM
                    {
                        Id = channel.Id,
                        Name = channel.Title,
                        LogoUrl = channel.LogoUrl,
                        Description = channel.Description,
                        SubCategory = channel.SubCategories.Select(a=>new SubCategoryVM {
                            Id = a.Id,
                            Name = a.Title,
                            Description = a.Description,
                            ThreadCount = a.Threads.Count()
                            
                        }),
                        SubCategoryCount = channel.SubCategories.Count()
                    },                    
                    Threads = a.SubCategory.Threads.Select(b => new ThreadVM
                    {
                        Id = a.Id,
                        AuthorId = a.AppUser.Id,
                        AuthorName = a.AppUser.UserName,
                        AuthorRating = a.AppUser.Rating,
                        Title = a.Title,
                        Content = a.Content,
                        DatePosted = a.DateCreated,
                        RepliesCount = a.ThreadReplies.Count(),
                    })
                });

                return View(threadList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult SubCategoryThread(int subCategoryId)
        {
            try
            {
                var subCat = _channelService.GetSubCategoryById(subCategoryId);
                var threads = _threadService.GetAllThreadsInChannel(subCat.Channel.Id, subCategoryId);
                var threadList = threads.Select(a => new ChannelThreads
                {                
                    SubCategory = new SubCategoryVM
                    {
                        Id = a.SubCategory.Id,
                        Channel = new ChannelVM
                        {
                            Id = a.SubCategory.Channel.Id,
                            Name = a.SubCategory.Channel.Title,
                            LogoUrl = a.SubCategory.Channel.LogoUrl,
                            Description = a.SubCategory.Channel.Description
                        },
                        Description = a.SubCategory.Description,
                        Name = a.SubCategory.Title,
                        ThreadCount = a.SubCategory.Threads.Count(),
                    },
                    Threads = a.SubCategory.Threads.Select(b => new ThreadVM
                    {
                        Id = a.Id,
                        AuthorId = a.AppUser.Id,
                        AuthorName = a.AppUser.UserName,
                        AuthorRating = a.AppUser.Rating,
                        Title = a.Title,
                        Content = a.Content,
                        DatePosted = a.DateCreated,
                        RepliesCount = a.ThreadReplies.Count(),
                    })
                });

                return View(threadList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
