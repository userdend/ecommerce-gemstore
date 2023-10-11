using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OnlineStore.ViewsComponent
{
    public class NavbarViewComponent : ViewComponent
    {
        public NavbarViewComponent() { }
        public async Task<IViewComponentResult> InvokeAsync() {
            ClaimsPrincipal claimUser = HttpContext.User;
            return await Task.FromResult((IViewComponentResult)View(claimUser));
        }
    }
}
