using Seed.TriumphSkill.Models;
using Seed.TriumphSkill.Models.DataTables;
using Seed.TriumphSkill.Services;
using Seed.TriumphSkill.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Seed.TriumphSkill.Controllers
{
    [SimpleAuthorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        public UserController(IUserService userService,
            IRoleService roleService)
        {
            this.userService = userService;
            this.roleService = roleService;
        }

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(DataTablesPageRequest pageRequest)
        {
            SearchQuery<Models.User> searchQuery = new SearchQuery<Models.User>();
            searchQuery.Skip = pageRequest.DisplayStart;
            searchQuery.Take = pageRequest.DisplayLength;

            //add all search filterGroups
            if (!string.IsNullOrEmpty(pageRequest.Search))
            {
                searchQuery.AddFilter((e) => e.Username.Contains(pageRequest.Search) || e.Name.Contains(pageRequest.Search));
            }

            //add all sort filterGroups
            var sortConditions = pageRequest.GetSortConditions();
            foreach (SortCondition condition in sortConditions)
            {
                FieldSortOrder<Models.User> fieldSortOrder = new FieldSortOrder<Models.User>();
                fieldSortOrder.Name = condition.ColumnName;
                fieldSortOrder.Direction = condition.SortDirection;

                //only sort on db column properties, ignore all custom/extended properties
                //for custom/extended properties, use manual sort/search after query result.
                searchQuery.AddSortCriteria(fieldSortOrder);
                
            }

            var members = userService.Search(searchQuery);

            Page<Models.User> petaPocoPage = new Page<Models.User>()
            {
                Items = members.Entities.ToList(),
                ItemsPerPage = pageRequest.DisplayLength,
                CurrentPage = pageRequest.DisplayStart,
                TotalItems = members.Count,
                TotalPages = members.Count / pageRequest.DisplayLength
            };
            var pageResponse = DataTablesFormat.Users(pageRequest, petaPocoPage);

            return Json(pageResponse);
        }

        //
        // GET: /User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewModels.User model = new ViewModels.User();
            model.Roles = ViewModels.Role.ConvertAll(roleService.All().OrderBy(r => r.Name).ToList());            
            return View(model);
        }

        //
        // POST: /Role/Create
        [HttpPost]
        public async Task<ActionResult> Create(ViewModels.User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    WebSecurity.CreateUserAndAccount(user.UserName, user.Password, new { Name = user.UserInfo.Name, Email = user.UserInfo.Email, ModifiedOn = DateTime.UtcNow });

                    Models.User dbUser = userService.GetUserByUsername(user.UserName);                    
                    List<ViewModels.Role> userRoles = user.Roles.Where(p => p.IsSelected).ToList();
                    foreach (ViewModels.Role role in userRoles)
                    {
                        Models.Role modelRole = userService.GetRoleById(role.RoleInfo.ID);
                        dbUser.Roles.Add(modelRole);
                    }

                    await userService.UpdateAsync(dbUser);

                    string successMessage = string.Format("User <b>{0}</b> created successfully", user.UserInfo.Name);
                    return RedirectToAction("Index", "User", new { successNotification = Url.Encode(successMessage) });
                    
                }
                else
                {
                    user.Roles = ViewModels.Role.ConvertAll(roleService.All().OrderBy(r => r.Name).ToList());
                    return View(user);
                }
            }
            catch (MembershipCreateUserException ex)
            {
                user.Roles = ViewModels.Role.ConvertAll(roleService.All().OrderBy(r => r.Name).ToList());
                ModelState.AddModelError("", ErrorCodeToString(ex.StatusCode));
                return View(user);
            }
            catch(Exception ex)
            {
                user.Roles = ViewModels.Role.ConvertAll(roleService.All().OrderBy(r => r.Name).ToList());
                ModelState.AddModelError("", ex.ToString());
                return View(user);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            Models.User user = userService.GetUserById(id);
            ViewModels.User model = new ViewModels.User();
            model.UserInfo = user;
            model.UserName = user.Username;
            model.Password = "dummypassword";
            model.ConfirmPassword = "dummypassword";
            model.Roles = ViewModels.Role.ConvertAll(roleService.All().OrderBy(r => r.Name).ToList());
            foreach (Models.Role role in user.Roles)
            {
                model.Roles.Find(p => p.RoleInfo.Name == role.Name).IsSelected = true;
            }
            return View(model);
        }

        //
        // POST: /Role/Edit/5

        [HttpPost]
        public async Task<ActionResult> Edit(int id, ViewModels.User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Models.User dbUser = userService.GetUserById(user.UserInfo.ID);
                    dbUser.Name = user.UserInfo.Name;
                    dbUser.Email = user.UserInfo.Email;
                    dbUser.Roles.Clear();
                    List<ViewModels.Role> userRoles = user.Roles.Where(p => p.IsSelected).ToList();
                    foreach (ViewModels.Role role in userRoles)
                    {
                        Models.Role modelRole = userService.GetRoleById(role.RoleInfo.ID);
                        dbUser.Roles.Add(modelRole);
                    }

                    await userService.UpdateAsync(dbUser);

                    string successMessage = string.Format("User <b>{0}</b> updated successfully", user.UserInfo.Name);
                    return RedirectToAction("Index", "User", new { successNotification = Url.Encode(successMessage) });
                    
                }
                else
                {
                    user.Roles = ViewModels.Role.ConvertAll(roleService.All().OrderBy(r => r.Name).ToList());
                    return View(user);
                }
            }
            catch
            {
                user.Roles = ViewModels.Role.ConvertAll(roleService.All().OrderBy(r => r.Name).ToList());
                return View(user);
            }
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id)
        {
            Models.User user = userService.GetUserById(id);
            return View(user);
        }

        //
        // POST: /Role/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, Models.User user)
        {
            try
            {
                ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(user.Username); // deletes record from webpages_Membership table
                ((SimpleMembershipProvider)Membership.Provider).DeleteUser(user.Username, true); // deletes record from UserProfile table

                string successMessage = string.Format("User <b>{0}</b> deleted successfully", user.Name);
                return RedirectToAction("Index", "User", new { successNotification = Url.Encode(successMessage) });
            }
            catch
            {
                return View();
            }
        }
    }
}
