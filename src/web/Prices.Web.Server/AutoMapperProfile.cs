using System;
using AutoMapper;
using Prices.Web.Server.Handlers.Data.Entities;
using Prices.Web.Server.Handlers.Requests;
using Prices.Web.Shared.Models.Home;
using Prices.Web.Shared.Models.Items;

namespace Prices.Web.Server
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ItemEntity, Item>();
            CreateMap<AddItemModel, ItemEntity>()
                .ForMember(model => model.RowKey, opts => opts.MapFrom(src => src.Id));

            CreateMap<CreateUserRequest, UserEntity>()
                .AfterMap((createRequest, userEntity) =>
                {
                    var partitionKey = Guid.NewGuid().ToString("N");

                    userEntity.Id = partitionKey;
                    userEntity.PartitionKey = partitionKey;
                    userEntity.RowKey = Guid.NewGuid().ToString("N");
                });
        }
    }
}