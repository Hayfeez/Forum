using System;
using System.Collections.Generic;
using AutoMapper;
using Forum.DataAccessLayer.IService;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class ChannelCount : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly IChannelService _channelService;
        private readonly Subscriber _tenant;

        public ChannelCount(IMapper mapper,IChannelService channelService, Subscriber tenant)
        {
            _mapper = mapper;
            _channelService = channelService;
            _tenant = tenant;
        }

        public IViewComponentResult Invoke()
        {
            var model = new List<ChannelThreadCount>();
            try
            {
                var allchannels = _channelService.GetAllChannels(_tenant.Id);
                model =  _mapper.Map<List<ChannelThreadCount>>(allchannels);
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
    }
}
