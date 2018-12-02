using AutoMapper;
using PriceChat.Web.Models.Home;
using ItemModel = PriceChat.Web.Models.Items.ItemModel;

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
