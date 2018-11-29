using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace PriceChat.Web.Models.Items
{
    public class ItemModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Retailer { get; set; }
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