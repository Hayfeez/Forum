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
    public class ForumUsers : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService;
        private readonly Subscriber _tenant;

        public ForumUsers(IMapper mapper,ITenantService tenantService, Subscriber tenant)
        {
            _mapper = mapper;
            _tenantService = tenantService;
            _tenant = tenant;
        }

        public IViewComponentResult Invoke()
        {
            var model = new List<TenantUsersList>();
            try
            {
                var allusers = _tenantService.GetAllTenantUsers(_tenant.Id).ToList();
                model =  _mapper.Map<List<TenantUsersList>>(allusers);
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
    }
}
