using AutoMapper;
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
    public class SupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SupplierDTO>> GetAllSupplier()
        {
            var supplier = await _unitOfWork.Suppliers.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierDTO>>(supplier);
        }

        public async Task<SupplierDTO> GetByIDSupplier(int id)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
            return _mapper.Map<SupplierDTO>(supplier);
        }

        public async Task AddSupplier(SupplierDTO supplierDTO)
        {
            var supplier = _mapper.Map<Supplier>(supplierDTO);
            await _unitOfWork.Suppliers.AddAsync(supplier);
            await _unitOfWork.SaveAsync();

        }

        public async Task UpdateSupplier(SupplierDTO supplierDTO)
        {
            var supplier = _mapper.Map<Supplier>(supplierDTO);
            _unitOfWork.Suppliers.Update(supplier);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteSupplier(int id)
        {
            await _unitOfWork.Suppliers.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
