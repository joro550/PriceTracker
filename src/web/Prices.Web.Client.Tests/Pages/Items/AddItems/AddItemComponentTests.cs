using Xunit;
using AutoFixture.Xunit2;
using System.Threading.Tasks;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Prices.Web.Client.Tests.Pages.Items.AddItems
{
    public class AddItemComponentTests
    {
        [Theory, AutoData]
        public async Task WhenAddingAnItemBadRequestIsReturned_ThenErrorsAreDisplayed(List<ValidationFailure> failure)
        {
            var component = new AddItemComponentBuilder()
                .WithMessageHandlerWithValidationErrors(failure)
                .Build();

            await component.AddUserToken("Token");
            await component.AddItem();
        }
    }
}