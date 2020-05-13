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
        private long _userId;
        private readonly IMapper _mapper;
        private readonly IForumUserService _userService;
        private readonly Subscriber _tenant;

        public ThreadsFollowing(IMapper mapper, IForumUserService userService, Subscriber tenant)
        {
            try
            {
                _mapper = mapper;
                _userService = userService;
                _tenant = tenant;
                _userId = User.Identity.GetSubscriberUserId();
            }
            catch (Exception ex)
            {

            }
           
        }
       

        public IViewComponentResult Invoke()
        {
            var following = new List<Thread>();
            try
            {
                var dt = _userService.GetUserThreadActions(_userId).Where(a => a.Followed);
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
