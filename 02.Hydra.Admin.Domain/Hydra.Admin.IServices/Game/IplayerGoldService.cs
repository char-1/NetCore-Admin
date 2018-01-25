using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility.iViewControl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Admin.IServices
{
    public interface IplayerGoldService : IBaseService<playerGold>
    {
        /// <summary>
        /// 玩家金币明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IViewTable<playerGold> GetPlayerGoldGrid(PlayerGoldQuery query);
        /// <summary>
        /// 玩家金币明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IViewTable<playerGold>> GetPlayerGoldGridAsync(PlayerGoldQuery query);
        /// <summary>
        /// 玩家金币明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IViewTableSlotFooter<playerGold, dynamic>> GetPlayerGoldGridFootAsync(PlayerGoldQuery query);
    }
}
