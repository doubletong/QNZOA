﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using SIGOA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace SIGOA.Server
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 无权限action
        /// </summary>
        public string DeniedAction { get; set; }
        public PermissionRequirement(string deniedAction)
        {
            DeniedAction = deniedAction;
        }
    }

    public class SetActionAttribute : Attribute
    {
        public string ActionName { get; set; }
    }
    //http://bubuko.com/infodetail-2371138.html
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 用户所有权限
        /// </summary>
        public IEnumerable<Menu> RoleMenus { get; set; }
        /// <summary>
        /// 当前方法的名称
        /// </summary>
        private string _actionName = string.Empty;
        private readonly SigbugsdbContext _context;
       // readonly IMenuServices _menuServices;
        //readonly YicaiyunContext _db;
       // readonly ILogger _logger;
        public PermissionHandler(SigbugsdbContext context/*, ILoggerFactory loggerFactory*/)
        {
            //_db = db;
            //IUnitOfWork uow = new UnitOfWork<YicaiyunContext>(_db);
            // _menuServices = menuServices;
            //_logger = loggerFactory.CreateLogger(this.GetType().FullName);
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息  
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext)?.HttpContext;
            //是否ajax
            //bool isAjaxCall = httpContext != null && httpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
            var isAuthenticated = httpContext != null && httpContext.User.Identity.IsAuthenticated;
            //登陆用户为admin 直接跳过
            if (isAuthenticated)
            {
                var currentUser = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Name)?.Value;
                if (currentUser == "admin")
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            var authorizationFilterContext = context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;
            //得到Controller类型   
            Type t = (authorizationFilterContext.ActionDescriptor as ControllerActionDescriptor).ControllerTypeInfo;
            //得到方法名    
            //string actionName = authorizationFilterContext.ActionDescriptor.RouteValues["action"].ToString();
            //得到控制器名
            //string controllerName = authorizationFilterContext.ActionDescriptor.RouteValues["controller"].ToString();

            //得到区域名
            //string areaName = authorizationFilterContext.ActionDescriptor.RouteValues["area"] == null ? "" : authorizationFilterContext.RouteData.Values["area"].ToString();
            
            //获取自定义的特性    
            //var actionAttribute = (authorizationFilterContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(SetActionAttribute), false).FirstOrDefault() as SetActionAttribute;

            //_actionName = actionAttribute == null ? actionName : actionAttribute.ActionName;


            //请求Url
            var questUrl = httpContext.Request.Path.Value.ToLower();
            //是否经过验证

            if (isAuthenticated)
            {
                //if (controllerName.ToLower() == "home" )
                //{
                //    context.Succeed(requirement);
                //    return Task.CompletedTask;
                //}
                var userId = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Sid)?.Value;

                var roles = _context.Roles.Where(r => r.UserRoles.Any(u => u.UserId == new Guid(userId))).ToList();
                RoleMenus = _context.Menus.Include(d=>d.RoleMenus).Where(d=>d.RoleMenus.Any(r=> roles.Contains(r.Role))).ToList();

                bool hasCurrentControllerRole = RoleMenus.Any(w => w.Url?.ToLower() == questUrl);

                if (hasCurrentControllerRole)
                {
                    //当前用户角色名
                    //var roleName = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Role).Value.Split(",");
                    //if (RoleMenus.Where(w => /*roleName.Contains(w.RoleName) &&*/ w.Controller == controllerName.ToLower() && 
                    //w.Action?.ToLower() == _actionName.ToLower() && w.Area == areaName.ToLower()).Any())
                    //{
                    //有权限标记处理成功
                    context.Succeed(requirement);
                    //}
                }

            }
            return Task.CompletedTask;
        }
    }

}
