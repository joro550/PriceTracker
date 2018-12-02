using AutoMapper;
using PriceChat.Web.Models.Home;
using PriceChat.Web.Models.Items;

namespace PriceChat.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Data.Item, Item>();
            CreateMap<Data.ItemPrice, ItemPrice>();

            CreateMap<ItemModel, Data.Item>()
                .ForMember(model => model.RowKey, opts => opts.MapFrom(src => src.Id));
        }
    }
}
