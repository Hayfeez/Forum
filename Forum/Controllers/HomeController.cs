using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Forum.Models;
using Forum.ViewModels;
using AutoMapper;
using Forum.Helpers;
using Forum.DataAccessLayer.IService;

namespace Forum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IThreadService _threadService;
        private readonly ISubscriberService _subscriberService;
        private readonly int _subscriberId;

        public HomeController(ILogger<HomeController> logger, IMapper mapper,
            IThreadService threadService, ISubscriberService subscriberService)
        {
            _logger = logger;
            _mapper = mapper;
            _threadService = threadService;
            _subscriberService = subscriberService;
            _subscriberId = _subscriberService.GetSubscriberId();
        }

        public IActionResult Index()
        {
            var guideline = _threadService.GetGuideline();           
            var model = new HomeViewModel
            {
                Guideline = _mapper.Map<ThreadVM>(guideline),
                Threads = new List<ThreadVM>()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
