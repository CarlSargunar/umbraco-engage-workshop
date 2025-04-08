using Umbraco.Engage.Headless.Extensions;
using Umbraco.Engage.Infrastructure.Permissions.ModulePermissions;
using Workshop.Website.Engage;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCors(options => options.AddPolicy("AllowAnyOrigin", policy => policy.AllowAnyOrigin()))
    .AddUnique<IModulePermissions, CookieModulePermission>();

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddEngageApiDocumentation()
    .AddComposers()
    .Build();

WebApplication app = builder.Build();

await app.BootUmbracoAsync();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.AppBuilder.UseCors("AllowAnyOrigin");
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
