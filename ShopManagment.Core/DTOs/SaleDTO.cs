using ShopManagment.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Core.DTOs
{
    public class SaleDTO
    {
        public int Id { get; set; }


        public DateTime SaleDate { get; set; }

        public string? SaleInvoiceNo { get; set; }

        public List<SaleItemDTO> SaleItems { get; set; } = new List<SaleItemDTO>();

    }
}
