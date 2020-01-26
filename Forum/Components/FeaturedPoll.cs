using System;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class FeaturedPoll : ViewComponent
    {
        public FeaturedPoll()
        {
        }

        public IViewComponentResult Invoke()
        {

            return View();
        }
    }
}
