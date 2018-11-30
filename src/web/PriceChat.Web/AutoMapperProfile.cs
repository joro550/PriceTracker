using System;
using AutoMapper;
using PriceChat.Web.Models.Items;

namespace PriceChat.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Data.Item, Models.Item>();
            CreateMap<Data.ItemPrice, Models.ItemPrice>();

            CreateMap<ItemModel, Data.Item>()
                .ForMember(model => model.RowKey, opts => opts.MapFrom(src => src.Id));
        }
    }
}
