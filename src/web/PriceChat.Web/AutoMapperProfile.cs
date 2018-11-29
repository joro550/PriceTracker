using AutoMapper;
using PriceChat.Web.Models.Items;

namespace PriceChat.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Data.Item, Models.Item>();
            CreateMap<ItemModel, Data.Item>();
            CreateMap<Data.ItemPrice, Models.ItemPrice>();
        }
    }
}
