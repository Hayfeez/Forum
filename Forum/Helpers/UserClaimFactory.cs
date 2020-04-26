using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Forum.Helpers
{
    public static class CustomClaimTypes
    {
        public const string SusbcriberUserId = "SusbcriberUserId";
        public const string SusbcriberUserRole = "SusbcriberUserRole";
       // public const string SusbcriberId = "SusbcriberId";
    }

    public static class IdentityExtensions
    {
        //public static int GetSubscriberId(this IIdentity identity)
        //{
        //    ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
        //    Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.SusbcriberId);

        //    if (claim == null)
        //        return 0;

        //    return int.Parse(claim.Value);
        //}

        public static long GetSubscriberUserId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.SusbcriberUserId);

            if (claim == null)
                return 0;

            return long.Parse(claim.Value);
        }

        public static string GetSubscriberUserRole(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.SusbcriberUserRole);

            return claim?.Value ?? string.Empty;
        }


        public static string GetName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Name);

            return claim?.Value ?? string.Empty;
        }
    }

    
}
