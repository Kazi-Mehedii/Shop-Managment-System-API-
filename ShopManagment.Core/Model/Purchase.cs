using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Core.Model
{
    public class Purchase
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int SupplierId { get; set; } 

        public int Quantity { get; set; }

        public int UnitPrice { get; set; }

        public int TotalPrice
        {
            get; set;
        }

        public DateTime PurchaseDate { get; set; }

        public string? PurcaseMemoNo { get; set; }

        public virtual Product Product { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
