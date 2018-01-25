using Hydra.Admin.IServices;
using Hydra.Admin.Models;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Utility;
using Microsoft.AspNetCore.Mvc;
using Plugin.Cache;
using System;

namespace Hydra.Admin.API.Controllers
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    [Produces("application/json")]
    [Route("api/Menu")]
    public class MenuController : BaseAPIController
    {
        private readonly IMenuService IMenuService;
        public MenuController(IMenuService IMenuService)
        {
            this.IMenuService = IMenuService;
        }
        /// <summary>
        /// 系统菜单新增
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [Log2DataBaseFilter("系统菜单新增")]
        public JsonResult Add(Menus menu)
        {
            var ret = APIFunc(menu, "---系统菜单新增---", (d, p) =>
            {
                var res = new AjaxResult();
                d.Id = Guid.NewGuid().ToString();
                d.ParentId = d.ParentId ?? Guid.Empty.ToString();
                res.data = IMenuService.Insert(d);
                Cache.Remove(CacheKeys.Cache_System_Menu);
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 系统菜单删除
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [Log2DataBaseFilter("系统菜单删除")]
        public JsonResult Remove(string menuId)
        {
            var ret = APIFuncOT(menuId, "---系统菜单删除---", (d, p) =>
            {
                var res = new AjaxResult();
                var canRemove = IMenuService.Count(s => s.ParentId == menuId);
                if (canRemove > 0)
                {
                    res.state = "error";
                    res.message = "节点存在下级附属";
                }
                else
                {
                    res.data = IMenuService.Delete(s => s.Id.Equals(d.ToString()));
                    Cache.Remove(CacheKeys.Cache_System_Menu);
                }
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 系统菜单编辑
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [Log2DataBaseFilter("系统菜单编辑")]
        public JsonResult Update(Menus menu)
        {
            var ret = APIFunc(menu, "---系统菜单编辑---", (d, p) =>
            {
                var res = new AjaxResult();
                d.ParentId = d.ParentId ?? Guid.Empty.ToString();
                d.ModifyTime = DateTime.Now;
                res.data = IMenuService.Update(d);
                Cache.Remove(CacheKeys.Cache_System_Menu);
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="menuId">菜单编号</param>
        /// <returns></returns>
        [HttpGet("Find")]
        public JsonResult Find(string menuId)
        {
            var ret = APIFuncOT(menuId, "---获取菜单---", (d, p) =>
            {
                var res = new AjaxResult();
                res.data = IMenuService.FirstOrDefault(d);
                return res;
            });
            return Json(ret);
        }
    }
}