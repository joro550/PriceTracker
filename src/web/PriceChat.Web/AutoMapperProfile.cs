using AutoMapper;

namespace PriceChat.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Data.Item, Models.Item>();
            CreateMap<Data.ItemPrice, Models.ItemPrice>();
        }
    }
}
