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
    public class DefaultController : Controller
    {
        private readonly ILogger<DefaultController> _logger;
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService;

        public DefaultController(ILogger<DefaultController> logger, IMapper mapper, ITenantService tenantService)
        {
            _logger = logger;
            _mapper = mapper;
            _tenantService = tenantService;
        }

       
        public IActionResult Index()
        {
            try
            {
                var subdomain = HttpContext.Request.Host.Host.ToLower();              
                if (_tenantService.DoesTenantExist(subdomain)) return RedirectToAction("Index", "Home");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error occured while loading default page");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            
        }

        public IActionResult Subscribers()
        {
            try
            {
                var model = _tenantService.GetAllSubscribers();
                return Json(new { Status = 1, Data = model } );
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error occured while getting subscribers");
                return BadRequest("Could not load subscribers");
            }

        }

        public IActionResult SearchSubscribers(string searchQuery)
        {
            try
            {
                var model = _tenantService.SearchSubscriber(searchQuery);
                return Json(new { Status = 1, Data = model });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error occured while searching subscribers");
                return BadRequest("Could not load subscribers");
            }

        }


        public async Task<IActionResult> AddSubscriber(SaveSubscriberInfo model)
        {
            try
            {
                var data = _mapper.Map<Subscriber>(model);
                var response = await _tenantService.AddSubscriber(data);
                if(response == DbActionsResponse.DuplicateExist)
                return Json(new { Status = -1, Message = "Name or domain name is already taken" });

                if (response == DbActionsResponse.Success)
                    return Json(new { Status = 1, Message = "Forum created successfully" });

                    return Json(new { Status = -1, Message = "Forum Account creation failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error occured while creating new subscriber");
                return BadRequest("Could not create new forum account");
            }

        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
