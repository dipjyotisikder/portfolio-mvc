namespace folio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class secondMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "PermanentAddress", c => c.String());
            AddColumn("dbo.AspNetUsers", "CurrentAddress", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CurrentAddress");
            DropColumn("dbo.AspNetUsers", "PermanentAddress");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
