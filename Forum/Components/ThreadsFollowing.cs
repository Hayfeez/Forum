using System;
using System.Collections.Generic;
using Forum.Helpers;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class ThreadsFollowing : ViewComponent
    {
        private long _userId;
        public ThreadsFollowing()
        {
            try
            {
                _userId = User.Identity.GetSubscriberUserId();
            }
            catch (Exception ex)
            {

            }
            
        }

        public IViewComponentResult Invoke()
        {
            var following = new List<ThreadVM>();
            try
            {

            }
            catch (Exception ex)
            {
               
            }
            return View(following);
        }

    }
}
