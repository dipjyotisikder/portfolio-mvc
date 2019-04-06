namespace folio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addNew : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectSkills", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectSkills", "SkillId", "dbo.Pskills");
            DropTable("dbo.ProjectSkills");
            CreateTable(
                "dbo.ProjectSkills",
                c => new
                {
                    ProjectId = c.Int(nullable: false),
                    SkillId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.ProjectId, t.SkillId })
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Pskills", t => t.SkillId, cascadeDelete: true);

            //CreateIndex("dbo.Pskills", "CategoryId");
            //AddForeignKey("dbo.Pskills", "CategoryId", "dbo.Pcategories", "Id", cascadeDelete: true);

        }

        public override void Down()
        {
            CreateTable(
                "dbo.ProjectSkills",
                c => new
                {
                    ProjectId = c.Int(nullable: false),
                    SkillId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.ProjectId, t.SkillId });

            DropForeignKey("dbo.ProjectSkills", "SkillId", "dbo.Pskills");
            DropForeignKey("dbo.ProjectSkills", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Pskills", "CategoryId", "dbo.Pcategories");
            DropIndex("dbo.Pskills", new[] { "CategoryId" });
            DropTable("dbo.ProjectSkills");
            AddForeignKey("dbo.ProjectSkills", "SkillId", "dbo.Pskills", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProjectSkills", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
        }
    }
}
