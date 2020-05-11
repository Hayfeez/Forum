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
using Microsoft.AspNetCore.Authorization;

namespace Forum.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IThreadService _threadService;
        private readonly Subscriber _tenant;

        public HomeController(ILogger<HomeController> logger, Subscriber tenant, IMapper mapper,
            IThreadService threadService)
        {
            _logger = logger;
            _mapper = mapper;
            _threadService = threadService;
            _tenant = tenant;
        }

       
        public IActionResult Index([FromServices]IPinnedPostService _pinnedPostService)
        {
            try
            {
                var pinnedPosts = _pinnedPostService.GetPinnedPosts(_tenant.Id);
                var threads = _threadService.GetLatestThreads(_tenant.Id, 10);
                var model = new HomeViewModel
                {
                    PinnedPosts = _mapper.Map<List<PinnedPost>>(pinnedPosts),
                    Threads = _mapper.Map<List<ThreadVM>>(threads)
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting threads");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
            
        }


        [HttpGet]
        public IActionResult Search(string searchText, [FromServices]ISearchService _searchService)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    var response = _searchService.SearchAllThreads(_tenant.Id, searchText);

                    return View(new SearchViewModel
                    {
                        Threads = _mapper.Map<List<ThreadVM>>(response),
                        SearchQuery = searchText,
                        ResultCount = response.Count()
                    });
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while searching threads");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult About()
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
