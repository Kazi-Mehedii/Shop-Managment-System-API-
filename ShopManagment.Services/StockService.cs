using AutoMapper;
using ShopManagment.Core.DTOs;
using ShopManagment.Data.IGeneric_Repository_And_IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Services
{
    public class StockService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public StockService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<StockDTO>> GetAllStocks()
        {
            var stocks = await _unitOfWork.Stocks.GetAllWithIncludesAsync();
            return _mapper.Map<IEnumerable<StockDTO>>(stocks);
        }

        public async Task<StockDTO> GetStockByIdAsync(int id)
        {
            var stock = await _unitOfWork.Stocks.GetByIdWithIncludesAsync(id, s => s.Id);
            if (stock == null)
            {
                throw new Exception("Stock Not Found");
            }

            return _mapper.Map<StockDTO>(stock);
        }

        // Method to get all products with stock information
        public async Task<IEnumerable<ProductDTO>> ViewStockInProductModel()
        {
            // Fetch all products
            var products = await _unitOfWork.Products.GetAllAsync();

            // Fetch all stock entries
            var stocks = await _unitOfWork.Stocks.GetAllAsync();

            // Map the products to ProductDTO
            var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            // Assign stock quantities to the respective products
            foreach (var productDTOs in productDtos)
            {
                var stock = stocks.FirstOrDefault(s => s.ProductId == productDTOs.Id);

                if(stock != null)
                {
                    productDTOs.Stock = stock.Quantity;
                }
                else { productDTOs.Stock = 0; } // If no stock, set to 0

            }

            return productDtos;

        }

    }
}
