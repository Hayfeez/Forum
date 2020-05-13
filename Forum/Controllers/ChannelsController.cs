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
using Microsoft.AspNetCore.Authorization;

namespace Forum.Controllers
{
    [AllowAnonymous]
    public class ChannelsController : Controller
    {
        private readonly ILogger<ChannelsController> _logger;
        private readonly IMapper _mapper;
        private readonly IChannelService _channelService;
        private readonly IThreadService _threadService;
        private readonly Subscriber _tenant;

        public ChannelsController(ILogger<ChannelsController> logger, Subscriber tenant, IMapper mapper, IChannelService channelService,
            IThreadService threadService)
        {
             _logger = logger;
            _mapper = mapper;
            _channelService = channelService;
            _threadService = threadService;
            _tenant = tenant;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [Route("LoadChannelCategories/{channelId}")]
        public IActionResult Category(int channelId)
        {
            try
            {
                if (channelId == 0)
                    return Ok(new
                    {
                        Message = "Select a Channel",
                        Status = -1
                    });

                var chanel = _channelService.GetAllCategoriesInChannel(channelId, false);

                return Ok(new
                {
                    Data = chanel.ToList(),
                    Message = "",
                    Status = 1
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting categories");
                return BadRequest(ex.Message);
            }
        }


        public IActionResult ChannelThreads(string channel)
        {
            try
            {
                var chanel = _channelService.GetChannelByName(channel);
                if (chanel != null)
                {
                    var threads = _threadService.GetAllThreadsInChannelOrCategory(chanel.Id, null);
                    var threadList = new ChannelThreads
                    {
                        Channel = _mapper.Map<ChannelVM>(chanel),
                        Threads = _mapper.Map<IEnumerable<ThreadVM>>(threads)
                    };

                    return View(threadList);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting threads");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult CategoryThreads(string category)
        {
            try
            {
                var catego = _channelService.GetCategoryByName(category);
                if (catego != null)
                {
                    //var threads = _threadService.GetAllThreadsInChannelOrCategory(catego.ChannelId, catego.Id);
                    var threadList = new CategoryThreads
                    {
                        Category = _mapper.Map<CategoryVM>(catego),
                        Threads = _mapper.Map<IEnumerable<ThreadVM>>(catego.Threads)
                    };

                    return View(threadList);
                }
                return RedirectToAction("Index", "Home");
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
