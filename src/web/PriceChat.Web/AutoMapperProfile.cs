using AutoMapper;
using PriceChat.Web.Data;
using PriceChat.Web.Models.Home;

namespace PriceChat.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ItemEntity, Item>();
            CreateMap<ItemModel, ItemEntity>()
                .ForMember(model => model.RowKey, opts => opts.MapFrom(src => src.Id));
        }
    }
}
