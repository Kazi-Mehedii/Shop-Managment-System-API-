using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopManagment.Core.DTOs;
using ShopManagment.Core.Model;
using ShopManagment.Data.IGeneric_Repository_And_IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Services
{
    public class SaleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SaleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // Get all sales
        public async Task<IEnumerable<SaleDTO>> GettAllSale()
        {
            var saleWithProduct = await _unitOfWork.Sales.GetAllWithIncludesAsync(p => p.SaleItems);
            return _mapper.Map<IEnumerable<SaleDTO>>(saleWithProduct);
        }

        public async Task<SaleDTO> GetSaleById(int id)
        {
            var sale = await _unitOfWork.Sales.GetByIdWithIncludesAsync(id, s => s.SaleItems);
            if (sale == null)
            {
                throw new Exception("Sale Not Found");
            }

            return _mapper.Map<SaleDTO>(sale);
        }

        public async Task<SaleDTO> CreateSale(SaleDTO saleDTO)
        {
            var sale = _mapper.Map<Sale>(saleDTO);

            foreach (var saltemDTO in saleDTO.SaleItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(saltemDTO.ProductId);
                var stock = await _unitOfWork.Stocks.GetSingleOrDefaultAsync(st => st.ProductId == saltemDTO.ProductId);


                if (product == null)
                {
                    throw new Exception($"Product With Id{saltemDTO.ProductId} Not Found");
                }

                if (stock == null)
                {
                    throw new Exception($"Stock for product ID {saltemDTO.ProductId} not found");
                }

                if (stock.Quantity < saltemDTO.Quantity)
                {
                    throw new Exception($"Insufficient stock for product {product.ProductName}. Available stock: {stock.Quantity}");
                }



                // If stock exists, decrease the quantity
                stock.Quantity -= saltemDTO.Quantity;
                _unitOfWork.Stocks.Update(stock);

                // Check if the sale item already exists
                var existingSaleItem = sale.SaleItems.FirstOrDefault(si => si.ProductId == saltemDTO.ProductId);

                if (existingSaleItem != null)
                {
                    // Update the existing sale item
                    existingSaleItem.Quantity = saltemDTO.Quantity; // Update quantity
                    existingSaleItem.Price = saltemDTO.Price;
                    existingSaleItem.TotalAmount = existingSaleItem.Quantity * existingSaleItem.Price; // Update total amount
                }
                else
                {
                    // Create a new SaleItem and calculate the total amount
                    var newSaleItem = _mapper.Map<SaleItem>(saltemDTO);
                    newSaleItem.TotalAmount = newSaleItem.Quantity * newSaleItem.Price;

                    // Add the sale item to the sale object
                    sale.SaleItems.Add(newSaleItem);
                }

            }



            await _unitOfWork.Sales.AddAsync(sale);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SaleDTO>(sale);
        }


        public async Task UpdateSale(SaleDTO saleDTO)
        {

            var sale = await _unitOfWork.Sales.GetByIdWithIncludesAsync(saleDTO.Id, s => s.SaleItems);

            if (sale == null)
            {
                throw new Exception("Sale Not Found");    
            }

            //// Loop through the sale items and update stock and sale details
            foreach (var saltemDTO in saleDTO.SaleItems)
            {
                var products = await _unitOfWork.Products.GetByIdAsync(saltemDTO.ProductId);
                var stocks = await _unitOfWork.Stocks.GetSingleOrDefaultAsync(st => st.ProductId == saltemDTO.ProductId);

                if (products == null)
                {
                    throw new Exception($"Product With Id{saltemDTO.ProductId} Not Found");
                }

                //adjust stock
                var existingItem = sale.SaleItems.FirstOrDefault(u => u.ProductId == saltemDTO.ProductId);

                if (existingItem != null)
                {
                    // Restore previous stock quantity
                    stocks.Quantity += existingItem.Quantity;

                    // // Deduct the new quantity
                    stocks.Quantity -= saltemDTO.Quantity;

                }
                else
                {
                    throw new Exception($"Sale Item with Product ID {saltemDTO.ProductId} not found");
                }

                _unitOfWork.Stocks.Update(stocks);

                // Update the sale item details

                existingItem.Quantity =saltemDTO.Quantity;
                existingItem.Price = saltemDTO.Price;
                existingItem.TotalAmount = saltemDTO.Quantity * saltemDTO.Price;

            }

            _unitOfWork.Sales.Update(sale);       
            await _unitOfWork.SaveAsync();
        }


        public async Task DeleteSale(int id)
        {
          var salse = await _unitOfWork.Sales.GetByIdWithIncludesAsync(id, s => s.SaleItems);

            if (salse == null)
            {
                throw new Exception("Sale Not Found");
            }

            //Restore stock quantities before deleting the sale
            foreach (var saleItem in salse.SaleItems)
            {
                var stock  = await _unitOfWork.Stocks.GetSingleOrDefaultAsync(st => st.ProductId == saleItem.ProductId);
                if (stock != null)
                {
                    stock.Quantity += saleItem.Quantity;
                    _unitOfWork.Stocks.Update(stock);
                }
            }

            await _unitOfWork.Sales.DeleteAsync(id);

            await _unitOfWork.SaveAsync();
        }

    }
}
