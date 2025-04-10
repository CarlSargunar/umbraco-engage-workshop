using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Workshop.Website.Revalidate
{
    public class NextJsRevalidateDictionaryNotificationHandler : INotificationAsyncHandler<DictionaryItemSavedNotification>
    {
        private readonly NextJsRevalidateService _revalidateService;
        private readonly ILogger<NextJsRevalidatePublishedNotificationHandler> _logger;

        public NextJsRevalidateDictionaryNotificationHandler(NextJsRevalidateService revalidateService,
            ILogger<NextJsRevalidatePublishedNotificationHandler> logger)
        {
            _revalidateService = revalidateService;
            _logger = logger;
        }

        public async Task HandleAsync(DictionaryItemSavedNotification notification, CancellationToken cancellationToken)
        {

            if (notification.SavedEntities.Any())
            {
                _logger.LogInformation("Localisation next js revalidation triggered");
                await _revalidateService.ForLocalisation();
            }
        }
    }
}
