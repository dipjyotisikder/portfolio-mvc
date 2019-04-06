namespace folio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddTableProjectFeature : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectFeatures",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    ProjectId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.ProjectFeatures", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectFeatures", new[] { "ProjectId" });
            DropTable("dbo.ProjectFeatures");
        }
    }
}
