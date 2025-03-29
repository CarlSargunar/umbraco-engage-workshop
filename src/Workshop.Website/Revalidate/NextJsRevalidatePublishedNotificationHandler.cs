using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Web;

namespace Workshop.Website.Revalidate
{
    public class NextJsRevalidatePublishedNotificationHandler : INotificationAsyncHandler<ContentPublishedNotification>
    {
        private readonly NextJsRevalidateService _revalidateService;
        private readonly ILogger<NextJsRevalidatePublishedNotificationHandler> _logger;
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        public NextJsRevalidatePublishedNotificationHandler(NextJsRevalidateService revalidateService,
            ILogger<NextJsRevalidatePublishedNotificationHandler> logger, IUmbracoContextAccessor umbracoContextAccessor)
        {
            _revalidateService = revalidateService;
            _logger = logger;
            _umbracoContextAccessor = umbracoContextAccessor;
        }

        private string[] AllowedContentContentType = new string[] { "Content", "Home", "Contact", "ArticleList", "Article" };

        private string[] NavigationTypes = new string[] { "Home" };

        public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken)
        {
            foreach (var content in notification.PublishedEntities)
            {
                if(AllowedContentContentType.Any(x => x == content.ContentType.Alias))
                {
                    if (_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext) && umbracoContext != null && umbracoContext.Content != null)
                    {
                        var publishedContent = umbracoContext.Content.GetById(content.Id);
                        if(publishedContent != null)
                        {
                            var path = publishedContent.Url();
                            _logger.LogInformation($"Web Content next js revalidation triggered for path {path}");
                            await _revalidateService.ForContent(path);
                        }
                    }
                }

                if(NavigationTypes.Any(x => x == content.ContentType.Alias))
                {
                    _logger.LogInformation("Navigation next js revalidation triggered");
                    await _revalidateService.ForNavigation();
                }
            }
        }
    }
}
