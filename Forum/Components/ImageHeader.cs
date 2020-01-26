using System;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class ImageHeader : ViewComponent
    {
        public ImageHeader()
        {
        }

        public IViewComponentResult Invoke()
        {

            return View();
        }
    }
}
