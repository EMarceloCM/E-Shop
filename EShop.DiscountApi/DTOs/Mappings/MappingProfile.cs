using AutoMapper;
using EShop.DiscountApi.Models;

namespace EShop.DiscountApi.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CouponDTO, Coupon>().ReverseMap();
        }
    }
}
