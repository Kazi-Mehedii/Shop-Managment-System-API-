using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Core.Model
{
    public class Report
    {
        public DateTime Date { get; set; }

        public string ProductName { get; set; }

        public int QuantitySold { get; set; }

        public int QuantityPurchased { get; set; }

        public decimal TotalSalse { get; set; }

        public decimal TotalPurchases { get; set; }

        public decimal Revenue { get; set; }
    }
}
