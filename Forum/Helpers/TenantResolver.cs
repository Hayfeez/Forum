using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SaasKit.Multitenancy;

namespace Forum.Helpers
{
    public class TenantResolver : MemoryCacheTenantResolver<Subscriber>
    {
        private readonly ApplicationDbContext _context;

        public TenantResolver(
            ApplicationDbContext context, IMemoryCache cache, ILoggerFactory loggerFactory)
             : base(cache, loggerFactory)
        {
            _context = context;
        }

        // Resolver runs on cache misses
        protected override async Task<TenantContext<Subscriber>> ResolveAsync(HttpContext context)
        {
            var subdomain = context.Request.Host.Host.ToLower();
            try
            {
                var tenant = await _context.Tenants
               .FirstOrDefaultAsync(t => t.Domain == subdomain && t.IsActive);

                if (tenant == null) return null;

                return new TenantContext<Subscriber>(tenant);
            }
            catch (Exception ex)
            {
                 return null;
            }
           
        }

        protected override MemoryCacheEntryOptions CreateCacheEntryOptions()
            => new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(2));

        protected override string GetContextIdentifier(HttpContext context)
            => context.Request.Host.Host.ToLower();

        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<Subscriber> context)
            => new string[] { context.Tenant.Domain };
    }

    public class CustomTenantMiddleware
    {
        RequestDelegate next;

        public CustomTenantMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var tenantContext = context.GetTenantContext<Subscriber>();

            if (tenantContext == null)
            {
                context.Response.Redirect("/Default");
                return;
            }

            await next(context);
        }
    }

}
