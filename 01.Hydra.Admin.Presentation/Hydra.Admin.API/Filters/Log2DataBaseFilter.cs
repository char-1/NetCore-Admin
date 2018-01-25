using Hydra.Admin.Models.Model;
using Hydra.Admin.Services;
using Hydra.Admin.Utility.Helper;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Admin.API
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public class Log2DataBaseFilter : ActionFilterAttribute
    {
        private readonly string _type;
        public Log2DataBaseFilter(string type)
        {
            _type = type;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var Authorer = context.HttpContext.Request.Headers.FirstOrDefault(s => s.Key.Equals("Authorer"));
            var log = new Logs
            {
                Id = Guid.NewGuid().ToString(),
                CreateTime = DateTime.Now,
                User = Authorer.Value.Any() ? Authorer.Value[0] : "Admin",
                IP = NetHelper.GetIPAddress(),
                Types = _type,
                Controller = context.RouteData.Values["controller"].ToString(),
                Action = context.RouteData.Values["action"].ToString(),
                Remark = GetRemark(context)
            };
            Task.Run(() =>
            {
                new LogsService().Insert(log);
            });
            base.OnActionExecuting(context);
        }

        #region private method 私有方法
        
        //组装提交参数
        private static string GetRemark(ActionExecutingContext filterContext)
        {
            var pv = "";
            var parametersCount = filterContext.ActionArguments;
            if (!parametersCount.Any()) return pv;
            var keys = filterContext.ActionArguments.Keys;
            foreach (var key in keys)
            {
                var value = filterContext.ActionArguments[key];
                if (null == value)
                    continue;
                if (value.GetType().IsClass && value.GetType().Name != "String" && value.GetType().Name != "JObject")
                {
                    var objClass = value;
                    var infos = objClass.GetType().GetProperties();
                    pv = infos.Where(info => info.CanRead).Aggregate(pv, (current, info) => current + (info.Name + "=" + info.GetValue(objClass, null) + ","));
                }
                else
                {
                    if (value.GetType().Name != "JObject")
                        pv += key + "=" + JsonConvert.SerializeObject(value) + ",";
                    else
                        pv += key + "=" + value + ",";
                }
            }
            return pv;
        }
        #endregion

    }
}
