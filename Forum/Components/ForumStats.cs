using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Forum.DataAccessLayer.IService;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class ForumStats : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly IChannelService _channelService;
        private readonly Subscriber _tenant;
        private readonly IThreadService _threadService;
        private readonly ITenantService _tenantService;

        public ForumStats(IMapper mapper,IChannelService channelService, Subscriber tenant,
            IThreadService threadService, ITenantService tenantService )
        {
            _mapper = mapper;
            _channelService = channelService;
            _tenant = tenant;
            _tenantService = tenantService;
            _threadService = threadService;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                var model = new ForumStatistics
                {
                    Channels = _channelService.GetAllChannels(_tenant.Id).Count(),
                   // Categories = _channelService.GetAllCategories(_tenant.Id).Count(),
                    Threads = _threadService.GetAllThreads(_tenant.Id).Count(),
                    Users = _tenantService.GetAllTenantUsers(_tenant.Id).Count(),
                    PendingInvites = _tenantService.GetAllTenantInvites(_tenant.Id).Count()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                return View(new ForumStatistics());
            }
           
        }
    }
}
