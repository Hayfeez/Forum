using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Forum.DataAccessLayer.IService;
using Forum.Models;
using Forum.ViewModels;
using Newtonsoft.Json;

namespace Forum.Helpers
{
    public class LoadDynamicContent
    {
        private readonly IChannelService _channelService;
        private readonly Subscriber _tenant;
        private readonly IMapper _mapper;

        public LoadDynamicContent(IChannelService channelService, IMapper mapper, Subscriber tenant)
        {
            _channelService = channelService;
            _mapper = mapper;
            _tenant = tenant;
        }

        public List<ChannelVM> GetChannels()
        {
            try
            {
                var channels = _channelService.GetAllChannels(_tenant.Id);

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
                var categories = _channelService.GetAllCategories(_tenant.Id);

                return _mapper.Map<List<CategoryVM>>(categories);
            }

            catch (Exception ex)
            {
                return new List<CategoryVM>();
            }
            
        }

      
    }
}
