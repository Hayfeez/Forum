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

namespace Forum.Controllers
{
    public class ThreadsController : Controller
    {
        private readonly ILogger<ThreadsController> _logger;
        private readonly IMapper _mapper;
        private readonly IThreadService _threadService;
        private readonly int _subscriberId = BaseClass.GetSubscriberId();

        public ThreadsController(ILogger<ThreadsController> logger, IMapper mapper, IThreadService threadService)
        {
            _logger = logger;
            _threadService = threadService;
            _mapper = mapper;
        }

        public IActionResult Index(long id)
        {
            try
            {
                var thread = _threadService.GetThreadById(id);
                var model = _mapper.Map<ThreadVM>(thread);
                
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while getting threads");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

                // _logger.LogError(0, ex, "Error while processing request from {Address}", address);
            }            
        }
              

       
    }
}
