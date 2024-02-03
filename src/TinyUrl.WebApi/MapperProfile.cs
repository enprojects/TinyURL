

using AutoMapper;
using TinyUrl.Backend.Models;
using TinyUrl.WebApi.ViewModel;

namespace TinyUrl.WebApi
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TinyUrlModel, TinyUrlDb>()
                //.ForMember(dest => dest.CreatedAt, act => act.Ignore())
                .ReverseMap();
            CreateMap<TinyUrlResponse, TinyUrlResponseViewModel>();
            CreateMap<TinyUrlRequestViewModel, TinyUrlRequest>();
        }
    }
}