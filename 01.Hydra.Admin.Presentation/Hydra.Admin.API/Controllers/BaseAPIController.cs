using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Hydra.Admin.Utility;
using Microsoft.AspNetCore.Cors;
using Hydra.Admin.Utility.Helper;
using Newtonsoft.Json;

namespace Hydra.Admin.API.Controllers
{
    [EnableCors("hydra.admin")]
    public class BaseAPIController : Controller
    {
        /// <summary>
        /// 接口操作
        /// </summary>
        /// <param name="data"></param>
        /// <param name="desc"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual AjaxResult APIFunc(JObject data, string desc, Func<JObject, string, AjaxResult> func)
        {
            var ret = new AjaxResult();
            try
            {
                ret = func(data, desc);
            }
            catch (Exception ex)
            {
                ret.message = ex.Message;
                ret.state = "error";
            }
            return ret;
        }
        /// <summary>
        /// 接口操作
        /// </summary>
        /// <param name="data"></param>
        /// <param name="desc"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual async Task<AjaxResult> APIFuncAsync(JObject data, string desc, Func<JObject, string, Task<AjaxResult>> func)
        {
            var ret = new AjaxResult();
            try
            {
                var dic = Sign.GetParameters(data.ToString());
                if (!Sign.Validate(dic))
                {
                    ret.state = "error";
                    ret.message = "参数验证失败";
                }
                else
                {
                    //if (dic.ContainsKey("sign"))
                    //    dic.Remove("sign");
                    ret = await func(data, desc);
                }
            }
            catch (Exception ex)
            {
                ret.message = "提交失败";
                ret.state = "error";
                Log.Error(desc, ex);
            }
            return ret;
        }



        /// <summary>
        /// 接口操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="desc"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual AjaxResult APIFunc<T>(T data, string desc, Func<T, string, AjaxResult> func) where T : class, new()
        {
            var ret = new AjaxResult();
            try
            {
                ret = func(data, desc);
            }
            catch (Exception ex)
            {
                ret.message = ex.Message;
                ret.state = "error";
            }
            return ret;
        }
        /// <summary>
        /// 接口操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="desc">接口描述</param>
        /// <param name="func">返回</param>
        /// <returns></returns>
        public virtual AjaxResult APIFuncOT( object data, string desc, Func<object, string, AjaxResult> func)
        {
            var ret = new AjaxResult();
            try
            {
                ret = func(data, desc);
            }
            catch (Exception ex)
            {
                ret.message = ex.Message;
                ret.state = "error";
            }
            return ret;
        }

        public virtual async Task<AjaxResult> APIFuncOTAsync(object data, string desc, Func<object, string, Task<AjaxResult>> func)
        {
            var ret = new AjaxResult();
            try
            {
                ret =await func(data, desc);
            }
            catch (Exception ex)
            {
                ret.message = "操作失败";
                ret.state = "error";
                Log.Error(desc, ex);
            }
            return ret;
        }

        /// <summary>
        /// 接口操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="desc">接口描述</param>
        /// <param name="func">返回</param>
        /// <returns></returns>
        public virtual AjaxResult APIFuncOT(string desc, Func<object[], string, AjaxResult> func, params object[] data)
        {
            var ret = new AjaxResult();
            try
            {
                ret = func(data, desc);
            }
            catch (Exception ex)
            {
                ret.message = ex.Message;
                ret.state = "error";
            }
            return ret;
        }
    }
}