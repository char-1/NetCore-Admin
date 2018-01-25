using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.Model.Game
{
    /// <summary>
    /// 在线玩家
    /// </summary>
    public class playerOnline
    {
        /// <summary>
        /// 时间(201701231405)  YYYYMMDDHHMM
        /// </summary>
        public long Id { get; set; }
        public int OLPlayer { get; set; }
        /// <summary>
        /// 年月日
        /// </summary>
        public int YMD { get; set; }
        /// <summary>
        /// 时分
        /// </summary>
        public int HM { get; set; }
    }
}
