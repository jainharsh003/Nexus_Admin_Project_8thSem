using AutoMapper;
using LoginSignup.Models;
using LoginSignup.DTOs;

namespace LoginSignup.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Map User entity ↔ DTOs
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
        }
    }
}
