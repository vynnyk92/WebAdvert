using AutoMapper;
using WebAdvert.Api.DTOs;
using WebAdvert.Models;

namespace WebAdvert.Api.Mapping
{
    public class AdvertProfile : Profile
    {
        public AdvertProfile()
        {
            CreateMap<AdvertModel, AdvertDto>();
        }
    }
}
