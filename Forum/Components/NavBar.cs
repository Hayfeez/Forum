using System;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class NavBar : ViewComponent
    {
        public NavBar()
        {
        }

        public IViewComponentResult Invoke()
        {

            return View();
        }
    }
}
