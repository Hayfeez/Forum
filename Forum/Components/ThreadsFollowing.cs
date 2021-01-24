using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Forum.DataAccessLayer.IService;
using Forum.Helpers;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class ThreadsFollowing : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly IForumUserService _userService;

        public ThreadsFollowing(IMapper mapper, IForumUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
           

        public IViewComponentResult Invoke()
        {
            var following = new List<Thread>();
            try
            {
                var dt = _userService.GetUserThreadActions(User.Identity.GetSubscriberUserId()).Where(a => a.Followed);
                foreach (var item in dt)
                {
                    following.Add(item.Thread);
                }

                return View(_mapper.Map<List<ThreadVM>>(following));
            }
            catch (Exception ex)
            {
               
            }
            return View(following);
        }

    }
}
