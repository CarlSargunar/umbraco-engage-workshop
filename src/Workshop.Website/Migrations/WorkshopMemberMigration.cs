using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Packaging;

namespace Workshop.Website.Migrations;

public class WorkshopMemberMigration : PackageMigrationBase
{
    private readonly IMemberService _memberService;

    public WorkshopMemberMigration(
        IPackagingService packagingService,
        IMediaService mediaService,
        MediaFileManager mediaFileManager,
        MediaUrlGeneratorCollection mediaUrlGenerators,
        IShortStringHelper shortStringHelper,
        IContentTypeBaseServiceProvider contentTypeBaseServiceProvider,
        IMigrationContext context,
        IOptions<PackageMigrationSettings> packageMigrationsSettings,
        IMemberService memberService) : base(packagingService, mediaService, mediaFileManager, mediaUrlGenerators, shortStringHelper, contentTypeBaseServiceProvider, context, packageMigrationsSettings)
    {
        _memberService = memberService;
    }

    protected override void Migrate()
    {
        var member = _memberService.CreateMember("workshop.member", "workshop@example.com", "Workshop Member", "member");
        member.Key = new Guid("faa51ba9-c3e5-44a1-89e8-07e181b27903");

        _memberService.Save(member);
    }
}