using System;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class CategoryCount : ViewComponent
    {
        public CategoryCount()
        {
        }

        public IViewComponentResult Invoke()
        {

            return View();
        }
    }
}
