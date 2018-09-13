using Seed.TriumphSkill.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Seed.TriumphSkill.Controllers
{
    [SimpleAuthorize]
    public class HomeController : Controller
    {
        private readonly IMenuService menuService;
        private readonly IUserService userService;
        private readonly IDashboardService dashboardService;
        private readonly ISystemSettingService systemSettingService;

        public HomeController(IMenuService menuService,
            IUserService userService,
            IDashboardService dashboardService,
            ISystemSettingService systemSettingService)
        {
            this.menuService = menuService;
            this.userService = userService;
            this.dashboardService = dashboardService;
            this.systemSettingService = systemSettingService;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View();
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult Menu()
        {
            var menu = menuService.GetAllForUser((User.Identity as SimpleIdentity).Username);
            return PartialView("_Menu", menu);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public JObject Dashboard(string name)
        {
            string configuration = "";
            Models.User user = userService.GetUserByUsername((User.Identity as SimpleIdentity).Username);
            Models.Dashboard dashboard = dashboardService.GetDashboardByUser(user.ID, name);
            if (dashboard == null)
            {
                Models.SystemSetting systemSetting = systemSettingService.GetAllByType(Models.SettingType.Dashboard).Where(s => s.Name == name).FirstOrDefault();
                if (systemSetting != null)
                {
                    configuration = systemSetting.Value;
                }
            }
            else
            {
                configuration = dashboard.Configuration;
            }

            if (!String.IsNullOrWhiteSpace(configuration))
                return (JObject)JsonConvert.DeserializeObject(configuration);
            else
                return null;
        }

        [HttpPost]
        public async Task<ActionResult> Dashboard()
        {
            String name = Request.Params["name"];
            String body = new StreamReader(Request.InputStream).ReadToEnd();

            if (!String.IsNullOrWhiteSpace(body))
            {
                Models.User user = userService.GetUserByUsername((User.Identity as SimpleIdentity).Username);
                Models.Dashboard dashboard = dashboardService.GetDashboardByUser(user.ID, name);
                if (dashboard == null)
                {
                    dashboard = new Models.Dashboard();
                    dashboard.Name = name;
                    dashboard.User_ID = user.ID;
                    dashboard.CreatedOn = DateTime.UtcNow;
                }
                dashboard.ModifiedOn = DateTime.UtcNow;
                dashboard.Configuration = body;
                if (dashboard.ID > 0)
                {
                    await dashboardService.UpdateAsync(dashboard);
                }
                else
                {
                    await dashboardService.AddAsync(dashboard);
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}
