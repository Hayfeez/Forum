using System;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class ThreadsFollowing : ViewComponent
    {
        public ThreadsFollowing()
        {
        }

        public IViewComponentResult Invoke()
        {
            
            return View();
        }

    }
}
