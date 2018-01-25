using Hydra.Admin.Utility.iViewControl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.View
{
    /// <summary>
    /// DashBoard
    /// </summary>
    public class DashBoardView
    {
        /// <summary>
        /// DashBoard Item
        /// </summary>
        public List<decimal> DashBoardItem { get; set; }
        /// <summary>
        /// x 轴
        /// </summary>
        public IEnumerable<string> xAxisData { get; set; }
        /// <summary>
        /// y 轴
        /// </summary>
        public Dictionary<string, IEnumerable<decimal>> yAxisData { get; set; }
        /// <summary>
        /// table 
        /// </summary>
        public IViewTable<Model.AnalysisDashboard> Table { get; set; }
    }
}
