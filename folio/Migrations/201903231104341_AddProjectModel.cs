namespace folio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddProjectModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectImages",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ImageUrl = c.String(),
                    ProjectId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);

            CreateTable(
                "dbo.Projects",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Pskills", "Strength", c => c.Int(nullable: true));
        }

        public override void Down()
        {
            DropForeignKey("dbo.ProjectImages", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectImages", new[] { "ProjectId" });
            DropColumn("dbo.Pskills", "Strength");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectImages");
        }
    }
}
