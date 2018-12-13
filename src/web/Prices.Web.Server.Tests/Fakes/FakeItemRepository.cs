﻿using System.Collections.Generic;
using Prices.Web.Server.Data;
using Prices.Web.Server.Data.Entities;

namespace Prices.Web.Server.Tests.Fakes
{
    public class FakeItemRepository : InMemoryRepository<ItemEntity>, IItemRepository
    {
        public static readonly List<ItemEntity> StandardItems = new List<ItemEntity>
        {
            new ItemEntity
            {
                Id = "1",
                Category = "Category",
                Retailer = "Retailer"
            }
        };

        private FakeItemRepository(List<ItemEntity> items)
            : base(items)
        {
        }

        public static FakeItemRepository WithItems(List<ItemEntity> items) 
            => new FakeItemRepository(items);

        public static FakeItemRepository WithStandardItems() 
            => new FakeItemRepository(StandardItems);

        public static FakeItemRepository WithNoItems() 
            => new FakeItemRepository(new List<ItemEntity>());
    }
}