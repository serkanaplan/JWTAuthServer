using AutoMapper;
using JWTAuthServer.Core.DTOs;
using JWTAuthServer.Core.Models;

namespace JWTAuthServer.Service;
internal class DtoMapper : Profile
{
    public DtoMapper()
    {
        CreateMap<ProductDto, Product>().ReverseMap();
        CreateMap<UserAppDto, UserApp>().ReverseMap();
    }
}