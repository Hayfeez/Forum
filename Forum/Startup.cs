using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Forum.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Forum.DataAccessLayer.IService;
using Forum.DataAccessLayer.Service;
using AutoMapper;
using Forum.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Forum.Models;
using SaasKit.Multitenancy.Internal;

namespace Forum
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<IdentityUser, IdentityRole>(options =>
            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

            }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
               // .AddDefaultTokenProviders();
               // .AddClaimsPrincipalFactory<UserClaimFactory>();
            //<---- add this line ;

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
           // services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddControllers(config =>
            {
                // using Microsoft.AspNetCore.Mvc.Authorization;
                // using Microsoft.AspNetCore.Authorization;
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            //services.AddSession();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Account/Login";
                options.LogoutPath = $"/Account/Logout";
                options.AccessDeniedPath = $"/Account/AccessDenied";
            });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.Configure<ReadAppSettings>(Configuration.GetSection("AppConfig"));
            services.AddTransient<SeedData>();

            services.AddMultitenancy<Subscriber, TenantResolver>();

            services.AddSingleton<MailHelper>();
            services.AddSingleton<LoadStaticContent>();
            services.AddTransient<LoadDynamicContent>();

            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IThreadService, ThreadService>();
            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddScoped<IPinnedPostService, PinnedPostService>();
            services.AddScoped<IThreadInfoService, ThreadInfoService>();
            services.AddScoped<IForumUserService, ForumUserService>();

        }

        private static bool CheckTenant(HttpContext context)
        {
            var path = context.Request.Path;
            return !path.StartsWithSegments("/default");  //return false if route is in default controller
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedData seedData)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

           // seedData.ClearDB();
            seedData.Seed();

            app.UseHttpsRedirection();
            app.UseStaticFiles();           

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           
            app.UseMultitenancy<Subscriber>();
            // app.UseMiddleware<TenantUnresolvedRedirectMiddleware<Subscriber>>("http://saaskit.net", false);
            // app.UseMiddleware<TenantUnresolvedRedirectMiddleware<Subscriber>>("/tenantnotfound.html", false);

            app.UseWhen(CheckTenant, appBuilder =>
            {
                appBuilder.UseMiddleware<CustomTenantMiddleware>();
            });

            app.Use(async (ctx, next) =>
            {
                await next.Invoke();

                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    string originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/404";
                    //await next();
                }
            });

            // var supportedCultures = new[]
            //{
            //     new CultureInfo("en-NG")
            // };

            // app.UseRequestLocalization(new RequestLocalizationOptions
            // {
            //     DefaultRequestCulture = new RequestCulture("en-NG"),
            //     SupportedCultures = supportedCultures,
            //     SupportedUICultures = supportedCultures
            // });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
