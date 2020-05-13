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
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IMapper _mapper;
        private readonly IThreadInfoService _threadInfoService;
        private readonly IForumUserService _userService;
        private readonly long _UserId;

        public ProfileController(ILogger<ProfileController> logger, IMapper mapper,
                    IThreadInfoService threadInfoService, IForumUserService userService)
        {
            _logger = logger;
            _threadInfoService = threadInfoService;
            _userService = userService;
            _mapper = mapper;
            _UserId = User.Identity.GetSubscriberUserId();
        }

        public IActionResult Index()  //logged in user profile
        {
            //no of threads, no of replies, no of followers, no of following
            try
            {
                var profile = _userService.GetUserProfile(_UserId);
                var model = _mapper.Map<ProfileVM>(profile);

                //add following and followers
                //  _userService.GetUserFollowings(_UserId);
               // _userService.GetUserFollowers(_UserId);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting user's prifile");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult UserProfile(long userId)
        {
            try
            {
                var profile = _userService.GetUserProfile(userId);
                var model = _mapper.Map<ProfileVM>(profile);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting user's prifile");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }


        public IActionResult MyThread([FromServices] IThreadService _threadService)
        {
            try
            {
                var thread = _threadService.GetAllThreadsByUser(_UserId);
                var model = _mapper.Map<List<ThreadVM>>(thread);
                
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting  user threads");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }            
        }

        public IActionResult Threads(UserAction action)
        {
            try
            {
                var threads = _userService.GetUserThreadActions(_UserId);
                switch (action)
                {
                    case UserAction.Bookmark:
                        threads = threads.Where(a => a.Bookmarked);
                        break;
                    case UserAction.Follow:
                        threads = threads.Where(a => a.Followed);
                        break;
                    case UserAction.Flag:
                        threads = threads.Where(a => a.Flagged);
                        break;
                    case UserAction.Like:
                        threads = threads.Where(a => a.Liked);
                        break;
                    default:
                        break;
                }

                var model = _mapper.Map<List<ThreadVM>>(threads);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting user's threads");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }


        [HttpPost]
        public async Task<IActionResult> FollowUser(long personId)
        {
            try
            {
                if (personId != 0)
                {                    
                    var response = await _userService.FollowUser(_UserId, personId);
                    if (response == DbActionsResponse.Success)
                    {
                        return Json(new { Status = 1, Message = "User is now being followed" });
                    }
                }

                return Json(new { Status = -1, Message = "Following User failed" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = "An error occured while Following User" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> UnFollowUser(long personId)
        {
            try
            {
                if (personId != 0)
                {
                    var response = await _userService.UnfollowUser(_UserId, personId);
                    if (response == DbActionsResponse.Success)
                    {
                        return Json(new { Status = 1, Message = "User has being unfollowed" });
                    }
                }

                return Json(new { Status = -1, Message = "Unfollowing user failed" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = "An error occured while unfollowing user" });
            }
        }

    }
}
