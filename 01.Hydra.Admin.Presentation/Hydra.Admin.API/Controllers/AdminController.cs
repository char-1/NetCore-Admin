using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hydra.Admin.IServices;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility;
using Newtonsoft.Json.Linq;
using AutoMapper;
using Hydra.Admin.Models.View;
using Hydra.Admin.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Plugin.Cache;
using Hydra.Admin.Models;

namespace Hydra.Admin.API.Controllers
{
    /// <summary>
    /// 管理员 API
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin")]
    public class AdminController : BaseAPIController
    {
        private readonly IAdminService IAdminService;
        private readonly IMenuService IMenuService;
        public AdminController(IAdminService IAdminService, IMenuService IMenuService)
        {
            this.IAdminService = IAdminService;
            this.IMenuService = IMenuService;
        }
        /// <summary>
        /// 管理员新增
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [Log2DataBaseFilter("管理员新增")]
        public JsonResult Add(Admins admin)
        {
            var ret = APIFunc(admin, "---管理员新增---", (d, p) =>
            {
                var res = new AjaxResult();

                var passSalt = Guid.NewGuid().ToString();
                d.PassSalt = passSalt;
                d.PassWord = SecurityHelper.Md5(passSalt + d.PassWord);
                d.Id = Guid.NewGuid().ToString();
                res.data = IAdminService.Insert(d);
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 管理员删除
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [Log2DataBaseFilter("管理员删除")]
        public IActionResult Remove(string adminId)
        {
            var ret = APIFuncOT(adminId, "---管理员删除---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IAdminService.Delete(s => s.Id.Equals(d))
                };
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 管理员编辑
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [Log2DataBaseFilter("管理员编辑")]
        public JsonResult Update(Admins admin)
        {
            var ret = APIFunc(admin, "---管理员编辑---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IAdminService.Update(s => new Admins
                    {
                        LoginName = d.LoginName,
                        RoleId = d.RoleId,
                        Remark = d.Remark,
                        IsEnable = d.IsEnable,
                        ModifyTime = DateTime.Now
                    }, w => w.Id == d.Id)
                };
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 系统管理员启用
        /// </summary>
        /// <param name="roleId">管理员编号</param>
        /// <param name="enable">是否启用</param>
        /// <returns></returns>
        [HttpPost("Enable")]
        [Log2DataBaseFilter("系统管理员启用")]
        public IActionResult Enable(EnableQuery query)
        {
            var ret = APIFunc(query, "---系统管理员启用---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IAdminService.Update(s => new Admins
                    {
                        IsEnable = d.IsEnable
                    }, w => w.Id == d.Id)
                };
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 管理员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("Grid")]
        public JsonResult Grid(BaseQuery query)
        {
            var ret = APIFunc(query, "---管理员列表---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IAdminService.GetAdminGrid(d)
                };
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="data">登录帐号/密码</param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public JsonResult Login(AdminLoginQuery data)
        {
            var ret = APIFunc(data, "---管理员登录---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IAdminService.Login(d)
                };
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 管理员登出
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task<JsonResult> Logout(string token)
        {
            return await Task.Run(() =>
            {
                Cache.Remove(string.Format(CacheKeys.Cache_Admin_Token, token));
                return Json(new AjaxResult());
            });
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost("ModifyPass")]
        [Log2DataBaseFilter("修改密码")]
        public JsonResult ModifyPass([FromForm] AdminChangePassQuery data)
        {
            var ret = APIFunc(data, "---修改密码---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IAdminService.ModifyPass(d)
                };
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 获取管理员菜单权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        [HttpGet("Permission")]
        public JsonResult Permission(string adminId)
        {
            var ret = APIFuncOT(adminId, "---获取菜单---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IAdminService.GetAdminPermission(d + "")
                };
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 获取管理员
        /// </summary>
        /// <param name="adminId">管理员编号</param>
        /// <returns></returns>
        [HttpGet("Find")]
        public JsonResult Find(string adminId)
        {
            var ret = APIFuncOT(adminId, "---获取菜单---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IAdminService.FirstOrDefault(d)
                };
                return res;
            });
            return Json(ret);
        }
    }
}