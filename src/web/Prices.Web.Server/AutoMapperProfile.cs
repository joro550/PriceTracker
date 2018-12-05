using AutoMapper;
using Prices.Web.Server.Data;
using Prices.Web.Shared.Models.Home;
using Prices.Web.Shared.Models.Items;

namespace Prices.Web.Server
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
