using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PriceChat.Web.Models.Items
{
    public class ItemModel
    {
        public string PartitionKey => "Product";

        [Required]
        public string Id { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Retailer { get; set; }

        public List<SelectListItem> RetailerList => new List<SelectListItem>
        {
            new SelectListItem {Value = "Amazon", Text = "Amazon"}
        };
    }

    public class ItemModelValidator : AbstractValidator<ItemModel>
    {
        public ItemModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.Category).NotNull().NotEmpty();
            RuleFor(x => x.Retailer).NotNull().NotEmpty();
        }
    }
}