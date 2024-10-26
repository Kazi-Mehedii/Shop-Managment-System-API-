using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Core.Model
{
    public class Sale
    {
        public int Id { get; set; }


        public DateTime SaleDate { get; set; }

        public string? SaleInvoiceNo { get; set; }

       public List <SaleItem> SaleItems { get; set; } = new List<SaleItem>(); // Multiple products in one sale

    }
}

