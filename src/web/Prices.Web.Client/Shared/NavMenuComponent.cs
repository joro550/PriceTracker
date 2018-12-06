using Microsoft.AspNetCore.Blazor.Components;

namespace Prices.Web.Client.Shared
{
    public class NavMenuComponent : BlazorComponent
    {
        private bool _collapseNavMenu;

        protected void ToggleNavMenu() 
            => _collapseNavMenu = !_collapseNavMenu;

        protected string NavMenuClass() 
            => _collapseNavMenu ? "collapse" : null;
    }
}