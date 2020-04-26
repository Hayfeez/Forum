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
using Microsoft.AspNetCore.Identity;

namespace Forum.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IMapper _mapper;
        private readonly IThreadService _threadService;
        private readonly IChannelService _channelService;
        private readonly Subscriber _tenant;

        public ProfileController(ILogger<ProfileController> logger, Subscriber tenant, IMapper mapper,
                    IThreadService threadService, IChannelService channelService)
        {
            _logger = logger;
            _threadService = threadService;
            _channelService = channelService;
            _mapper = mapper;
            _tenant = tenant;

        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Thread(string id)
        {
            try
            {
                var thread = _threadService.GetThreadById(id);
                var model = _mapper.Map<ThreadVM>(thread);
                if(model == null) return RedirectToAction("Index", "Home");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting thread");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }            
        }

    }
}
