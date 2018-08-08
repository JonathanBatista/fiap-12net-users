using AutoMapper;
using GeekBurger.Users.Contract;
using GeekBurger.Users.Core.Domains;

namespace GeekBurger.Users.Mapping.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserProcess>()
                .ForMember(dest => dest.Processing, opt => opt.MapFrom(src => src.InProcessing))
                .ForMember(dest => dest.UserGuid, opt => opt.MapFrom(src => src.AzureGuid))
                .ReverseMap();
        }
    }
}
