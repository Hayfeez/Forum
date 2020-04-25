using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Forum.Models;
using Forum.ViewModels;
using Forum.DataAccessLayer.IService;
using AutoMapper;
using Forum.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Forum.Controllers
{
    [AllowAnonymous]
    public class SearchController : Controller
    {
        private readonly ILogger<ThreadsController> _logger;
        private readonly IMapper _mapper;
        private readonly ISearchService _searchService;
        private readonly Subscriber _tenant;        

        public SearchController(ILogger<ThreadsController> logger, Subscriber tenant, IMapper mapper,
                    ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
            _mapper = mapper;
            _tenant = tenant;
        }

        [HttpPost]
        public IActionResult Search(string searchText)
        {
            return RedirectToAction("SearchResult", new { searchText = searchText });
        }


        public IActionResult SearchResult(string searchText)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _searchService.SearchAllThreads(_tenant.Id, searchText);
                    
                    return View(new SearchViewModel {
                        Threads = _mapper.Map<List<ThreadVM>>(response),
                        SearchQuery = searchText,
                        ResultCount = response.Count()
                    });
                }

                return RedirectToAction("Home", "Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while searching threads");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
        }

         
                  
    }
}
