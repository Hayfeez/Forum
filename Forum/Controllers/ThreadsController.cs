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
using static Forum.Helpers.BaseClass;
using Microsoft.AspNetCore.Authorization;

namespace Forum.Controllers
{
    public class ThreadsController : Controller
    {
        private readonly ILogger<ThreadsController> _logger;
        private readonly IMapper _mapper;
        private readonly IThreadService _threadService;
        private readonly ISubscriberService _subscriberService;
        private readonly int _subscriberId;

        public ThreadsController(ILogger<ThreadsController> logger, IMapper mapper,
                    IThreadService threadService, ISubscriberService subscriberService)
        {
            _logger = logger;
            _threadService = threadService;
            _mapper = mapper;
            _subscriberService = subscriberService;
            _subscriberId = _subscriberService.GetSubscriberId();

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
                _logger.LogError(0, ex, "Error while getting threads");
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
                _logger.LogError(0, ex, "Error while getting threads");
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
                    model.UserId = "1";

                    var user = new SubscriberUser();
                    var category = new Category();

                   var response =  await _threadService.CreateThread(new Thread
                    {
                       Content = model.Content,
                       Title = model.Title,
                       Tags = model.Tags,
                       DateCreated = DateTime.Now,
                       SubscriberUser = user,
                       Category = category
                    });

                    if(response == DbActionsResponse.DuplicateExist)
                    {

                    }
                    else if(response == DbActionsResponse.Success)
                    {

                    }
                    //Failed
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "Required fileds are empty");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while loading thread page");
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
