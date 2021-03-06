﻿using Hydra.Admin.IServices;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Hydra.Admin.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Analysis")]
    public class AnalysisController : BaseAPIController
    {
        private readonly IAnalysisRemainService IAnalysisRemainService;
        private readonly IAnalysisDashBoardService IAnalysisDashBoardService;
        private readonly IplayerGoldService IplayerGoldService;
        private readonly IplayerOnlineService IplayerOnlineService;
        private readonly IAnalysisGameProfitService IAnalysisGameProfitService;
        public AnalysisController(
            IAnalysisRemainService IAnalysisRemainService,
            IAnalysisDashBoardService IAnalysisDashBoardService,
            IplayerGoldService IplayerGoldService,
            IplayerOnlineService IplayerOnlineService,
            IAnalysisGameProfitService IAnalysisGameProfitService)
        {
            this.IAnalysisRemainService = IAnalysisRemainService;
            this.IAnalysisDashBoardService = IAnalysisDashBoardService;
            this.IplayerGoldService = IplayerGoldService;
            this.IplayerOnlineService = IplayerOnlineService;
            this.IAnalysisGameProfitService = IAnalysisGameProfitService;
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
        /// <summary>
        /// 首页统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
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
        /// 玩家金币流水
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
        /// <summary>
        /// 统计在线玩家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("PlayerOnline")]
        public async Task<JsonResult> PlayerOnline(DashBoardQuery query)
        {
            var data = await IplayerOnlineService.GetPlayerOnlineChart(query);
            return Json(new AjaxResult
            {
                data = data
            });
        }
        /// <summary>
        /// 平台收支
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GameProfitAsync")]
        public async Task<JsonResult> GameProfitAsync(GameProfitQuery query)
        {
            var data = await IAnalysisGameProfitService.GetGameProfitViewAsync(query);
            return Json(new AjaxResult
            {
                data = data
            });
        }
        /// <summary>
        /// 平台收支
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("ProfitDetail")]
        public async Task<JsonResult> ProfitDetail(GameProfitQuery query)
        {
            var data = await IAnalysisGameProfitService.GetGameProfitViewAsync(query);
            return Json(new AjaxResult
            {
                data = data
            });
        }
        /// <summary>
        /// 平台收支明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GameProfit")]
        public JsonResult GameProfit(GameProfitQuery query)
        {
            var data = IAnalysisGameProfitService.GetGameProfitView(query);
            return Json(new AjaxResult
            {
                data = data
            });
        }
        /// <summary>
        /// 平台充值
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("PlatRecharge")]
        public JsonResult PlatRecharge(PlayerGoldQuery query)
        {
            var data = IplayerGoldService.GetPlatRecharge(query);
            return Json(new AjaxResult
            {
                data = data
            });
        }
        /// <summary>
        /// 平台支出
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("PlatDistrbute")]
        public JsonResult PlatDistrbute(PlayerGoldQuery query)
        {
            var data = IplayerGoldService.GetPlatDistrubute(query);
            return Json(new AjaxResult
            {
                data = data
            });
        }
    }
}