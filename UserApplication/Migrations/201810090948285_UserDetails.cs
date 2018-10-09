namespace UserApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDetails : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Country", "CountryName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Country", "CountryName", c => c.Int(nullable: false));
        }
    }
}
