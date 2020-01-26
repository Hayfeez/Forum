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

namespace Forum.Controllers
{
    public class ThreadController : Controller
    {
        private readonly IThreadService _threadService;
        private readonly int _subscriberId = 1;

        public ThreadController(IThreadService threadService)
        {
            _threadService = threadService;
        }

        public IActionResult Index(long id)
        {
            try
            {
                var thread = _threadService.GetThreadById(id);
                var model = new ThreadVM
                {
                    Id = thread.Id,
                    AuthorId = thread.AppUser.Id,
                    AuthorImageUrl = thread.AppUser.ProfileImageUrl,
                    AuthorName = thread.AppUser.UserName,
                    AuthorRating = thread.AppUser.Rating,
                    Content = thread.Content,
                    DatePosted = thread.DateCreated,
                    Title = thread.Title,
                    Replies = thread.ThreadReplies.Select(n=> new ThreadReplyVM {
                        Id= n.Id,
                        AuthorId= n.AppUser.Id,
                        AuthorImageUrl= n.AppUser.ProfileImageUrl,
                        AuthorName = n.AppUser.UserName,
                        AuthorRating = n.AppUser.Rating,
                        Content = n.Content,
                        DatePosted = n.DateCreated,
                        ThreadId = n.Thread.Id
                    })
                };

                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
              

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
