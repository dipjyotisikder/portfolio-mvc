namespace folio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Pskills", "CategoryId", "dbo.Pcategories");
            //DropIndex("dbo.Pskills", new[] { "CategoryId" });
        }

        public override void Down()
        {
            CreateIndex("dbo.Pskills", "CategoryId");
            AddForeignKey("dbo.Pskills", "CategoryId", "dbo.Pcategories", "Id", cascadeDelete: true);
        }
    }
}
