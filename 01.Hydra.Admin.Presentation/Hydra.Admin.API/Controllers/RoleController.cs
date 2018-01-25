using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hydra.Admin.IServices;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Utility;
using Hydra.Admin.Models.Query;
using Newtonsoft.Json.Linq;
using Hydra.Admin.Utility.Helper;
using Microsoft.AspNetCore.Cors;

namespace Hydra.Admin.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Role")]
    public class RoleController : BaseAPIController
    {
        private readonly IRoleService IRoleService;
        private readonly IAdminService IAdminService;
        public RoleController(IRoleService IRoleService, IAdminService IAdminService)
        {
            this.IRoleService = IRoleService;
            this.IAdminService = IAdminService;
        }
        /// <summary>
        /// 系统角色新增
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [Log2DataBaseFilter("系统角色新增")]
        public JsonResult Add(Roles role)
        {
            var ret = APIFunc(role, "---系统角色新增---", (d, p) =>
            {
                var res = new AjaxResult();
                d.Id = Guid.NewGuid().ToString();
                res.data = IRoleService.Insert(d);
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 系统角色删除
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [Log2DataBaseFilter("系统角色删除")]
        public JsonResult Remove(string roleId)
        {
            var ret = APIFuncOT(roleId, "---系统角色删除---", (d, p) =>
            {
                var res = new AjaxResult();
                var canRemove = IAdminService.Count(s => s.RoleId == roleId);
                if (canRemove > 0)
                {
                    res.state = "error";
                    res.message = "角色已被分配使用";
                }
                else
                {
                    res.data = IRoleService.Delete(s => s.Id.Equals(d.ToString()));
                }
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 系统角色编辑
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [Log2DataBaseFilter("系统角色编辑")]
        public JsonResult Update(Roles role)
        {
            var ret = APIFunc(role, "---系统角色编辑---", (d, p) =>
            {
                var res = new AjaxResult();
                res.data = IRoleService.Update(d);
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 系统角色启用/停用
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="enable">启用/停用</param>
        /// <returns></returns>
        [HttpPost("Enable")]
        [Log2DataBaseFilter("系统角色启用/停用")]
        public IActionResult Enable(EnableQuery query)
        {
            var ret = APIFunc(query, "---系统角色启用/停用---", (d, p) =>
            {
                var res = new AjaxResult();
                res.data = IRoleService.Update(s => new Roles
                {
                    IsEnable = d.IsEnable
                }, w => w.Id == d.Id);
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 系统角色列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("Grid")]
        public IActionResult Grid(BaseQuery query)
        {
            var ret = APIFunc(query, "---系统角色列表---", (d, p) =>
            {
                var res = new AjaxResult();
                res.data = IRoleService.GetRoleGrid(d);
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        [HttpGet("Permission")]
        public IActionResult Permission(string roleId, bool isMenu = false)
        {
            var ret = APIFuncOT("---获取角色权限---", (d, p) =>
            {
                var res = new AjaxResult();
                res.data = IRoleService.GetRolePermission(d[0].ToString(), d[1].ToBoolean());
                return res;
            }, roleId, isMenu);
            return Json(ret);
        }
        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        [HttpPost("SetPerission")]
        [Log2DataBaseFilter("设置角色权限")]
        public IActionResult SetPerission([FromForm]SetRolePermissionQuery data)
        {
            var ret = APIFunc(data, "---设置角色权限---", (d, p) =>
            {
                var res = new AjaxResult();
                res.data = IRoleService.SetRolePermission(d.roleId, d.permissionNodes.Split(','));
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        [HttpGet("Find")]
        public JsonResult Find(string roleId)
        {
            var ret = APIFuncOT(roleId, "---获取角色---", (d, p) =>
            {
                var res = new AjaxResult();
                res.data = IRoleService.FirstOrDefault(d);
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        [HttpGet("Select")]
        public JsonResult Select()
        {
            var ret = APIFunc(new Roles(), "---获取角色---", (d, p) =>
            {
                var res = new AjaxResult();
                res.data = IRoleService.GetSelectRole(s => s.IsEnable);
                return res;
            });
            return Json(ret);
        }
    }
}