using AutoMapper;
using PriceChat.Web.Data;
using PriceChat.Web.Models.Home;
using PriceChat.Web.Models.Items;

namespace PriceChat.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ItemEntity, Item>();
            CreateMap<AddItemModel,ItemEntity>()
                .ForMember(model => model.RowKey, opts => opts.MapFrom(src => src.Id));
        }
    }
}
