using FluentValidation;
using System.Collections.Generic;

namespace Prices.Web.Shared.Models.Items
{
    public class ItemModelValidator : AbstractValidator<AddItemModel>
    {
        private readonly List<string> _knownRetailers = new List<string> {"Amazon", "Argos"};

        public ItemModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Item Id is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Item Category is required");
            RuleFor(x => x.Retailer).NotEmpty().WithMessage("Item Retailer is required");
            RuleFor(x => x.Retailer).Must(BeAKnownRetailer).WithMessage("Please specify a known retailer");
        }

        private bool BeAKnownRetailer(string arg) 
            => _knownRetailers.Contains(arg);
    }
}