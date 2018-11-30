using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Results;
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

        public IList<ValidationFailure> Errors { get; set; } = new List<ValidationFailure>();

        public List<SelectListItem> RetailerList => new List<SelectListItem>
        {
            new SelectListItem {Value = "Amazon", Text = "Amazon"}
        };
    }

    public class ItemModelValidator : AbstractValidator<ItemModel>
    {
        private readonly List<string> _knownRetailers = new List<string> {"Amazon"};

        public ItemModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Item Id is required");
            RuleFor(x => x.Category).NotNull().NotEmpty().WithMessage("Item Category is required");
            RuleFor(x => x.Retailer).NotNull().NotEmpty().WithMessage("Item Retailer is required");
            RuleFor(x => x.Retailer).Must(BeAKnownRetailer).WithMessage("Please specify a known retailer");
        }

        private bool BeAKnownRetailer(string arg) 
            => _knownRetailers.Contains(arg);
    }
}