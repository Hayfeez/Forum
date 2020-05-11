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

        [HttpPost]
        public IActionResult ResendInviteMail(string email, [FromServices] ITenantService _tenantService, [FromServices]MailHelper _mailHelper)
        {
            try
            {
                var getInvite = _accountService.GetSubscriberInvite(email, _tenant.Id);
                if (getInvite != null)
                {
                    var callbackUrl = Url.Action("Register", "Account", new { inviteCode = getInvite.InviteCode, email = getInvite.Email }, Request.Scheme);
                    _mailHelper.SendMail(
                       getInvite.Email,
                       $"You have been invited to join the {_tenant.Name} forum. Please accept this invite by <a href='{callbackUrl}'>clicking here</a>.",
                       $"{_tenant.Name} Forum Invitation");

                    return Json(new { Status = 1, Message = "Invite deleted successfully" });
                }

                return Json(new { Status = -1, Message = "Resending invite mail failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error occured while resending invite");
                return BadRequest("Could not resend invite");
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

        [HttpGet]
        public IActionResult GetSubscriberInfo()
        {
            try
            {
               return Json(new { Status = 1, Data = _tenant, Message = "" });
            }
            catch (Exception ex)
            {
                return BadRequest("Could not load info");
            }

        }


        [HttpPost]
        public async Task<IActionResult> UpdateSubscriberInfo(SaveSubscriberInfo model, [FromServices] ITenantService _tenantService)
        {
            try
            {
                var response = await _tenantService.UpdateSubscriber(_tenant.Id, model);
                if (response == DbActionsResponse.DuplicateExist)
                    return Json(new { Status = -1, Message = "Name or domain name is already taken" });

                if (response == DbActionsResponse.Success)
                    return Json(new { Status = 1, Message = "Subscription Info updated successfully" });

                return Json(new { Status = -1, Message = "Update failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error occured while updating forum info");
                return BadRequest("Could not update subscriber info");
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubscriberAccount([FromServices] ITenantService _tenantService)
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


        #region Channel
        public async Task<IActionResult> AddChannel(SaveChannelVM model, [FromServices] IChannelService _channelService)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _mapper.Map<Channel>(model);
                    data.SubscriberId = _tenant.Id;
                    var response = await _channelService.CreateChannel(data);

                    if (response == DbActionsResponse.DuplicateExist)
                        return Json(new { Status = -1, Message = "This channel exists" });

                    else if (response == DbActionsResponse.Success)
                        return Json(new { Status = 1, Message = "Channel saved successfully" });

                    return Json(new { Status = -1, Message = "Channel not saved" });
                }

                return Json(new { Status = -1, Message = "Required fileds are empty" });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while creating channel");
                return BadRequest("Could not create channel");
            }
        }

        public async Task<IActionResult> UpdateChannel(SaveChannelVM model, [FromServices] IChannelService _channelService)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _mapper.Map<Channel>(model);
                    data.SubscriberId = _tenant.Id;
                    var response = await _channelService.UpdateChannel(data);

                    if (response == DbActionsResponse.NotFound)
                        return Json(new { Status = -1, Message = "This channel cannot be found" });

                    else if (response == DbActionsResponse.DuplicateExist)
                        return Json(new { Status = -1, Message = "This channel exists" });

                    else if (response == DbActionsResponse.Success)
                        return Json(new { Status = 1, Message = "Channel updated successfully" });

                    return Json(new { Status = -1, Message = "Channel not updated" });
                }

                return Json(new { Status = -1, Message = "Required fileds are empty" });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while updating channel");
                return BadRequest("Could not update channel");
            }
        }

        public async Task<IActionResult> DeleteChannel(int channelId, [FromServices] IChannelService _channelService)
        {
            try
            {

                var response = await _channelService.DeleteChannel(channelId, _tenant.Id);

                if (response == DbActionsResponse.NotFound)
                    return Json(new { Status = -1, Message = "This channel cannot be found" });

                else if (response == DbActionsResponse.DeleteDenied)
                    return Json(new { Status = -1, Message = "This channel cannot be deleted" });

                else if (response == DbActionsResponse.Success)
                    return Json(new { Status = 1, Message = "Channel deleted successfully" });

                return Json(new { Status = -1, Message = "Channel not deleted" });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while deleting channel");
                return BadRequest("Could not delete channel");
            }
        }

        public IActionResult GetChannel(int channelId, [FromServices] IChannelService _channelService)
        {
            try
            {

                var response = _channelService.GetChannelById(channelId);

                if (response == null)
                    return Json(new { Status = -1, Message = "This channel cannot be found" });

                else if (response.SubscriberId != _tenant.Id)
                    return Json(new { Status = -1, Message = "This channel does not exist" });

                else
                    return Json(new { Status = -1, Message = "", Data = response });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while retrieving channel");
                return BadRequest("Could not retrieve channel");
            }
        }

        #endregion

        #region Categories
        public async Task<IActionResult> AddCategory(SaveCategoryVM model, [FromServices] IChannelService _channelService)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _mapper.Map<Category>(model);
                    var response = await _channelService.CreateCategory(data);

                    if (response == DbActionsResponse.DuplicateExist)
                        return Json(new { Status = -1, Message = "This category exists" });

                    else if (response == DbActionsResponse.Success)
                        return Json(new { Status = 1, Message = "Category saved successfully" });

                    return Json(new { Status = -1, Message = "Category not saved" });
                }

                return Json(new { Status = -1, Message = "Required fileds are empty" });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while creating category");
                return BadRequest("Could not create category");
            }
        }

        public async Task<IActionResult> UpdateCategory(SaveCategoryVM model, [FromServices] IChannelService _channelService)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _mapper.Map<Category>(model);
                    var response = await _channelService.UpdateCategory(data);

                    if (response == DbActionsResponse.NotFound)
                        return Json(new { Status = -1, Message = "This category cannot be found" });

                    else if (response == DbActionsResponse.DuplicateExist)
                        return Json(new { Status = -1, Message = "This category exists" });

                    else if (response == DbActionsResponse.Success)
                        return Json(new { Status = 1, Message = "Category updated successfully" });

                    return Json(new { Status = -1, Message = "Category not updated" });
                }

                return Json(new { Status = -1, Message = "Required fileds are empty" });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while updating category");
                return BadRequest("Could not update category");
            }
        }

        public async Task<IActionResult> DeleteCategory(int categoryId, [FromServices] IChannelService _channelService)
        {
            try
            {

                var response = await _channelService.DeleteCategory(categoryId, _tenant.Id);

                if (response == DbActionsResponse.NotFound)
                    return Json(new { Status = -1, Message = "This category cannot be found" });

                else if (response == DbActionsResponse.DeleteDenied)
                    return Json(new { Status = -1, Message = "This category cannot be deleted" });

                else if (response == DbActionsResponse.Success)
                    return Json(new { Status = 1, Message = "Category deleted successfully" });

                return Json(new { Status = -1, Message = "Category not deleted" });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while deleting category");
                return BadRequest("Could not delete category");
            }
        }

        public IActionResult GetCategory(int categoryId, [FromServices] IChannelService _channelService)
        {
            try
            {

                var response = _channelService.GetCategoryById(categoryId);

                if (response == null)
                    return Json(new { Status = -1, Message = "This category cannot be found" });

                else if (response.Channel.SubscriberId != _tenant.Id)
                    return Json(new { Status = -1, Message = "This category does not exist" });

                else
                    return Json(new { Status = -1, Message = "", Data = response });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while retrieving category");
                return BadRequest("Could not retrieve category");
            }
        }


        #endregion

        #region Pinned Posts

        public async Task<IActionResult> AddPinnedPost(SavePinnedPostVM model, [FromServices] IPinnedPostService _pinnedService)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _mapper.Map<PinnedPost>(model);
                    data.SubscriberId = _tenant.Id;
                    var response = await _pinnedService.CreatePinnedPost(data);

                    if (response == DbActionsResponse.DuplicateExist)
                        return Json(new { Status = -1, Message = "This post exists" });

                    else if (response == DbActionsResponse.Success)
                        return Json(new { Status = 1, Message = "Post saved successfully" });

                    return Json(new { Status = -1, Message = "Post not saved" });
                }

                return Json(new { Status = -1, Message = "Required fileds are empty" });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while creating Post");
                return BadRequest("Could not create Post");
            }
        }

        public async Task<IActionResult> UpdatePinnedPost(SavePinnedPostVM model, [FromServices] IPinnedPostService _pinnedService)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _pinnedService.UpdatePost(model.Id, model.Content, _tenant.Id);

                    if (response == DbActionsResponse.NotFound)
                        return Json(new { Status = -1, Message = "This Post cannot be found" });

                    else if (response == DbActionsResponse.DuplicateExist)
                        return Json(new { Status = -1, Message = "This Post exists" });

                    else if (response == DbActionsResponse.Success)
                        return Json(new { Status = 1, Message = "Post updated successfully" });

                    return Json(new { Status = -1, Message = "Post not updated" });
                }

                return Json(new { Status = -1, Message = "Required fileds are empty" });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while updating Post");
                return BadRequest("Could not update Post");
            }
        }

        public async Task<IActionResult> DeletePinnedPost(int postId, [FromServices] IPinnedPostService _pinnedService)
        {
            try
            {

                var response = await _pinnedService.DeletePost(postId, _tenant.Id);

                if (response == DbActionsResponse.NotFound)
                    return Json(new { Status = -1, Message = "This Post cannot be found" });

                else if (response == DbActionsResponse.DeleteDenied)
                    return Json(new { Status = -1, Message = "This Post cannot be deleted" });

                else if (response == DbActionsResponse.Success)
                    return Json(new { Status = 1, Message = "Post deleted successfully" });

                return Json(new { Status = -1, Message = "Post not deleted" });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while deleting Post");
                return BadRequest("Could not delete Post");
            }
        }

        public async Task<IActionResult> TogglePinnedPost(int postId, [FromServices] IPinnedPostService _pinnedService)
        {
            try
            {

                var response = await _pinnedService.ToggleActive(postId, _tenant.Id);

                if (response == DbActionsResponse.NotFound)
                    return Json(new { Status = -1, Message = "This Post cannot be found" });

                else if (response == DbActionsResponse.DeleteDenied)
                    return Json(new { Status = -1, Message = "This Post's status cannot be changed" });

                else if (response == DbActionsResponse.Success)
                    return Json(new { Status = 1, Message = "Status changed successfully" });

                return Json(new { Status = -1, Message = "Post's status not changed" });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while changing Post's status");
                return BadRequest("Could not change Post's status");
            }
        }

        public IActionResult GetPinnedPost(int postId, [FromServices] IPinnedPostService _pinnedService)
        {
            try
            {

                var response = _pinnedService.GetPinnedPostById(postId);

                if (response == null)
                    return Json(new { Status = -1, Message = "This Post cannot be found" });

                else if (response.SubscriberId != _tenant.Id)
                    return Json(new { Status = -1, Message = "This Post does not exist" });

                else
                    return Json(new { Status = -1, Message = "", Data = response });

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while retrieving Post");
                return BadRequest("Could not retrieve Post");
            }
        }


        #endregion

        #region Moderators


        #endregion

    }
}
