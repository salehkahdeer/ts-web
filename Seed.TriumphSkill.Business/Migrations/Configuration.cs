namespace Seed.TriumphSkill.Migrations
{
    using Seed.TriumphSkill.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Seed.TriumphSkill.Contexts.SqlDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Seed.TriumphSkill.Contexts.SqlDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Menus.AddOrUpdate(
                  p => p.ID,
                  new Menu { ID = 1, Name = "Home", DisplayName = "Home", Order = 1, Action = "Index", Controller = "Home", CreatedOn = DateTime.UtcNow },
                  new Menu { ID = 2, Name = "Security", DisplayName = "Security", Order = 2, Action = null, Controller = null, CreatedOn = DateTime.UtcNow },
                  new Menu { ID = 3, Name = "Users", DisplayName = "Users", Order = 3, Action = "Index", Controller = "User", ParentID = 2, CreatedOn = DateTime.UtcNow },
                  new Menu { ID = 4, Name = "Roles", DisplayName = "Roles", Order = 4, Action = "Index", Controller = "Role", ParentID = 2, CreatedOn = DateTime.UtcNow },                  
                  new Menu { ID = 5, Name = "Masters", DisplayName = "Masters", Order = 5, Action = null, Controller = null, CreatedOn = DateTime.UtcNow },
                  new Menu { ID = 6, Name = "Villages", DisplayName = "Villages", Order = 6, Action = "Index", Controller = "Village", ParentID = 5, CreatedOn = DateTime.UtcNow },
                  new Menu { ID = 7, Name = "Masters", DisplayName = "Settings", Order = 7, Action = "Index", Controller = "SystemSetting", CreatedOn = DateTime.UtcNow }
                );

            Models.Action roleIndex = new Models.Action { ID = 1, Name = "ACTION.Role.Index", DisplayName = "ACTION.Role.Index", CreatedOn = DateTime.UtcNow };
            Models.Action roleCreate = new Models.Action { ID = 2, Name = "ACTION.Role.Create", DisplayName = "ACTION.Role.Create", CreatedOn = DateTime.UtcNow };
            Models.Action roleEdit = new Models.Action { ID = 3, Name = "ACTION.Role.Edit", DisplayName = "ACTION.Role.Edit", CreatedOn = DateTime.UtcNow };
            Models.Action roleDelete = new Models.Action { ID = 4, Name = "ACTION.Role.Delete", DisplayName = "ACTION.Role.Delete", CreatedOn = DateTime.UtcNow };
            Models.Action userIndex = new Models.Action { ID = 5, Name = "ACTION.User.Index", DisplayName = "ACTION.User.Index", CreatedOn = DateTime.UtcNow };
            Models.Action userCreate = new Models.Action { ID = 6, Name = "ACTION.User.Create", DisplayName = "ACTION.User.Create", CreatedOn = DateTime.UtcNow };
            Models.Action userEdit = new Models.Action { ID = 7, Name = "ACTION.User.Edit", DisplayName = "ACTION.User.Edit", CreatedOn = DateTime.UtcNow };
            Models.Action userDelete = new Models.Action { ID = 8, Name = "ACTION.User.Delete", DisplayName = "ACTION.User.Delete", CreatedOn = DateTime.UtcNow };
            Models.Action homeIndex = new Models.Action { ID = 9, Name = "ACTION.Home.Index", DisplayName = "ACTION.Home.Index", CreatedOn = DateTime.UtcNow };
            Models.Action systemSettingIndex = new Models.Action { ID = 10, Name = "ACTION.SystemSetting.Index", DisplayName = "ACTION.SystemSetting.Index", CreatedOn = DateTime.UtcNow };
            Models.Action systemSettingCreate = new Models.Action { ID = 11, Name = "ACTION.SystemSetting.Create", DisplayName = "ACTION.SystemSetting.Create", CreatedOn = DateTime.UtcNow };
            Models.Action systemSettingEdit = new Models.Action { ID = 12, Name = "ACTION.SystemSetting.Edit", DisplayName = "ACTION.SystemSetting.Edit", CreatedOn = DateTime.UtcNow };
            Models.Action systemSettingDelete = new Models.Action { ID = 13, Name = "ACTION.SystemSetting.Delete", DisplayName = "ACTION.SystemSetting.Delete", CreatedOn = DateTime.UtcNow };
            

            context.Actions.AddOrUpdate(
                  p => p.ID,
                  roleIndex,
                  roleCreate,
                  roleEdit,
                  roleDelete,
                  userIndex,
                  userCreate,
                  userEdit,
                  userDelete,
                  homeIndex,
                  systemSettingIndex,
                  systemSettingCreate,
                  systemSettingEdit,
                  systemSettingDelete
                );

            context.Permissions.AddOrUpdate(
                  p => p.ID,
                  new Permission { ID = 1, Name = "View roles", Feature = "Security", Actions = new List<Models.Action>() { roleIndex }, CreatedOn = DateTime.UtcNow, },
                  new Permission { ID = 2, Name = "Manage roles", Feature = "Security", Actions = new List<Models.Action>() { roleCreate, roleEdit, roleDelete }, CreatedOn = DateTime.UtcNow },
                  new Permission { ID = 3, Name = "View users", Feature = "Security", Actions = new List<Models.Action>() { userIndex }, CreatedOn = DateTime.UtcNow },
                  new Permission { ID = 4, Name = "Manage users", Feature = "Security", Actions = new List<Models.Action>() { userIndex, userEdit, userDelete }, CreatedOn = DateTime.UtcNow },
                  new Permission { ID = 5, Name = "View home page", Feature = "Home Page", Actions = new List<Models.Action>() { homeIndex }, CreatedOn = DateTime.UtcNow },
                  new Permission { ID = 6, Name = "View settings", Feature = "System", Actions = new List<Models.Action>() { systemSettingIndex }, CreatedOn = DateTime.UtcNow },
                  new Permission { ID = 7, Name = "Manage settings", Feature = "System", Actions = new List<Models.Action>() { systemSettingIndex, systemSettingEdit, systemSettingDelete }, CreatedOn = DateTime.UtcNow }
                );

            context.SystemSettings.AddOrUpdate(
                p => p.ID,
                new SystemSetting { ID = 1, SettingType = SettingType.Dashboard, Comments = "Default dashboard settings", CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow, Name = "Home", Value = "{'title':'Dashboard','structure':'6-6','rows':[{'columns':[{'styleClass':'col-md-3','widgets':[{'type':'number','config':{'background':'#337ab7','foreground':'#ffffff','icon':'glyphicon-tasks','description':'Total Sales','url':'http://localhost:55952/Village/Statistics'},'title':'Total Sales','titleTemplateUrl':'../src/templates/widget-title.html','wid':7}],'cid':1},{'styleClass':'col-md-3','widgets':[{'type':'number','config':{'background':'#5cb85c','foreground':'#ffffff','icon':'glyphicon-usd','description':'Total Sales','url':'http://localhost:55952/Village/Statistics'},'title':'Number Widget','titleTemplateUrl':'../src/templates/widget-title.html','wid':8}],'cid':2},{'styleClass':'col-md-3','widgets':[{'type':'number','config':{'background':'#f0ad4e','foreground':'#ffffff','icon':'glyphicon-shopping-cart','description':'Total Sales','url':'http://localhost:55952/Village/Statistics'},'title':'Total Sales','titleTemplateUrl':'../src/templates/widget-title.html','wid':9}],'cid':3},{'styleClass':'col-md-3','widgets':[{'type':'number','config':{'background':'#d9534f','foreground':'#ffffff','icon':'glyphicon-bell','description':'Total Sales','url':'http://localhost:55952/Village/Statistics'},'title':'Number Widget','titleTemplateUrl':'../src/templates/widget-title.html','wid':10}],'cid':4}]},{'columns':[{'styleClass':'col-md-6','widgets':[{'type':'markdown','config':{'content':'![celusion logo](http://www.celusion.com/wp-content/uploads/2012/04/logo.png)   \n \n \n We are a team of highly skilled software professionals who are passionate about technology and committed to deliver in a challenging environment. We build software using an agile approach, that helps keep up with business dynamics. We empower our team to stay ahead of the technology curve and apply new & upcoming trends to solve business problems. \n \n \n Our engineering team is responsible for the research and development of pioneering software products in the field of mobility & communication. Over 1500 organizations across the globe, leverage these products integrated with their core business systems. Our current product portfolio comprise of SMS ConneXion® – Enterprise Messaging Platform & Mobiliteam™ – Enterprise Mobility Platform. \n \n \n Our services team provide the design, technology, team and methodology to deliver complex business software for Enterprises. We build desktop, web & mobile applications and are unbiased towards the use of different technologies to solve different problems. Our expertise, spans a broad range of technologies like .NET, Java, PHP, Android, iOS, HTML5 and JavaScript.'},'title':'Markdown','titleTemplateUrl':'../src/templates/widget-title.html','wid':11}],'cid':5},{'styleClass':'col-md-6','widgets':[{'type':'clock','config':{'timePattern':'HH:mm:ss','datePattern':'DD MMM YYYY'},'title':'Digital Clock','titleTemplateUrl':'../src/templates/widget-title.html','wid':12},{'type':'linklist','config':{'links':[{'title':'Celusion Technologies','href':'http://www.celusion.com'},{'title':'SMS ConneXion','href':'http://www.smsconnexion.com'},{'title':'Mobiliteam','href':'http://www.mobiliteam.in'}]},'title':'Links','titleTemplateUrl':'../src/templates/widget-title.html','wid':13}],'cid':6}]}],'titleTemplateUrl':'../src/templates/dashboard-title.html'}" }
                );
        }
    }
}
