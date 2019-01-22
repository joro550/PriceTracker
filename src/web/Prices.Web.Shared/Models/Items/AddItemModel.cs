﻿using System.Collections.Generic;

namespace Prices.Web.Shared.Models.Items
{
    public class AddItemModel
    {
        public string PartitionKey => "Product";
        public string Id { get; set; }
        public string Category { get; set; }
        public string Retailer { get; set; }

        public List<SelectListItem> RetailerList => new List<SelectListItem>
        {
            new SelectListItem {Value = "Amazon", Text = "Amazon"},
            new SelectListItem {Value = "Argos", Text = "Argos"}
        };
    }

    public class SelectListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}