using AutoMapper;
using GeekBurger.Users.Contract.Dtos.RequestDto;
using GeekBurger.Users.Contract.Dtos.ResponseDto;
using GeekBurger.Users.Core.Domains;

namespace GeekBurger.Users.Contract.Mapping.Profiles
{
    public class UserProcessProfile : Profile
    {
        public UserProcessProfile()
        {
            CreateMap<UserRequestDto, User>()
                .ForMember(dest => dest.FaceBase64, opt => opt.MapFrom(src => src.Face))
                .ReverseMap();

            CreateMap<User, UserProcessDto>()
                .ForMember(dest => dest.UserGuid, opt => opt.MapFrom(src => src.AzureGuid))
                .ReverseMap();
        }
    }
}
