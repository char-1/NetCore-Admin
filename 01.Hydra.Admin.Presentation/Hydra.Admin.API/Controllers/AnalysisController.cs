using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hydra.Admin.IServices;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility;

namespace Hydra.Admin.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Analysis")]
    public class AnalysisController : BaseAPIController
    {
        private readonly IAnalysisRemainService IAnalysisRemainService;
        private readonly IAnalysisDashBoardService IAnalysisDashBoardService;
        private readonly IplayerGoldService IplayerGoldService;
        public AnalysisController(IAnalysisRemainService IAnalysisRemainService, IAnalysisDashBoardService IAnalysisDashBoardService, IplayerGoldService IplayerGoldService)
        {
            this.IAnalysisRemainService = IAnalysisRemainService;
            this.IAnalysisDashBoardService = IAnalysisDashBoardService;
            this.IplayerGoldService = IplayerGoldService;
        }

        [HttpGet("DashBoardGrid")]
        public JsonResult DashBoardGrid(BaseQuery query)
        {
            var ret = APIFunc(query, "---获取DashBoardGrid面板数据---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IAnalysisDashBoardService.GetDashBoardGrid(d)
                };
                return res;
            });
            return Json(ret);
        }

        [HttpGet("RemainGrid")]
        public JsonResult RemainGrid(BaseQuery query)
        {
            var ret = APIFunc(query, "---获取RemainGrid面板数据---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IAnalysisRemainService.GetRemainGrid(d)
                };
                return res;
            });
            return Json(ret);
        }

        [HttpGet("DashBoard")]
        public JsonResult DashBoard(DashBoardQuery query)
        {
            var ret = APIFunc(query, "---获取DashBoard面板数据---", (d, p) =>
            {
                var res = new AjaxResult
                {
                    data = IAnalysisDashBoardService.GetDashBoard(d)
                };
                return res;
            });
            return Json(ret);
        }
        [HttpGet("DashBoardAsync")]
        public async Task<JsonResult> DashBoardAsync(DashBoardQuery query)
        {
            var data = await IAnalysisDashBoardService.GetDashBoardAsync(query);
            return Json(new AjaxResult
            {
                data = data
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("PlayerGold")]
        public async Task<JsonResult> PlayerGold(PlayerGoldQuery query)
        {
            var data = await IplayerGoldService.GetPlayerGoldGridFootAsync(query);
            return Json(new AjaxResult
            {
                data = data
            });
        }
    }
}