using System;
using System.Collections.Generic;
using AutoMapper;
using Forum.DataAccessLayer.IService;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class TopBar : ViewComponent
    {
        private readonly IChannelService _channelService;
        private readonly IMapper _mapper;
        private readonly ISubscriberService _subscriberService;

        public TopBar(IChannelService channelService, IMapper mapper, ISubscriberService subscriberService)
        {
            _channelService = channelService;
            _mapper = mapper;
            _subscriberService = subscriberService;
        }

       
        public IViewComponentResult Invoke()
        {
            List<ChannelVM> model = new List<ChannelVM>();
            try
            {
                var categories = _channelService.GetAllChannels(_subscriberService.GetSubscriberId());

                model = _mapper.Map<List<ChannelVM>>(categories);
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);
            }
           
        }

       
    }
}
