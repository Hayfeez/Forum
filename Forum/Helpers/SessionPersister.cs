using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Forum.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Forum.Helpers
{
    public static class SessionPersister
    {
        private static IHttpContextAccessor _httpContextAccessor => new HttpContextAccessor();

        #region IsAuthenticated
        public static bool IsAuthenticated
        {
            get
            {
                if (_httpContextAccessor.HttpContext.Session.GetString("LoggedInUser") != null)
                {
                    return true;
                }
                return false;
            }
        }

        //public static bool IsForumModerator
        //{
        //    get
        //    {
        //        return (IsAuthenticated && GetClaim(ClaimTypes.Role) == "User");
        //    }
        //}
        #endregion

        #region Profile Fields
        public static string Subscriber_Id = string.Empty;
        //public static string Role_Id = string.Empty;               

        //public static long RoleId
        //{
        //    get { return GetClaimInt64(Role_Id); }
        //}

        public static long SubscriberUserId
        {
            get { return GetClaimInt64(ClaimTypes.NameIdentifier); }
        }

        public static string Email
        {
            get { return GetClaim(ClaimTypes.Email); }
        }

        public static string Account_Name
        {
            get { return GetClaim(ClaimTypes.Name); }
        }

        public static string Role_Name
        {
            get { return GetClaim(ClaimTypes.Role); }
        }


        public static int SubscriberId
        {
            get { return GetClaimInt32(Subscriber_Id); }
        }
        #endregion

        //Get Claims
        private static int GetClaimInt32(string claimType)
        {
            return int.Parse(GetClaim(claimType) ?? "0");
        }

        private static long GetClaimInt64(string claimType)
        {
            return long.Parse(GetClaim(claimType) ?? "0");
        }


        public static string GetClaim(string claimType)
        {
            string acctString = _httpContextAccessor.HttpContext.Session.GetString("LoggedInUser");
            if (acctString != null)
            {
                var acc = JsonConvert.DeserializeObject<List<Claim>>(acctString, new ClaimsConverter());
                if (acc != null)
                {
                    var claim = acc.FirstOrDefault(c => c.Type == claimType);
                    if (claim != null)
                        return claim.Value;
                }
            }
            return null;
        }

        public static void SetAuthentication(SubscriberUser acc)
        {
            //create claim basic.
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, acc.Id.ToString()),
                   // new Claim(ClaimTypes.Name, acc.Fullname),
                    new Claim(ClaimTypes.Email, acc.Email),
                  //  new Claim(ClaimTypes.Role, acc.RoleName),
                    new Claim(Subscriber_Id, acc.SubscriberId.ToString())
                    
                };

            _httpContextAccessor.HttpContext.Session.SetString("LoggedInUser", JsonConvert.SerializeObject(claims));

        }

        public static void SignOut()
        {
            _httpContextAccessor.HttpContext.Session.Remove("LoggedInUser");

        }
    }
}