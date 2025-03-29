using System.ComponentModel.DataAnnotations;

namespace Workshop.Website.Revalidate
{
    public class NextJsRevalidateOptions
    {
        [Required]
        public string? WebHookUrls { get; set; } = null;

        [Required]
        public string WebHookSecret { get; set; } = string.Empty;

    }
}