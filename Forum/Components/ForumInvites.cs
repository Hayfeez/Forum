using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Forum.DataAccessLayer.IService;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Components
{
    public class ForumInvites : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService;
        private readonly Subscriber _tenant;

        public ForumInvites(IMapper mapper, ITenantService tenantService, Subscriber tenant)
        {
            _mapper = mapper;
            _tenantService = tenantService;
            _tenant = tenant;
        }

        public IViewComponentResult Invoke()
        {
            var model = new List<TenantInviteList>();
            try
            {
                var allusers = _tenantService.GetAllTenantInvites(_tenant.Id).ToList();
                model = _mapper.Map<List<TenantInviteList>>(allusers);
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
    }
}
