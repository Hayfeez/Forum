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
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Forum.Controllers
{
    //[Authorize]
    public class ManageController : Controller
    {
        private readonly ILogger<ManageController> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly ISubscriberService _subscriberService;
        private readonly int _subscriberId;

        public ManageController(ILogger<ManageController> logger, IMapper mapper,
            IAccountService accountService, ISubscriberService subscriberService)
        {
            _logger = logger;
            _mapper = mapper;
            _accountService = accountService;
            _subscriberService = subscriberService;
            _subscriberId = _subscriberService.GetSubscriberId();
        }

        #region Invites

        public IActionResult ManageInvites()
        {
            try
            {
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while loading manage invites");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
        }

        public IActionResult InviteUser()
        {
            try
            {

                return View(new JoinVM());
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while loading manage invites");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
        }

        [HttpPost]
        public async Task<IActionResult> InviteUser(JoinVM model, [FromServices] MailHelper _mailHelper)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkEmail = _accountService.IsUserASubscriberUser(model.Email, _subscriberId);
                    if (checkEmail)
                    {
                        ModelState.AddModelError(string.Empty, "This user is already a member");
                        return View(model);
                    }
                    var code = Guid.NewGuid().ToString();
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var createInvite = await _accountService.CreateSubscriberInvite(new SubscriberInvite
                    {
                        DateCreated = DateTime.Now,
                        Email = model.Email,
                        InviteCode = code,
                        SubscriberId = _subscriberId,
                    }, true);

                    if (createInvite == DbActionsResponse.DuplicateExist)
                    {
                        //user has an invite
                        ModelState.AddModelError(string.Empty, "There is a pending invite.");
                        return View(model);
                    }
                    else if (createInvite == DbActionsResponse.Success)
                    {
                        _logger.LogInformation("User Invite saved.");
                        var callbackUrl = Url.Action("Register", "Account", new { inviteCode = code, email = model.Email }, Request.Scheme);

                        _mailHelper.SendMail(
                           model.Email,
                           $"You have been invited to join the forum. Please accept this invite by <a href='{callbackUrl}'>clicking here</a>.",
                           "Forum Invitation");

                       
                        //add success message
                        return View(new JoinVM());
                    }
                }

                // If we got this far, something failed, redisplay form
                ModelState.AddModelError(string.Empty, "Inviting user failed");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while saving forum invite");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }

        #endregion

        #region Subscriber


        #endregion


        #region Moderators


        #endregion

        #region Settings

        #endregion


    }
}
