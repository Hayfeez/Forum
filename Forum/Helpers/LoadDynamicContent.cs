using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Forum.DataAccessLayer.IService;
using Forum.ViewModels;
using Newtonsoft.Json;

namespace Forum.Helpers
{
    public class LoadDynamicContent
    {
        private readonly IChannelService _channelService;
        private readonly ISubscriberService _subscriberService;
        private readonly IMapper _mapper;
        private readonly int userSubscriberId;

        public LoadDynamicContent(IChannelService channelService, IMapper mapper, ISubscriberService subscriberService)
        {
            _channelService = channelService;
            _mapper = mapper;
            _subscriberService = subscriberService;
            userSubscriberId = _subscriberService.GetSubscriberId();
        }

        public List<ChannelVM> GetChannels()
        {
            try
            {
                var channels = _channelService.GetAllChannels(userSubscriberId);

                return _mapper.Map<List<ChannelVM>>(channels);
            }

            catch (Exception ex)
            {
                return new List<ChannelVM>();
            }

        }

        public List<CategoryVM> GetCategories()
        {
            try
            {
                var categories = _channelService.GetAllCategories(userSubscriberId);

                return _mapper.Map<List<CategoryVM>>(categories);
            }

            catch (Exception ex)
            {
                return new List<CategoryVM>();
            }
            
        }

      
    }
}
