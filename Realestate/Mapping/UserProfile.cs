using AutoMapper;
using Realestate.DTOs.Auth;
using Realestate.DTOs.Category;
using Realestate.DTOs.District;
using Realestate.DTOs.Item;
using Realestate.DTOs.Status;
using Realestate.Entities;

namespace Realestate.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.Ignore()) 
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());


            CreateMap<BaseRequestDto, Category>().ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Category, BaseResponseDto>();


            CreateMap<BaseRequestDto, City>().ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<City, BaseResponseDto>();

            CreateMap<DistrictRequestDto, District>().ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<District, DistrictResponseDto>().ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City != null ? src.City.NameEn : null));

            CreateMap<BaseRequestDto, PropertyType>().ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<PropertyType, BaseResponseDto>();

            CreateMap<BaseRequestDto, MyType>().ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<MyType, BaseResponseDto>();

            CreateMap<StatusRequestDto, Status>().ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<Status, StatusResponseDto>();

            CreateMap<ItemRequestDto, Item>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<ItemUpdateRequestDto, Item>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.CityId, opt => opt.Ignore())
                .ForMember(dest => dest.DistrictId, opt => opt.Ignore())
                .ForMember(dest => dest.MyTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.PropertyTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.StatusId, opt => opt.Ignore())
                .ForMember(dest => dest.AdvertiseNo, opt => opt.Ignore())
                .ForMember(dest => dest.AdNo, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Item, ItemResponseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.NameEn : null))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City != null ? src.City.NameEn : null))
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District != null ? src.District.NameEn : null))
                .ForMember(dest => dest.MyTypeName, opt => opt.MapFrom(src => src.MyType != null ? src.MyType.NameEn : null))
                .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.NameEn : null))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status != null ? src.Status.NameEn : null))
                .ForMember(dest => dest.StatusColor, opt => opt.MapFrom(src => src.Status != null ? src.Status.Color : null))
                .ForMember(dest => dest.StatusBackgroundColor, opt => opt.MapFrom(src => src.Status != null ? src.Status.BackgroundColor : null))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.ImageUrl)));
            ;
        }
    }
}
