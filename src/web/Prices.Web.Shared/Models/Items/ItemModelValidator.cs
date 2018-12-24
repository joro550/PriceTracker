using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace Prices.Web.Shared.Models.Items
{
    public class ItemModelValidator : AbstractValidator<AddItemModel>
    {
        public ItemModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Item Id is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Item Category is required");
            RuleFor(x => x.Retailer).NotEmpty().WithMessage("Item Retailer is required");
            RuleFor(x => x.Retailer).Must((model, retailer) => BeAKnownRetailer(model.RetailerList, retailer))
                .WithMessage("Please specify a known retailer");
        }

        private static bool BeAKnownRetailer(IEnumerable<SelectListItem> knownRetailers, string retailerValue)
        {
            return knownRetailers.Select(retailer => retailer.Value).Contains(retailerValue);
        }
    }
}