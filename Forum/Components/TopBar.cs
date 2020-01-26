using System;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class TopBar : ViewComponent
    {
        public TopBar()
        {
        }

        public IViewComponentResult Invoke()
        {

            return View();
        }
    }
}
