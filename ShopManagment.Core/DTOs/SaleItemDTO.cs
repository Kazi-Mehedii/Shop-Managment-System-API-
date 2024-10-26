using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Core.DTOs
{
    public class SaleItemDTO
    {
   
        public int Id { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int TotalAmount { get; set; }
    }
}
