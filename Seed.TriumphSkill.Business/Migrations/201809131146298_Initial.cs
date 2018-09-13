namespace Seed.TriumphSkill.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DisplayName = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Feature = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 250),
                        Email = c.String(nullable: false, maxLength: 250),
                        IsAdministrator = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Dashboards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        User_ID = c.Int(nullable: false),
                        Configuration = c.String(storeType: "ntext"),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.User_ID, cascadeDelete: true)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        DisplayName = c.String(maxLength: 100),
                        Action = c.String(maxLength: 100),
                        Controller = c.String(maxLength: 100),
                        Order = c.Int(nullable: false),
                        ParentID = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Menus", t => t.ParentID)
                .Index(t => t.ParentID);
            
            CreateTable(
                "dbo.SystemSettings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SettingTypeValue = c.String(nullable: false),
                        SettingType = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        Value = c.String(nullable: false),
                        Comments = c.String(maxLength: 1024),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Villages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PermissionActions",
                c => new
                    {
                        Permission_ID = c.Int(nullable: false),
                        Action_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Permission_ID, t.Action_ID })
                .ForeignKey("dbo.Permissions", t => t.Permission_ID, cascadeDelete: true)
                .ForeignKey("dbo.Actions", t => t.Action_ID, cascadeDelete: true)
                .Index(t => t.Permission_ID)
                .Index(t => t.Action_ID);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        Role_ID = c.Int(nullable: false),
                        Permission_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_ID, t.Permission_ID })
                .ForeignKey("dbo.Roles", t => t.Role_ID, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.Permission_ID, cascadeDelete: true)
                .Index(t => t.Role_ID)
                .Index(t => t.Permission_ID);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        User_ID = c.Int(nullable: false),
                        Role_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_ID, t.Role_ID })
                .ForeignKey("dbo.Users", t => t.User_ID, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_ID, cascadeDelete: true)
                .Index(t => t.User_ID)
                .Index(t => t.Role_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Menus", "ParentID", "dbo.Menus");
            DropForeignKey("dbo.UserRoles", "Role_ID", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Dashboards", "User_ID", "dbo.Users");
            DropForeignKey("dbo.RolePermissions", "Permission_ID", "dbo.Permissions");
            DropForeignKey("dbo.RolePermissions", "Role_ID", "dbo.Roles");
            DropForeignKey("dbo.PermissionActions", "Action_ID", "dbo.Actions");
            DropForeignKey("dbo.PermissionActions", "Permission_ID", "dbo.Permissions");
            DropIndex("dbo.UserRoles", new[] { "Role_ID" });
            DropIndex("dbo.UserRoles", new[] { "User_ID" });
            DropIndex("dbo.RolePermissions", new[] { "Permission_ID" });
            DropIndex("dbo.RolePermissions", new[] { "Role_ID" });
            DropIndex("dbo.PermissionActions", new[] { "Action_ID" });
            DropIndex("dbo.PermissionActions", new[] { "Permission_ID" });
            DropIndex("dbo.Menus", new[] { "ParentID" });
            DropIndex("dbo.Dashboards", new[] { "User_ID" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.RolePermissions");
            DropTable("dbo.PermissionActions");
            DropTable("dbo.Villages");
            DropTable("dbo.SystemSettings");
            DropTable("dbo.Menus");
            DropTable("dbo.Dashboards");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Permissions");
            DropTable("dbo.Actions");
        }
    }
}
