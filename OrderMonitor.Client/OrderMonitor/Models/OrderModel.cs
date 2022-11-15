using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMonitor.Models
{
    public class OrderInfo
	{
        public string OrderId { get; set; }
        public string Symbol { get; set; }
        public string Side { get; set; }
        public string Client { get; set; }
		public double LivePrice { get; set; }
		public double Qty { get; set; }
		public double Total { get; set; }
	}
}
