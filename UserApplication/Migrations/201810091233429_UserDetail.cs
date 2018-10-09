namespace UserApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserViewModels",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Gender = c.String(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        Hobbies = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        IsEmailVerified = c.String(),
                        Password = c.String(nullable: false),
                        ConfirmPassword = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CourseId = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        RoleId = c.Int(nullable: false),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.CourseModels",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseName = c.String(),
                        UserViewModel_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.UserViewModels", t => t.UserViewModel_UserId)
                .Index(t => t.UserViewModel_UserId);
            
            CreateTable(
                "dbo.RoleModels",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        UserViewModel_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.RoleId)
                .ForeignKey("dbo.UserViewModels", t => t.UserViewModel_UserId)
                .Index(t => t.UserViewModel_UserId);
            
            AddColumn("dbo.Address", "AddressLine", c => c.String());
            AddColumn("dbo.User", "ConfirmPassword", c => c.String(nullable: false));
            AddColumn("dbo.User", "IsEmailVerified", c => c.String());
            AddColumn("dbo.User", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.User", "DateModified", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleModels", "UserViewModel_UserId", "dbo.UserViewModels");
            DropForeignKey("dbo.CourseModels", "UserViewModel_UserId", "dbo.UserViewModels");
            DropIndex("dbo.RoleModels", new[] { "UserViewModel_UserId" });
            DropIndex("dbo.CourseModels", new[] { "UserViewModel_UserId" });
            DropColumn("dbo.User", "DateModified");
            DropColumn("dbo.User", "DateCreated");
            DropColumn("dbo.User", "IsEmailVerified");
            DropColumn("dbo.User", "ConfirmPassword");
            DropColumn("dbo.Address", "AddressLine");
            DropTable("dbo.RoleModels");
            DropTable("dbo.CourseModels");
            DropTable("dbo.UserViewModels");
        }
    }
}
