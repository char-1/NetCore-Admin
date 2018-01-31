using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Utility.eChartControl
{
    /// <summary>
    /// 
    /// </summary>
    public class EChartItem
    {
        public EChartItem()
        {
            this.legend = new HashSet<string>();
            this.xAxis = new HashSet<string>();
            this.series = new HashSet<SeriesItem>();
        }
        /// <summary>
        /// legend
        /// </summary>
        public IEnumerable<string> legend { get; set; }
        /// <summary>
        /// X轴
        /// </summary>
        public IEnumerable<string> xAxis { get; set; }
        /// <summary>
        /// series
        /// </summary>
        public IEnumerable<SeriesItem> series { get; set; }

    }
    /// <summary>
    /// Series 集合
    /// </summary>
    public class SeriesItem
    {
        public SeriesItem()
        {
            this.type = "line";
            this.data = new HashSet<decimal>();
            this.smooth = true;
        }
        public string type { get; set; }
        public string name { get; set; }
        public bool smooth { get; set; }
        public ItemStyle itemStyle { get; set; }
        /// <summary>
        /// Y 轴
        /// </summary>
        public IEnumerable<decimal> data { get; set; }
    }
    /// <summary>
    /// Series Style
    /// </summary>
    public class ItemStyle
    {
        public Normal normal { get; set; }
    }

    public class Normal
    {
        public Normal()
        {
            this.color = "#2b83f9";
        }
        public string color { get; set; }
        public AreaStyle areaStyle { get; set; }
    }
    public class AreaStyle
    {
        public AreaStyle()
        {
            this.color = "#E8F5FD";
        }
        public string color { get; set; }
    }
}
