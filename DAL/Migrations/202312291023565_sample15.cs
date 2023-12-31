namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sample15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SLNO = c.Int(nullable: false),
                        CUSTNAME = c.String(nullable: false, maxLength: 100),
                        Question = c.String(maxLength: 255),
                        Date = c.DateTime(nullable: false),
                        Answer = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Questions");
        }
    }
}
