using AutoMapper;
using Hydra.Admin.IServices;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Models.View;
using Hydra.Admin.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Hydra.Admin.API.Controllers
{
    /// <summary>
    /// 系统 API
    /// </summary>
    [Produces("application/json")]
    [Route("api/System")]
    public class SystemController : BaseAPIController
    {
        private readonly IMenuService IMenuService;
        private readonly IConfigService IConfigService;
        private readonly ILogsService ILogsService;
        public SystemController(IMenuService IMenuService, IConfigService IConfigService, ILogsService ILogsService)
        {
            this.IMenuService = IMenuService;
            this.IConfigService = IConfigService;
            this.ILogsService = ILogsService;
        }

        /// <summary>
        /// 获取角色权限 IView Tree
        /// </summary>
        /// <returns></returns>
        [HttpGet("MenuTree")]
        public IActionResult MenuTree()
        {
            var ret = APIFuncOT("", "---系统角色列表---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IMenuService.GetIViewTreePermissions(s => s.IsEnable)
                };
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 获取系统权限 IView Cascader
        /// </summary>
        /// <returns></returns>
        [HttpGet("MenuCascader")]
        public IActionResult MenuCascader()
        {
            var ret = APIFuncOT("", "---获取系统权限级联---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IMenuService.GetIViewCascaderPermissions(s => s.IsEnable)
                };
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 系统升级 信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("NativeUpdate")]
        public IActionResult NativeUpdate()
        {
            var ret = APIFuncOT("", "---获取系统权限级联---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IMenuService.GetIViewCascaderPermissions(s => s.IsEnable)
                };
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 系统参数配置
        /// </summary>
        /// <param name="config">参数</param>
        /// <returns></returns>
        [HttpPost("SetConfig")]
        [Log2DataBaseFilter("系统参数配置")]
        public IActionResult SetConfig(Config config)
        {
            var ret = APIFunc(config, "---系统参数配置---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IConfigService.SetConfig(config)
                };
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetConfig")]
        [AllowAnonymous]
        public IActionResult GetConfig(bool file = true)
        {
            var data = IConfigService.GetConfig();
            if (file)
                return Json(Mapper.Map<Config, ConfigView>(data));
            else
            {
                var ret = APIFuncOT(file, "---获取系统配置---", (d, p) =>
                {
                    var res = new AjaxResult();
                    res.data = data;
                    return res;
                });
                return Json(ret);
            }
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost("Upload")]
        [AllowAnonymous]
        public IActionResult Upload(string FilePath)
        {
            var ret = APIFuncOT(FilePath, "---文件上传---", (d, p) =>
            {
                var res = new AjaxResult();
                try
                {
                    var oFile = Request.Form.Files["file"];
                    var extName = Path.GetExtension(oFile.FileName);
                    var dateDir = DateTime.Now.ToString("yyMMddHHmmss");
                    Stream sm = oFile.OpenReadStream();
                    var saveDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", FilePath, dateDir);
                    if (!Directory.Exists(saveDir))
                        Directory.CreateDirectory(saveDir);
                    string fileName = string.Format("game_code_{0}{1}", dateDir, extName);
                    var filePath = Path.Combine(saveDir, fileName);
                    using (var fs = new FileStream(Path.Combine(saveDir, fileName), FileMode.OpenOrCreate))
                    {
                        byte[] buffer = new byte[sm.Length];
                        sm.Read(buffer, 0, buffer.Length);
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Dispose();
                    }
                    res.data = new
                    {
                        FileName = fileName,
                        FilePath = filePath,
                        DateDir = dateDir
                    };
                }
                catch (Exception ex)
                {
                    res.message = ex.Message;
                    res.state = "error";
                }
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 获取系统操作日志
        /// </summary>
        /// <returns></returns>
        [HttpGet("Logs")]
        public IActionResult Logs(BaseQuery query)
        {
            var ret = APIFunc(query, "---获取系统操作日志---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = ILogsService.GetLogsGrid(query)
                };
                return res;
            });
            return Json(ret);
        }
        /// <summary>
        /// 删除系统操作日志
        /// </summary>
        /// <param name="logIds"></param>
        /// <returns></returns>
        [HttpPost("RmLogs")]
        public IActionResult RmLogs(string logIds)
        {
            var ret = APIFuncOT(logIds, "---删除系统操作日志---", (d, p) =>
            {
                var res = new AjaxResult();
                res.data = ILogsService.RemoveLogs(d + "");
                return res;
            });
            return Json(ret);
        }
    }
}