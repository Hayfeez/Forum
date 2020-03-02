using System;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class ChannelCount : ViewComponent
    {
        public ChannelCount()
        {
        }

        public IViewComponentResult Invoke()
        {

            return View();
        }
    }
}
