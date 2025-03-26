using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.Security;

namespace Workshop.Website.Controllers;

public class LoginController : RenderController
{
    private readonly MemberManager _memberManager;
    private readonly MemberSignInManager _memberSignInManager;

    public LoginController(
        ILogger<RenderController> logger,
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        MemberManager memberManager,
        MemberSignInManager memberSignInManager) : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
        _memberManager = memberManager;
        _memberSignInManager = memberSignInManager;
    }

    [NonAction]
    public sealed override IActionResult Index() => throw new NotImplementedException();

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var member = await _memberManager.FindByEmailAsync("workshop@example.com");

        if (member == null)
        {
            return NotFound();
        }

        await _memberSignInManager.SignInAsync(member, false);

        return Redirect("/");
    }
}