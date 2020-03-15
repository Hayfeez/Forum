using System;
using System.Collections.Generic;
using AutoMapper;
using Forum.DataAccessLayer.IService;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class ChannelCount : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly IChannelService _channelService;
        private readonly ISubscriberService _subscriberService;

        private readonly int _subscriberId;
        public ChannelCount(IMapper mapper,IChannelService channelService, ISubscriberService subscriberService)
        {
            _mapper = mapper;
            _channelService = channelService;
            _subscriberService = subscriberService;
            _subscriberId = _subscriberService.GetSubscriberId();
        }

        public IViewComponentResult Invoke()
        {
            var model = new List<ChannelThreadCount>();
            try
            {
                var allchannels = _channelService.GetAllChannels(_subscriberId);
                model =  _mapper.Map<List<ChannelThreadCount>>(allchannels);
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
    }
}
