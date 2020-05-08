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
    [Authorize]
    public class ManageController : Controller
    {
        private readonly ILogger<ManageController> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly Subscriber _tenant;

        public ManageController(ILogger<ManageController> logger, Subscriber tenant, IMapper mapper,
            IAccountService accountService)
        {
            _logger = logger;
            _mapper = mapper;
            _accountService = accountService;
            _tenant = tenant;
        }

        #region Users

        public IActionResult Users()
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
                    var checkEmail = _accountService.IsUserASubscriberUser(model.Email, _tenant.Id);
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
                        SubscriberId = _tenant.Id,
                    }, true);

                    if (createInvite == DbActionsResponse.DuplicateExist)
                    {
                        //user has an invite, resend mail
                        var existingInvite = _accountService.GetSubscriberInvite(model.Email, _tenant.Id);
                        var callbackUrl = Url.Action("Register", "Account", new { inviteCode = existingInvite.InviteCode, email = model.Email }, Request.Scheme);
                        _mailHelper.SendMail(
                           model.Email,
                           $"You have been invited to join the {_tenant.Name} forum. Please accept this invite by <a href='{callbackUrl}'>clicking here</a>.",
                           $"{_tenant.Name} Forum Invitation");

                        //show success message
                        return View(model);
                    }
                    else if (createInvite == DbActionsResponse.Success)
                    {
                        var callbackUrl = Url.Action("Register", "Account", new { inviteCode = code, email = model.Email }, Request.Scheme);
                        _mailHelper.SendMail(
                           model.Email,
                           $"You have been invited to join the {_tenant.Name} forum. Please accept this invite by <a href='{callbackUrl}'>clicking here</a>.",
                           $"{_tenant.Name} Forum Invitation");


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

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRole model, [FromServices] ITenantService _tenantService)
        {
            try
            {
                var response = await _tenantService.UpdateUserRole(_tenant.Id, model.AppUserId, model.Role);
                if (response == DbActionsResponse.NotFound)
                    return Json(new { Status = -1, Message = "User not found" });

                if (response == DbActionsResponse.Success)
                    return Json(new { Status = 1, Message = "User role updated successfully" });

                return Json(new { Status = -1, Message = "Role Update failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error occured while updating user role");
                return BadRequest("Could not update user role");
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string appUserId, [FromServices] ITenantService _tenantService)
        {
            try
            {
                var response = await _tenantService.DeleteTenantUser(_tenant.Id, appUserId);
                if (response == DbActionsResponse.NotFound)
                    return Json(new { Status = -1, Message = "User not found" });

                if (response == DbActionsResponse.Success)
                    return Json(new { Status = 1, Message = "User deleted successfully" });

                return Json(new { Status = -1, Message = "Deleting user failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error occured while deleting user");
                return BadRequest("Could not delete user");
            }

        }

        [HttpPost]
        public async Task<IActionResult> CancelInvite(long Id, [FromServices] ITenantService _tenantService)
        {
            try
            {
                var response = await _tenantService.CancelInvite(_tenant.Id, Id);
                if (response == DbActionsResponse.NotFound)
                    return Json(new { Status = -1, Message = "User not found" });

                if (response == DbActionsResponse.Success)
                    return Json(new { Status = 1, Message = "Invite deleted successfully" });

                return Json(new { Status = -1, Message = "Deleting invite failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error occured while deleting invite");
                return BadRequest("Could not delete invite");
            }

        }

        #endregion

        #region Tenant
        public IActionResult Settings([FromServices] IPinnedPostService _pinnedPostService, [FromServices] IChannelService _channelService)
        {
            try
            {
                var model = new TenantSettings
                {
                    PinnedPosts = _pinnedPostService.GetPinnedPosts(_tenant.Id),
                    Subscriber = _tenant,
                    Categories = _mapper.Map<List<CategoryVM>>(_channelService.GetAllCategories(_tenant.Id)),
                    Channels = _mapper.Map<List<ChannelVM>>(_channelService.GetAllChannels(_tenant.Id))
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while loading tenant info");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSettings(UpdateSubscriberSetting model, [FromServices] ITenantService _tenantService)
        {
            try
            {
                var response = await _tenantService.UpdateSubscriber(_tenant.Id, model);
                if (response == DbActionsResponse.DuplicateExist)
                    return Json(new { Status = -1, Message = "Name or domain name is already taken" });

                if (response == DbActionsResponse.Success)
                    return Json(new { Status = 1, Message = "Settings updated successfully" });

                return Json(new { Status = -1, Message = "Update failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error occured while updating forum settings");
                return BadRequest("Could not update settings");
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount([FromServices] ITenantService _tenantService)
        {
            try
            {
                var response = await _tenantService.DeleteSubscriber(_tenant.Domain);
                if (response == DbActionsResponse.NotFound)
                    return Json(new { Status = -1, Message = "Tenant not found" });

                if (response == DbActionsResponse.Success)
                    return Json(new { Status = 1, Message = "Forum deleted successfully" });

                return Json(new { Status = -1, Message = "Forum could not be deleted" });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error occured while deleting forum");
                return BadRequest("Could not delete delete forum");
            }

        }

        #endregion


        #region Moderators


        #endregion

    }
}
