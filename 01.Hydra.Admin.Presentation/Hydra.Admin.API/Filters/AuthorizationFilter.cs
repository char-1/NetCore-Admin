using Hydra.Admin.Models;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Plugin.Cache;
using System;
using System.Linq;

namespace Hydra.Admin.API
{
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter)) return;
            var Authorization = context.HttpContext.Request.Headers.FirstOrDefault(s => s.Key.Equals("Authorization"));
            if (!Authorization.Value.Any() || string.IsNullOrEmpty(Authorization.Value[0]))
            {
                context.Result = new JsonResult(new AjaxResult
                {
                    state = "noAuth",
                    message = "未授权的操作"
                });
            }
            else
            {
                //校验 Authorization 的真实性
                var token = Authorization.Value[0];
                var authorization = Cache.Get<Admins>(string.Format(CacheKeys.Cache_Admin_Token, token));
                if (authorization == null)
                {
                    context.Result = new JsonResult(new AjaxResult
                    {
                        state = "faildAuth",
                        message = "授权已失效"
                    });
                }
            }
        }
    }
}
