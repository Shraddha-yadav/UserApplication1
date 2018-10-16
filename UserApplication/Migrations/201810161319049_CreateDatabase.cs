namespace UserApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        AddressLine1 = c.String(nullable: false),
                        AddressLine2 = c.String(nullable: false),
                        CountryId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        Zipcode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.City", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.State", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.Country", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId)
                .Index(t => t.StateId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        CityName = c.String(),
                        StateId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("dbo.State", t => t.StateId, cascadeDelete: false)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        StateName = c.String(),
                        CountryId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StateId)
                .ForeignKey("dbo.Country", t => t.CountryId, cascadeDelete: false)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryName = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        Hobbies = c.String(),
                        Password = c.String(),
                        ConfirmPassword = c.String(),
                        Email = c.String(),
                        IsEmailVerified = c.String(),
                        DOB = c.DateTime(nullable: false),
                        CourseId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        AddressId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Address", t => t.AddressId, cascadeDelete: true)
                .ForeignKey("dbo.Course", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.RoleId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseName = c.String(),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "dbo.SubjectInCourse",
                c => new
                    {
                        SubjectInCourseId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubjectInCourseId)
                .ForeignKey("dbo.Course", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Subject", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Subject",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        SubjectName = c.String(),
                    })
                .PrimaryKey(t => t.SubjectId);
            
            CreateTable(
                "dbo.TeacherInSubject",
                c => new
                    {
                        TeacherInSubjectId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeacherInSubjectId)
                .ForeignKey("dbo.Subject", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.UserInRole",
                c => new
                    {
                        UserInRoleId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserInRoleId)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserViewModel",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Gender = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        Hobbies = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        IsEmailVerified = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        ConfirmPassword = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CourseId = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        AddressLine1 = c.String(nullable: false),
                        AddressLine2 = c.String(nullable: false),
                        Zipcode = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        RoleId = c.Int(nullable: false),
                        RoleName = c.String(),
                        CountryId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.CityModels",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        CityName = c.String(),
                        UserViewModel_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("dbo.UserViewModel", t => t.UserViewModel_UserId)
                .Index(t => t.UserViewModel_UserId);
            
            CreateTable(
                "dbo.CountryModels",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryName = c.String(nullable: false),
                        UserViewModel_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.CountryId)
                .ForeignKey("dbo.UserViewModel", t => t.UserViewModel_UserId)
                .Index(t => t.UserViewModel_UserId);
            
            CreateTable(
                "dbo.CourseModels",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseName = c.String(),
                        UserViewModel_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.UserViewModel", t => t.UserViewModel_UserId)
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
                .ForeignKey("dbo.UserViewModel", t => t.UserViewModel_UserId)
                .Index(t => t.UserViewModel_UserId);
            
            CreateTable(
                "dbo.StateModels",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        StateName = c.String(),
                        UserViewModel_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.StateId)
                .ForeignKey("dbo.UserViewModel", t => t.UserViewModel_UserId)
                .Index(t => t.UserViewModel_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StateModels", "UserViewModel_UserId", "dbo.UserViewModel");
            DropForeignKey("dbo.RoleModels", "UserViewModel_UserId", "dbo.UserViewModel");
            DropForeignKey("dbo.CourseModels", "UserViewModel_UserId", "dbo.UserViewModel");
            DropForeignKey("dbo.CountryModels", "UserViewModel_UserId", "dbo.UserViewModel");
            DropForeignKey("dbo.CityModels", "UserViewModel_UserId", "dbo.UserViewModel");
            DropForeignKey("dbo.User", "RoleId", "dbo.Role");
            DropForeignKey("dbo.UserInRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserInRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.User", "CourseId", "dbo.Course");
            DropForeignKey("dbo.TeacherInSubject", "UserId", "dbo.User");
            DropForeignKey("dbo.TeacherInSubject", "SubjectId", "dbo.Subject");
            DropForeignKey("dbo.SubjectInCourse", "SubjectId", "dbo.Subject");
            DropForeignKey("dbo.SubjectInCourse", "CourseId", "dbo.Course");
            DropForeignKey("dbo.User", "AddressId", "dbo.Address");
            DropForeignKey("dbo.State", "CountryId", "dbo.Country");
            DropForeignKey("dbo.Address", "CountryId", "dbo.Country");
            DropForeignKey("dbo.City", "StateId", "dbo.State");
            DropForeignKey("dbo.Address", "StateId", "dbo.State");
            DropForeignKey("dbo.Address", "CityId", "dbo.City");
            DropIndex("dbo.StateModels", new[] { "UserViewModel_UserId" });
            DropIndex("dbo.RoleModels", new[] { "UserViewModel_UserId" });
            DropIndex("dbo.CourseModels", new[] { "UserViewModel_UserId" });
            DropIndex("dbo.CountryModels", new[] { "UserViewModel_UserId" });
            DropIndex("dbo.CityModels", new[] { "UserViewModel_UserId" });
            DropIndex("dbo.UserInRole", new[] { "RoleId" });
            DropIndex("dbo.UserInRole", new[] { "UserId" });
            DropIndex("dbo.TeacherInSubject", new[] { "SubjectId" });
            DropIndex("dbo.TeacherInSubject", new[] { "UserId" });
            DropIndex("dbo.SubjectInCourse", new[] { "CourseId" });
            DropIndex("dbo.SubjectInCourse", new[] { "SubjectId" });
            DropIndex("dbo.User", new[] { "AddressId" });
            DropIndex("dbo.User", new[] { "RoleId" });
            DropIndex("dbo.User", new[] { "CourseId" });
            DropIndex("dbo.State", new[] { "CountryId" });
            DropIndex("dbo.City", new[] { "StateId" });
            DropIndex("dbo.Address", new[] { "CityId" });
            DropIndex("dbo.Address", new[] { "StateId" });
            DropIndex("dbo.Address", new[] { "CountryId" });
            DropTable("dbo.StateModels");
            DropTable("dbo.RoleModels");
            DropTable("dbo.CourseModels");
            DropTable("dbo.CountryModels");
            DropTable("dbo.CityModels");
            DropTable("dbo.UserViewModel");
            DropTable("dbo.UserInRole");
            DropTable("dbo.Role");
            DropTable("dbo.TeacherInSubject");
            DropTable("dbo.Subject");
            DropTable("dbo.SubjectInCourse");
            DropTable("dbo.Course");
            DropTable("dbo.User");
            DropTable("dbo.Country");
            DropTable("dbo.State");
            DropTable("dbo.City");
            DropTable("dbo.Address");
        }
    }
}
