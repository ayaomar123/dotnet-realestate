using AutoMapper;
using Realestate.DTOs.Auth;
using Realestate.DTOs.Category;
using Realestate.DTOs.District;
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
        }
    }
}
