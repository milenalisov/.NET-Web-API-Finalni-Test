namespace MilenaLisov.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Druga : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Hotels", "Sobe", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Hotels", "Sobe", c => c.Int(nullable: false));
        }
    }
}
