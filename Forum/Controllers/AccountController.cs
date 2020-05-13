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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using Forum.DataAccessLayer.IService;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Forum.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly Subscriber _tenant;

        [TempData]
        public string ErrorMessage { get; set; }

        public AccountController(ILogger<HomeController> logger, Subscriber tenant, IMapper mapper,
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IAccountService accountService)
        {
            _logger = logger;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _accountService = accountService;
            _tenant = tenant;
        }

        #region Join

        public IActionResult JoinForum()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            return View(new JoinVM());
        }

        [HttpPost]
        public async Task<IActionResult> JoinForum(JoinVM model, [FromServices]MailHelper _mailHelper)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkEmail = _accountService.IsUserASubscriberUser(model.Email, _tenant.Id);
                    if (checkEmail)
                    {
                        ModelState.AddModelError(string.Empty, "Email address is already in use");
                        return View(model);
                    }
                    var code = Guid.NewGuid().ToString(); //await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var createInvite = await _accountService.CreateSubscriberInvite(new SubscriberInvite
                    {
                        DateCreated = DateTime.Now,
                        Email = model.Email,
                        InviteCode = code,
                        SubscriberId = _tenant.Id,
                    }, false);

                    if (createInvite == DbActionsResponse.DuplicateExist)
                    {
                        //user has an invite
                        ModelState.AddModelError(string.Empty, "There is a pending invite. Accept the invitation mail already sent to you");
                        return View(model);
                    }
                    else if (createInvite == DbActionsResponse.DeleteDenied)
                    {
                        //user has an invite
                        ModelState.AddModelError(string.Empty, "You cannot join this forum. An administrator has to invite you to the forum");
                        return View(model);
                    }
                    else if (createInvite == DbActionsResponse.Success)
                    {
                        _logger.LogInformation("User Invite saved.");
                        var callbackUrl = Url.Action("Register", "Account", new { inviteCode = code, email = model.Email }, Request.Scheme);

                        _mailHelper.SendMail(
                           model.Email,
                           $"Please confirm your account on the {_tenant.Name} forum by <a href='{callbackUrl}'>clicking here</a>.",
                           $"{_tenant.Name} Forum Invitation");

                        //add success message
                        return View(new JoinVM());
                    }
                }

                // If we got this far, something failed, redisplay form
                ModelState.AddModelError(string.Empty, "An error occured");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while saving join forum");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }         

        }

        #endregion

        #region Login

        public async Task<IActionResult> Login(string returnUrl)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");
            // Clear the existing external cookie to ensure a clean login process
               await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var model = new LoginVM
            {
                ReturnUrl = returnUrl,
                // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {       
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityUser user;
                    user = await _userManager.FindByNameAsync(model.UsernameEmail);
                    if (user == null)
                        user = await _userManager.FindByEmailAsync(model.UsernameEmail);

                    if (user != null)
                    {
                        // Clear the existing external cookie to ensure a clean login process
                        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                        
                        var subscriberUser = _accountService.GetSubscriberUser(user.Id, _tenant.Id);
                        if (subscriberUser == null || !subscriberUser.IsActive)
                        {                         
                            ModelState.AddModelError(string.Empty, "Account does not exist on this forum");
                            return View(model);
                        }
                        var hasher = new PasswordHasher<IdentityUser>();
                        PasswordVerificationResult pwdResult = hasher.VerifyHashedPassword(user, subscriberUser.Password, model.Password);
                        if (pwdResult != PasswordVerificationResult.Failed)
                        {
                            var ident = await _userManager.AddClaimsAsync(user, new[]
                            {
                                new Claim(CustomClaimTypes.SusbcriberUserId, subscriberUser.Id.ToString()),
                                new Claim(CustomClaimTypes.SusbcriberUserRole, subscriberUser.UserRole),
                                new Claim(CustomClaimTypes.SusbcriberUserImageUrl, subscriberUser.ProfileImageUrl)
                            });

                            await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);
                            _logger.LogInformation("User logged in.");

                            return LocalRedirect(model.ReturnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid credentials.");
                            return View(model);
                        }
                    }

                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while loggin in");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }           
        }

        #endregion
               
        #region Register

        public async Task<IActionResult> Register(string inviteCode, string email)
        {
            if(string.IsNullOrEmpty(inviteCode) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var getInvite = _accountService.GetSubscriberInvite(email, _tenant.Id);
                if (getInvite == null) // no pending invite for subscriber
                {
                    return RedirectToAction("Index", "Home");
                }

                if (!inviteCode.Equals(getInvite.InviteCode))
                {
                    return RedirectToAction("Index", "Home");
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                }

                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                var model = new RegisterVM
                {
                    Email = getInvite.Email,
                    InviteCode = getInvite.InviteCode
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while verifying invite code");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }           
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model, [FromServices]MailHelper _mailHelper)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Password != model.ConfirmPassword)
                    {
                        ModelState.AddModelError("", "New Password and Confirm Password must be the same");
                        return View(model);
                    }

                    var getInvite = _accountService.GetSubscriberInvite(model.Email, _tenant.Id);
                    if (getInvite == null) // no pending invite for subscriber
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    if (!model.InviteCode.Equals(getInvite.InviteCode))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    IdentityUser user;
                    user = await _userManager.FindByEmailAsync(model.Email);
                    bool createSubscriberUser = false;
                    if (user == null)
                    {
                        user = new IdentityUser { UserName = model.Username, Email = model.Email };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                            createSubscriberUser = true;
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else
                    {
                        createSubscriberUser = true;
                    }

                    if (createSubscriberUser)
                    {
                        var hasher = new PasswordHasher<IdentityUser>();
                        var obj = _mapper.Map<SubscriberUser>(model);
                        obj.Password = hasher.HashPassword(user, model.Password);
                        obj.ApplicationUserId = user.Id;
                        obj.SubscriberId = _tenant.Id;
                        obj.ProfileImageUrl = "";
                        var response = await _accountService.CreateSubscriberUser(obj);
                        if (response == DbActionsResponse.DuplicateExist)
                        {
                            //failed, remove user created in application user
                            return RedirectToAction("Login", "Account");
                        }

                        else if (response == DbActionsResponse.Success)
                        {
                            //delete the invite
                            var deleteInvite = await _accountService.DeleteSubscriberInvite(getInvite);

                            _logger.LogInformation("User created a new account with password.");
                            var callbackUrl = Url.Action("Login", "Account");
                            _mailHelper.SendMail(
                               model.Email,
                               $"Your account on the {_tenant.Name} forum has been successfully created. Login by <a href='{callbackUrl}'>clicking here</a>.",
                               $"{_tenant.Name} Forum Account Created");

                            //add success message
                            return RedirectToAction("Login", "Account");
                            //await _signInManager.SignInAsync(user, isPersistent: false);
                        }
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while registering user");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }           
        }

        #endregion

        #region Logout

        [Authorize, HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");
               
                if (returnUrl != null)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while logging out ");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }            
        }

        #endregion


        #region Forgot & Reset Password
 
        public IActionResult ForgotPassword()
        {
            var model = new ForgotPasswordVM();
           
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model, [FromServices]MailHelper _mailHelper)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    //save reset code and send email
                    var code = Guid.NewGuid().ToString();
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var reset = await _accountService.CreateResetPasswordCode(new ResetPasswordCode
                    {
                        DateCreated = DateTime.Now,
                        SubscriberId = _tenant.Id,
                        Email = model.Email,
                        ResetCode = code
                    });
                    if (reset == DbActionsResponse.DuplicateExist)
                    {
                        //user has an invite
                        ModelState.AddModelError(string.Empty, "There is a pending reset password link.");
                        return View(model);
                    }
                    else if (reset == DbActionsResponse.Success)
                    {
                        _logger.LogInformation("User Reset password code saved.");
                        var callbackUrl = Url.Action("ResetPassword", "Account", new { resetCode = code, email = model.Email }, Request.Scheme);

                        _mailHelper.SendMail(
                           model.Email,
                           $"Please reset the password to your account on the {_tenant.Name} forum by <a href='{callbackUrl}'> clicking here </a>.",
                           $"{_tenant.Name} Forum - Reset Password");

                        //add success message
                        return View(new ForgotPasswordVM());
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while handling forgot password");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        public IActionResult ResetPassword(string resetCode, string Email)
        {
            if (string.IsNullOrEmpty(resetCode) || string.IsNullOrEmpty(Email))
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var getResetCode = _accountService.GetResetPasswordCode(Email, _tenant.Id);
                if (getResetCode == null) // no pending reset for subscriber
                {
                    return RedirectToAction("Index", "Home");
                }

                if (!resetCode.Equals(getResetCode.ResetCode))
                {
                    return RedirectToAction("Index", "Home");
                }               

                var model = new ResetPasswordVM
                {
                    Email = Email,
                    ResetCode = resetCode
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while verifying reset code");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }
          

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.NewPassword != model.ConfirmNewPassword)
                    {
                        ModelState.AddModelError("", "New Password and Confirm Password must be the same");
                        return View(model);
                    }

                    var getResetCode = _accountService.GetResetPasswordCode(model.Email, _tenant.Id);
                    if (getResetCode == null) // no pending reset for subscriber
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    if (!model.ResetCode.Equals(getResetCode.ResetCode))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    //begin reset
                    IdentityUser user;
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if(user == null)
                    {
                        ModelState.AddModelError("", "User not found");
                        return View(model);
                    }

                    var hasher = new PasswordHasher<IdentityUser>();
                    string newPassword = hasher.HashPassword(user, model.NewPassword);
                    var resetPwd = await _accountService.ResetSubscriberUserPassword(model.Email, newPassword);
                    if (resetPwd == DbActionsResponse.NotFound)
                    {
                        ModelState.AddModelError(string.Empty, "User not found");
                        return View(model);
                    }
                    else if (resetPwd == DbActionsResponse.Success)
                    {
                        _logger.LogInformation("User password reset successfully");
                        //delete the resetcode
                        var deleteCode = await _accountService.DeleteResetPasswordCode(getResetCode);

                        //add success message
                        return RedirectToAction("Login", "Account");
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while handling reset password");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }


        #endregion

        #region Change Password
        public IActionResult ChangePassword()
        {
            try
            {

                return View(new ChangePasswordVM());
            }
            catch (Exception ex)
            {
                return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }

        [Authorize, HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.NewPassword != model.ConfirmNewPassword)
                    {
                        ModelState.AddModelError("", "New Password and Confirm Password must be the same");
                        return View(model);
                    }

                    string name = User.Identity.Name;
                    IdentityUser user;
                    user = await _userManager.FindByNameAsync(name);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "User not found");
                        return View(model);
                    }

                    var hasher = new PasswordHasher<IdentityUser>();
                    string newPassword = hasher.HashPassword(user, model.NewPassword);
                    var changePwd = await _accountService.ResetSubscriberUserPassword(user.Email, newPassword);
                    if (changePwd == DbActionsResponse.NotFound)
                    {
                        ModelState.AddModelError(string.Empty, "User not found");
                        return View(model);
                    }
                    else if (changePwd == DbActionsResponse.DeleteDenied)
                    {
                        ModelState.AddModelError(string.Empty, "Current Password is incorrect. Enter your correct password");
                        return View(model);
                    }
                    else if (changePwd == DbActionsResponse.Success)
                    {
                        _logger.LogInformation("User password changed successfully");

                        await _signInManager.SignOutAsync(); //sign user out

                        //add success message
                        return RedirectToAction("Login", "Account");
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while handling change password");
                return View("Views/Shared/Error.cshtml", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }


        #endregion
    }
}
