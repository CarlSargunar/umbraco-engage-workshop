using Umbraco.Engage.Infrastructure.Permissions.ModulePermissions;

namespace Workshop.Website.Engage;

public class CookieModulePermission : IModulePermissions
{
    public bool AbTestingIsAllowed(HttpContext context)
    {
        return true;
    }

    public bool AnalyticsIsAllowed(HttpContext context)
    {
        return true;
    }

    public bool PersonalizationIsAllowed(HttpContext context)
    {
        return true;
    }
}