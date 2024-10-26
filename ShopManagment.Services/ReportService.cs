using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopManagment.Core.DTOs;
using ShopManagment.Core.Model;
using ShopManagment.Data.ContextClass;
using ShopManagment.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Services
{
    public class ReportService : IReportInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReportService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Report> GenarateReport(string type)
        {
            var salse = _context.Sale.Include(s => s.SaleItems).ThenInclude(si => si.Product).ToList();
            var purchase = _context.Purchase.Include(p => p.Product).ToList();

            var reportData = from sale in salse
                             from saleItem in sale.SaleItems
                             group saleItem by new { sale.SaleDate, saleItem.ProductId } into saleGroup


                             let product = saleGroup.First().Product
                             let totalSalse = saleGroup.Sum(s => s.TotalAmount)
                             let totalQuantitySold = saleGroup.Sum(s => s.Quantity)

                             let purchaseGroup = purchase.Where(p => p.ProductId == product.Id)
                             let totalPurchase = purchaseGroup.Sum(p => p.TotalPrice)
                             let totalQuantityPurchased = purchaseGroup.Sum(p => p.Quantity)
                             select new Report
                             {
                                 Date = saleGroup.Key.SaleDate,  // Get the SaleDate from the grouping key
                                 ProductName = product.ProductName,
                                 QuantitySold = totalQuantitySold,
                                 QuantityPurchased=totalQuantityPurchased,
                                 TotalSalse =  totalSalse,
                                 TotalPurchases = totalPurchase,
                                 Revenue = totalSalse - totalPurchase,
                             };

            // Filter by type (daily, monthly, yearly)

            if (type == "daily")
            {
                reportData = reportData.Where(r => r.Date.Date == DateTime.Now.Date);
            }

            else if (type == "monthly")
            {
                reportData = reportData.Where(r => r.Date.Month == DateTime.Now.Month && r.Date.Year == DateTime.Now.Year);
            }
            else if (type == "yearly")
            {
                reportData = reportData.Where(r => r.Date.Year == DateTime.Now.Year);
            }

            // Use AutoMapper to map to ReportDTO
            //var reportDtos = _mapper.Map<List<ReportDTOs>>(reportData.ToList());*/

            return reportData.ToList();
        }


    }
}
