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
using AutoMapper;
using Forum.Helpers;

namespace Forum.Controllers
{
    public class ChannelsController : Controller
    {
        private readonly ILogger<ChannelsController> _logger;
        private readonly IMapper _mapper;
        private readonly IChannelService _channelService;
        private readonly IThreadService _threadService;
        private readonly int _subscriberId = BaseClass.GetSubscriberId();

        public ChannelsController(ILogger<ChannelsController> logger, IMapper mapper, IChannelService channelService, IThreadService threadService)
        {
             _logger = logger;
            _mapper = mapper;
            _channelService = channelService;
            _threadService = threadService;
        }

        public IActionResult Index()
        {
            try
            {
                var channels = _channelService.GetAllChannels(_subscriberId);
                var d = _mapper.Map<IEnumerable<ChannelVM>>(channels);
                return View(new ChannelList { Channels = d });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting threads");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult ChannelThreads(int channelId)
        {
            try
            {
                var channel = _channelService.GetChannelById(channelId);
                var threads = _threadService.GetAllThreadsInChannelOrCategory(channelId, null);
                var threadList = new ChannelThreads
                {
                    Channel = _mapper.Map<ChannelVM>(channel),
                    Threads = _mapper.Map<IEnumerable<ThreadVM>>(threads)
                };                

                return View(threadList);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting threads");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult CategoryThreads(int categoryId)
        {
            try
            {
                var category = _channelService.GetCategoryById(categoryId);
                var threads = _threadService.GetAllThreadsInChannelOrCategory(categoryId, null);
                var threadList = new CategoryThreads
                {
                    Category = _mapper.Map<CategoryVM>(category),
                    Threads = _mapper.Map<IEnumerable<ThreadVM>>(threads)
                };

                return View(threadList);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting threads");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }


        #region private methods

        public void AddToViewBag(string type, string message)
        {
            TempData["Type"] = type;
            TempData["Message"] = message;
        }

        public string GetViewBagMessage(string key)
        {
            if (key.ToLower() == "type" && TempData["Type"] != null)
            {
                return TempData["Type"].ToString();

            }

            else if (key.ToLower() == "message" && TempData["Message"] != null)
            {
                return TempData["Message"].ToString();

            }

            return null;
        }

        #endregion
    }
}
