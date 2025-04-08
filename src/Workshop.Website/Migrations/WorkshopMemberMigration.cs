using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;

namespace Workshop.Website.Migrations;

public class WorkshopMemberMigration : MigrationBase
{
    private readonly IMemberService _memberService;

    public WorkshopMemberMigration(IMigrationContext context, IMemberService memberService) : base(context)
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