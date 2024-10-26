using AutoMapper;
using ShopManagment.Core.DTOs;
using ShopManagment.Core.Model;

namespace ShopManagment.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Purchase, PurchaseDTO>().ReverseMap();
            CreateMap<Sale, SaleDTO>().ReverseMap();
            CreateMap<Stock, StockDTO>().ReverseMap();
            CreateMap<Supplier, SupplierDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<SaleItem, SaleItemDTO>().ReverseMap();
            //CreateMap<Report, ReportDTOs>().ReverseMap();
        }
    }
}
