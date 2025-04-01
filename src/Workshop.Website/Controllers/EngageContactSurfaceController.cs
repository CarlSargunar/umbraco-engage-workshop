using Clean.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Mail;
using Umbraco.Cms.Core.Models.Email;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;


namespace Workshop.Website.Controllers;

public class EngageContactSurfaceController : SurfaceController
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<EngageContactSurfaceController> _logger;
    private readonly GlobalSettings _globalSettings;

    public EngageContactSurfaceController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        IEmailSender emailSender,
        ILogger<EngageContactSurfaceController> logger,
        IOptions<GlobalSettings> globalSettings)
        : base(umbracoContextAccessor, databaseFactory,
              services, appCaches, profilingLogger,
              publishedUrlProvider)
    {
        _emailSender = emailSender;
        _logger = logger;
        _globalSettings = globalSettings.Value;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitForm(ContactViewModel model)
    {
        if (!ModelState.IsValid) return CurrentUmbracoPage();

        TempData["Success"] = await SendEmail(model);

        return RedirectToCurrentUmbracoPage();
    }

    public async Task<bool> SendEmail(ContactViewModel model)
    {
        try
        {
            var fromAddress = _globalSettings.Smtp.From;

            var subject = string.Format("Enquiry from: {0} - {1}", model.Name, model.Email);
            EmailMessage message = new EmailMessage(fromAddress, fromAddress, subject, model.Message, false);
            await _emailSender.SendAsync(message, emailType: "Contact");

            _logger.LogInformation("Contact Form Submitted Successfully");
            return true;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error When Submitting Contact Form");
            return false;
        }
    }
}
