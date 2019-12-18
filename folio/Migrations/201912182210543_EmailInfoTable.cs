namespace folio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailInfoTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Subject = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailInfoes");
        }
    }
}
