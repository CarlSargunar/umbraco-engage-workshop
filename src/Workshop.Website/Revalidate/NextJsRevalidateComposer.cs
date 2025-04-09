using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Notifications;

namespace Workshop.Website.Revalidate
{
    public class NextJsRevalidateComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddTransient<NextJsRevalidateService>();
            builder.AddNotificationAsyncHandler<ContentPublishedNotification, NextJsRevalidatePublishedNotificationHandler>();
            builder.AddNotificationAsyncHandler<DictionaryItemSavedNotification, NextJsRevalidateDictionaryNotificationHandler>();

            OptionsBuilder<NextJsRevalidateOptions> optionsBuilder = builder.Services.AddOptions<NextJsRevalidateOptions>()
                .BindConfiguration("NextJs:Revalidate")
                .ValidateDataAnnotations();
        }
    }
}