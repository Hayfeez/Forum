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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Forum.Controllers
{
    [AllowAnonymous]
    public class ThreadsController : Controller
    {
        private readonly ILogger<ThreadsController> _logger;
        private readonly IMapper _mapper;
        private readonly IThreadService _threadService;
        private readonly IChannelService _channelService;
        private readonly ISubscriberService _subscriberService;
        private readonly int _subscriberId;



        public ThreadsController(ILogger<ThreadsController> logger, IMapper mapper,
                    IThreadService threadService, ISubscriberService subscriberService, IChannelService channelService)
        {
            _logger = logger;
            _threadService = threadService;
            _channelService = channelService;
            _mapper = mapper;
            _subscriberService = subscriberService;
            _subscriberId = _subscriberService.GetSubscriberId();

        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Thread(long id)
        {
            try
            {
                var thread = _threadService.GetThreadById(id);
                var model = _mapper.Map<ThreadVM>(thread);
                
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting thread");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

                // _logger.LogError(0, ex, "Error while processing request from {Address}", address);
            }            
        }

        public IActionResult Guideline()
        {
            try
            {
                var thread = _threadService.GetGuideline() ;
                var model =  _mapper.Map<ThreadVM>(thread);

                return View("Thread", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting guideline");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

                // _logger.LogError(0, ex, "Error while processing request from {Address}", address);
            }
        }

        [Authorize]
        public IActionResult CreateThread(int channelId, int categoryId)
        {
            try
            {
                var model = new SaveThreadVM {
                    ChannelId = channelId,
                    CategoryId = categoryId
                    
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while loading thread page");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateThread(SaveThreadVM model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    //model.UserId = _userManager.GetUserId; "1";
                    long subscriberUserId = User.Identity.GetSubscriberUserId();
                    var response =  await _threadService.CreateThread(new Thread
                    {
                       Content = model.Content,
                       Title = model.Title,
                       Tags = model.Tags,
                       DateCreated = DateTime.Now,
                       CategoryId = model.CategoryId,
                       SubscriberUserId = subscriberUserId                      
                       
                       
                    });

                    if(response == DbActionsResponse.DuplicateExist)
                    {
                        ModelState.AddModelError(string.Empty, "This title exists");
                        return View(model);
                    }
                    else if(response == DbActionsResponse.Success)
                    {
                        ModelState.AddModelError(string.Empty, "Topic saved successfully");
                        var category = _channelService.GetCategoryById(model.CategoryId);

                        return RedirectToAction("CategoryThreads", "Channels", new { category = category?.Title });
                    }

                    //Failed
                    ModelState.AddModelError(string.Empty, "Topic not saved");
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "Required fileds are empty");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while creating thread");
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
