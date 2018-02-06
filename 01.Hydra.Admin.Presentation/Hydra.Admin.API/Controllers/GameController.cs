using Flurl;
using Flurl.Http;
using Hydra.Admin.API.Helper;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Hydra.Admin.API.Controllers
{
    /// <summary>
    /// 游戏HTTP接口操作
    /// </summary>
    [Produces("application/json")]
    [Route("api/Game")]
    public class GameController : BaseAPIController
    {
        #region Notice 游戏通知公告
        /// <summary>
        /// 获取游戏通知公告
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetNotice")]
        public async Task<IActionResult> GetNotice(BaseQuery query)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetNotice;
            var ret = await APIFuncOTAsync(query, "---获取通知公告---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParam("p", query.p)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 设置游戏通知公告
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("SetNotice")]
        [Log2DataBaseFilter("设置游戏通知公告")]
        public async Task<IActionResult> SetNotice([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.SetNotice;
            var ret = await APIFuncAsync(data, "---设置通知公告---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 删除游戏通知公告
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("RemNotice")]
        [Log2DataBaseFilter("删除游戏通知公告")]
        public async Task<IActionResult> RemNotice([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.RemNotice;
            var ret = await APIFuncAsync(data, "---删除通知公告---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 启/停游戏通知公告
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("EabNotice")]
        [Log2DataBaseFilter("启/停游戏通知公告")]
        public async Task<IActionResult> EabNotice([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.EabNotice;
            var ret = await APIFuncAsync(data, "---启/停游戏通知公告---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        #endregion

        #region Game 游戏配置及相关
        /// <summary>
        /// 获取游戏配置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetGameConfig")]
        public async Task<IActionResult> GetGameConfig(string gameid)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetGameConfig;
            var ret = await APIFuncOTAsync(gameid, "---获取游戏配置---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParam("gameid", d)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 设置游戏配置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("SetGameConfig")]
        [Log2DataBaseFilter("设置游戏配置")]
        public async Task<IActionResult> SetGameConfig([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.SetGameConfig;
            var ret = await APIFuncAsync(data, "---设置游戏配置---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        #endregion

        #region Task 游戏任务配置
        /// <summary>
        /// 获取任务配置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetTaskConfig")]
        public async Task<IActionResult> GetTaskConfig(BaseQuery query)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetTaskConfig;
            var ret = await APIFuncOTAsync(query, "---获取任务配置---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParam("p", query.p)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 设置任务配置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("SetTaskConfig")]
        [Log2DataBaseFilter("设置任务配置")]
        public async Task<IActionResult> SetTaskConfig([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.SetTaskConfig;
            var ret = await APIFuncAsync(data, "---设置任务配置---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 删除任务配置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("RemTaskConfig")]
        [Log2DataBaseFilter("删除任务配置")]
        public async Task<IActionResult> RemTaskConfig([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.RemTaskConfig;
            var ret = await APIFuncAsync(data, "---删除任务配置---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 启/停任务配置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("EabTaskConfig")]
        [Log2DataBaseFilter("启/停任务配置")]
        public async Task<IActionResult> EabTaskConfig([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.EabTaskConfig;
            var ret = await APIFuncAsync(data, "---启/停任务配置---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 获取任务配置（单条）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("SglTaskConfig")]
        public async Task<IActionResult> SglTaskConfig(string id)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.SgnTaskConfig;
            var ret = await APIFuncOTAsync(id, "---获取任务配置（单条）---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParam("id", d)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        #endregion

        #region BUG 配置相关
        /// <summary>
        /// 获取意见建议
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetBugs")]
        public async Task<IActionResult> GetBugs(BaseQuery query)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetBugs;
            var ret = await APIFuncOTAsync(query, "---获取意见建议---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParams(query)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 采纳建议
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("SetBug")]
        [Log2DataBaseFilter("采纳建议")]
        public async Task<IActionResult> SetBug([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.SetBug;
            var ret = await APIFuncAsync(data, "---采纳建议---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 删除意见建议
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("RemBug")]
        [Log2DataBaseFilter("删除意见建议")]
        public async Task<IActionResult> RemBug([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.RemBug;
            var ret = await APIFuncAsync(data, "---删除意见建议---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        #endregion

        #region Email 邮件相关
        /// <summary>
        /// 获取邮件列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetMails")]
        public async Task<IActionResult> GetMails(BaseQuery query)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetMails;
            var ret = await APIFuncOTAsync(query, "---获取邮件列表---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParams(query)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("SendMail")]
        [Log2DataBaseFilter("发送邮件")]
        public async Task<IActionResult> SendMail([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.SendMail;
            var ret = await APIFuncAsync(data, "---发送邮件---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        #endregion

        #region Single Config 系统配置
        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetSingleConfig")]
        public async Task<IActionResult> GetSingleConfig(string _id)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetSingleConfig;
            var ret = await APIFuncOTAsync(_id, "---获取系统配置---", async (d, p) =>
             {
                 var res = new AjaxResult();
                 switch (d + "")
                 {
                     case "fortune":
                         res.data = await api
                     .SetQueryParam("_id", d)
                     .GetJsonListAsync();
                         break;
                     default:
                         res.data = await api
                     .SetQueryParam("_id", d)
                     .GetJsonAsync();
                         break;
                 }
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 设置系统配置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("SetSingleConfig")]
        [Log2DataBaseFilter("设置系统配置")]
        public async Task<IActionResult> SetSingleConfig([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.SetSingleConfig;
            var ret = await APIFuncAsync(data, "---设置系统配置---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 手动创建机器人
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("CreateRobot")]
        [Log2DataBaseFilter("手动创建机器人")]
        public async Task<IActionResult> CreateRobot([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.CreateRobot;
            var ret = await APIFuncAsync(data, "---手动创建机器人---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        #endregion

        #region Player 游戏玩家
        /// <summary>
        /// 获取玩家列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetPlayers")]
        public async Task<IActionResult> GetPlayers(PlayerQuery query)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetPlayers;
            var ret = await APIFuncOTAsync(query, "---获取玩家列表---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParams(query)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 获取玩家相关信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetPlayerRelated")]
        public async Task<IActionResult> GetPlayerRelated(PlayerRelatedQuery query)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetPlayerRelated;
            var ret = await APIFuncOTAsync(query, "---获取玩家相关信息---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParams(query)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 获取玩家金币排行
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetPlayerGoldRank")]
        public async Task<IActionResult> GetPlayerGoldRank(PlayerQuery query)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetPlayerGoldRank;
            var ret = await APIFuncOTAsync(query, "---获取玩家金币排行---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParams(query)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 获取单个玩家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetPlayer")]
        public async Task<IActionResult> GetPlayer(PlayerQuery query)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetPlayer;
            var ret = await APIFuncOTAsync(query, "---获取单个玩家---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParams(query)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 玩家上下分/VIP等级修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("SetPlayerGold")]
        [Log2DataBaseFilter("玩家上下分/VIP等级修改")]
        public async Task<IActionResult> SetPlayerGold([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.SetPlayerGold;
            var ret = await APIFuncAsync(data, "---玩家上下分/VIP等级修改---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 解/锁玩家
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("SetPlayerFreeze")]
        [Log2DataBaseFilter("解/锁玩家")]
        public async Task<IActionResult> SetPlayerFreeze([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.SetPlayerFreeze;
            var ret = await APIFuncAsync(data, "---解/锁玩家---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 获取抽奖记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetDialRecord")]
        public async Task<IActionResult> GetDialRecord(BaseQuery query)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetDialRecord;
            var ret = await APIFuncOTAsync(query, "---获取抽奖记录---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParams(query)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        #endregion

        #region Spark 游戏控制
        /// <summary>
        /// 获取Spark控制
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetSpark")]
        public async Task<IActionResult> GetSpark(string gameId)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetSpark;
            var ret = await APIFuncOTAsync(gameId, "---获取Spark控制---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParam("gameId", d)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 配置Spark
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("SetSpark")]
        [Log2DataBaseFilter("配置Spark")]
        public async Task<IActionResult> SetSpark([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.SetSpark;
            var ret = await APIFuncAsync(data, "---配置Spark---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        #endregion

        #region Product 充值商品
        /// <summary>
        /// 获取充值商品
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        public async Task<IActionResult> GetProduct(BaseQuery query)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetProduct;
            var ret = await APIFuncOTAsync(query, "---获取充值商品---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParams(d)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 设置充值商品
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("SetProduct")]
        [Log2DataBaseFilter("设置充值商品")]
        public async Task<IActionResult> SetProduct([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.SetProduct;
            var ret = await APIFuncAsync(data, "---设置充值商品---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 删除充值商品
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("RemProduct")]
        [Log2DataBaseFilter("删除充值商品")]
        public async Task<IActionResult> RemProduct([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.RemProduct;
            var ret = await APIFuncAsync(data, "---删除充值商品---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        #endregion

        #region PlayerOrder 提现/充值
        /// <summary>
        /// 获取玩家订单(提现/充值)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetPlayerOrder")]
        public async Task<IActionResult> GetPlayerOrder(PlayerOrderQurey query)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.GetPlayerOrder;
            var ret = await APIFuncOTAsync(query, "---获取玩家订单(提现/充值)---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .SetQueryParams(query)
                     .GetAsync()
                     .ReceiveJson()
                 };
                 return res;
             });
            return Json(ret);
        }
        /// <summary>
        /// 处理玩家提现申请
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("HandleCashRecord")]
        [Log2DataBaseFilter("处理玩家提现申请")]
        public async Task<IActionResult> HandleCashRecord([FromBody] JObject data)
        {
            var api = DataAccess.DbConfig.GameAPI + GameUrl.HandleCashRecord;
            var ret = await APIFuncAsync(data, "---处理玩家提现申请---", async (d, p) =>
             {
                 var res = new AjaxResult
                 {
                     data = await api
                     .PostJsonAsync(d)
                     .ReceiveString()
                 };
                 return res;
             });
            return Json(ret);
        }
        #endregion
    }
}