namespace ProductShop
{
    using AutoMapper;
    using ProductShop.Models;
    using ProductShop.Dtos.Import;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUserDto, User>();

            this.CreateMap<ImportProductDto, Product>();

            this.CreateMap<ImportCategoryDto, Category>();

            this.CreateMap<ImportCategoryProductsDto, CategoryProduct>();
        }
    }
}
