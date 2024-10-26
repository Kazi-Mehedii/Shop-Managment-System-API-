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
    public class PurchaseService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public PurchaseService( IUnitOfWork unitOfWork, IMapper mapper )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PurchaseDTO>> GetAllPurchase()
        {
            var purchaseQuery = await _unitOfWork.Purchases.GetAllWithIncludesAsync(p => p.Product, s => s.Supplier);

            var purchaseList = await purchaseQuery.ToListAsync();

            return _mapper.Map<IEnumerable<PurchaseDTO>>(purchaseList);
        }

        public async Task<PurchaseDTO> GetPurchaseById(int id)
        {
            var purchase = await _unitOfWork.Purchases.GetByIdAsync( id );
            return _mapper.Map<PurchaseDTO>( purchase );
        }

        public async Task<PurchaseDTO> CreatePurchase(PurchaseDTO purchaseDTO)
        {
            // Retrieve the product and supplier based on IDs from PurchaseDTO
            var product = await _unitOfWork.Products.GetByIdAsync(purchaseDTO.ProductId);
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(purchaseDTO.SupplierId);
            //var stock = await _unitOfWork.Stocks.GetByIdAsync(purchaseDTO.ProductId );

            if (product == null)
            {
                throw new Exception("Product Not Found");
            }

            if (supplier == null)
            {
                throw new Exception("Supplier Not Found");
            }

            var stock = await _unitOfWork.Stocks.GetSingleOrDefaultAsync(st => st.ProductId == purchaseDTO.ProductId);
            if (stock != null)
            {
                //if stock exists, increase the quantity
                stock.Quantity += purchaseDTO.Quantity;
                _unitOfWork.Stocks.Update(stock);
            }
            else
            {
                //if stock doesnot exists, create new  stocks
                var newStock = new Stock
                {
                    ProductId = purchaseDTO.ProductId,
                    Quantity = purchaseDTO.Quantity,
                };
                await _unitOfWork.Stocks.AddAsync(newStock);
            }

            // Map the DTO to the Purchase entity and calculate total price
            var purchase = _mapper.Map<Purchase>(purchaseDTO);

            
            purchase.TotalPrice = purchase.Quantity * purchase.UnitPrice;

            // Associate the existing Product and Supplier entities with the Purchase
            purchase.Product = product;
            purchase.Supplier = supplier;

            //add the purchase record to the database
            await _unitOfWork.Purchases.AddAsync(purchase);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<PurchaseDTO>(purchase);
        }


        public async Task UpdatePurchase(PurchaseDTO purchaseDTO)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(purchaseDTO.ProductId);

            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(purchaseDTO.SupplierId);

            if (product == null)
            {
                throw new Exception("Product Not Found");
            }

            if (supplier == null)
            {
                throw new Exception("Supplier Not Found");
            }


            // Retrieve the stock associated with the product
            var stock = await _unitOfWork.Stocks.GetSingleOrDefaultAsync(st => st.ProductId == purchaseDTO.ProductId);
            if (stock == null)
            {
                throw new Exception("Stock for product not found");
            }

            var orginalPurchase = await _unitOfWork.Purchases.GetByIdAsync(purchaseDTO.Id);
            if (orginalPurchase == null)
            {
                throw new Exception("Purchase not found");
            }

            // Adjust the stock quantity before updating
            // First, restore the original purchase quantity to the stock
            stock.Quantity -= orginalPurchase.Quantity; // Undo the original purchase quantity


            // Now, apply the new purchase quantity
            stock.Quantity += purchaseDTO.Quantity; // Add the new purchase quantity

            if (stock.Quantity < 0)
            {
                throw new Exception("Stock quantity cannot be negative.");
            }

            // Update the stock in the database
            _unitOfWork.Stocks.Update(stock);

            // Map the updated purchaseDTO to the original purchase entity
            _mapper.Map(purchaseDTO, orginalPurchase);

            // Explicitly set TotalPrice again
            orginalPurchase.TotalPrice = orginalPurchase.Quantity * orginalPurchase.UnitPrice;

            _unitOfWork.Purchases.Update(orginalPurchase);
           await _unitOfWork.SaveAsync();


            // Retrieve the product and supplier and stock based on IDs from PurchaseDTO
           
            //var stock = await _unitOfWork.Stocks.GetByIdAsync(purchaseDTO.ProductId);

            

            //if (stock != null)
            //{
            //    //if stock exists, increase the quantity
            //    stock.Quantity += purchaseDTO.Quantity;
            //    _unitOfWork.Stocks.Update(stock);
            //}
            //else
            //{
            //    //if stock doesnot exists, create new  stocks
            //    var newStock = new Stock
            //    {
            //        ProductId = purchaseDTO.ProductId,
            //        Quantity = purchaseDTO.Quantity,
            //    };
            //    await _unitOfWork.Stocks.AddAsync(newStock);
            //}

            //Map the purchaseDto to the purchase entity
            //var purchase = _mapper.Map<Purchase>(purchaseDTO);

            // Explicitly set TotalPrice
            //purchase.TotalPrice = purchase.Quantity * purchase.UnitPrice;

            // Associate the existing Product and Supplier entities with the Purchase
            //purchase.Product = product;
            //purchase.Supplier = supplier;

        }

        public async Task DeletePurchase(int id)
        {
            var purchase = await _unitOfWork.Purchases.GetByIdWithIncludesAsync(id, p => p.Product, s => s.Supplier);
            if (purchase == null) { throw new Exception("Purchase Not Found"); }

            // Adjust stock by reducing the quantity of the purchase being deleted
            var stock = await _unitOfWork.Stocks.GetSingleOrDefaultAsync(st => st.ProductId == purchase.ProductId);
            if (stock != null)
            {
                stock.Quantity -= purchase.Quantity;
                _unitOfWork.Stocks.Update(stock);
            }

            await _unitOfWork.Purchases.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }


    }
}
