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
using Microsoft.AspNetCore.Identity;

namespace Forum.Controllers
{
    [AllowAnonymous]
    public class ThreadsController : Controller
    {
        private readonly ILogger<ThreadsController> _logger;
        private readonly IMapper _mapper;
        private readonly IThreadService _threadService;
        private readonly IChannelService _channelService;
        private readonly Subscriber _tenant;

        public ThreadsController(ILogger<ThreadsController> logger, Subscriber tenant, IMapper mapper,
                    IThreadService threadService, IChannelService channelService)
        {
            _logger = logger;
            _threadService = threadService;
            _channelService = channelService;
            _mapper = mapper;
            _tenant = tenant;

        }

        public IActionResult Index(string tag)
        {
            try
            {
                if (string.IsNullOrEmpty(tag)) tag = "lt";

                var allthreads = _threadService.GetAllThreads(_tenant.Id);
                IEnumerable<ThreadVM> data;

                if (tag == "fp")  //fp is latest for now
                    data = _mapper.Map<IEnumerable<ThreadVM>>(allthreads.OrderByDescending(a => a.DateCreated));

                else if (tag == "pp") //popular by no of replies
                    data = _mapper.Map<IEnumerable<ThreadVM>>(allthreads.OrderByDescending(a => a.ThreadReplies.Count()));

                else if (tag == "nr") //no replies
                    data = _mapper.Map<IEnumerable<ThreadVM>>(allthreads.Where(a => a.ThreadReplies.Count() == 0));
                else
                    data = _mapper.Map<IEnumerable<ThreadVM>>(allthreads.OrderByDescending(a => a.DateCreated));

                ViewBag.tag = tag;
                return View(data);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }            
        }

        public IActionResult Thread(string id)
        {
            try
            {

                var thread = _threadService.GetThreadById(id);
                var model = _mapper.Map<ThreadVM>(thread);
                if(model == null) return RedirectToAction("Index", "Home");

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting thread");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }            
        }
     
        public IActionResult Pinned(int id, [FromServices] IPinnedPostService _pinnedPostService)
        {
            try
            {
                var pinned = _pinnedPostService.GetPinnedPostById(id);
                var model =  _mapper.Map<ThreadVM>(pinned);

                return View("Pinned", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting pinned post");
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
                    var data = _mapper.Map<Thread>(model);
                    data.SubscriberUserId = User.Identity.GetSubscriberUserId(); ;
                    var response = await _threadService.CreateThread(data);

                    if(response == DbActionsResponse.DuplicateExist)
                    {
                        ModelState.AddModelError(string.Empty, "This title exists");
                        return View(model);
                    }
                    else if(response == DbActionsResponse.Success)
                    {
                        ModelState.AddModelError(string.Empty, "Topic saved successfully");
                        var category = _channelService.GetCategoryById(model.CategoryId);

                        //return RedirectToAction("CategoryThreads", "Channels", new { category = category?.Title });
                        return RedirectToAction("Thread", "Threads", new { id = data.Title });
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


       
        [HttpGet]
        public IActionResult LoadThreadInfo(long threadId, [FromServices] IThreadInfoService _threadInfoService)
        {
            try
            {
                var data = _threadInfoService.GetThreadInfo(threadId);
                return Json(new { Status = 1, Data = data });
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = "Error while loading thread info" });
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult LoadThreadUserInfo(long threadId, [FromServices] IForumUserService _userService)
        {
            try
            {
                var data = _userService.GetUserThreadInfo(User.Identity.GetSubscriberUserId(), threadId);

                return Json(new { Status = 1, Data = _mapper.Map<UserThreadInfoVM>(data) }) ;
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = "Error while loading thread info" });
            }
        }

        [HttpGet]
        public IActionResult LoadReplies(long threadId)
        {
            try
            {
                var eply = _threadService.GetAllRepliesToThread(threadId);
                var model = _mapper.Map<List<ThreadReplyVM>>(eply);

                return Json(new { Status = 1, Data = model });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting thread replies");
                return Json(new { Status = -1, Message = "Error while loading thread replies" });
            }
        }
       
        [HttpGet]
        public async Task<IActionResult> IncreaseThreadView(long threadId, [FromServices] IThreadInfoService _threadInfoService)
        {
            try
            {
                if (threadId != 0)
                {
                    var response = await _threadInfoService.IncreaseThreadView(threadId);
                    if (response == DbActionsResponse.Success)
                    {
                        return Json(new { Status = 1, Message = "Success" });
                    }
                }

                return Json(new { Status = -1, Message = "Failed" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = "An error occured" });
            }
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> ReplyThread([FromBody]SaveThreadReplyVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _mapper.Map<ThreadReply>(model);
                    data.SubscriberUserId = User.Identity.GetSubscriberUserId();

                    var response = await _threadService.CreateReply(data);
                    if (response == DbActionsResponse.Success)
                    {
                        return Json(new { Status = 1, Message = "Success" });
                    }
                }

                return Json(new { Status = -1, Message = "Failed" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = "An error occured" });
            }
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> ThreadAction([FromBody]SaveUserAction model, [FromServices] IForumUserService _userService)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _userService.SaveUserThreadInfo(model, User.Identity.GetSubscriberUserId());
                    if (response == DbActionsResponse.Success)
                    {
                        return Json(new { Status = 1, Message = "Success" });
                    }
                }

                return Json(new { Status = -1, Message = "Failed" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = "An error occured" });
            }
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> ReplyAction([FromBody]SaveUserAction model, [FromServices] IThreadInfoService _threadInfoService)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _threadInfoService.SaveThreadReplyInfo(model, User.Identity.GetSubscriberUserId());
                    if (response == DbActionsResponse.Success)
                    {
                        return Json(new { Status = 1, Message = "Success" });
                    }
                }

                return Json(new { Status = -1, Message = "Failed" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = "An error occured" });
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
