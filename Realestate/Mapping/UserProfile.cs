using AutoMapper;
using Realestate.DTOs.Auth;
using Realestate.Entities;

namespace Realestate.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.Ignore()) // حنعينه يدوي
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // لأنه مخصص
        }
    }
}
