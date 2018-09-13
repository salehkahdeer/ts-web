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

namespace Seed.TriumphSkill.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService roleService;
        private readonly IPermissionService permissionService;
        public RoleController(IRoleService roleService, IPermissionService permissionService)
        {
            this.roleService = roleService;
            this.permissionService = permissionService;
        }

        public ActionResult Index(String successNotification)
        {
            if (!string.IsNullOrEmpty(successNotification))
            {
                ViewBag.Success = successNotification;
            }

            return View();
        }
        
        [HttpPost]
        public ActionResult Index(DataTablesPageRequest pageRequest)
        {
            SearchQuery<Models.Role> searchQuery = new SearchQuery<Models.Role>();
            searchQuery.Skip = pageRequest.DisplayStart;
            searchQuery.Take = pageRequest.DisplayLength;

            //add all search filterGroups
            if (!string.IsNullOrEmpty(pageRequest.Search))
            {
                searchQuery.AddFilter((e) => e.Name.Contains(pageRequest.Search));
            }

            //add all sort filterGroups
            var sortConditions = pageRequest.GetSortConditions();
            foreach (SortCondition condition in sortConditions)
            {
                FieldSortOrder<Models.Role> fieldSortOrder = new FieldSortOrder<Models.Role>();
                fieldSortOrder.Name = condition.ColumnName;
                fieldSortOrder.Direction = condition.SortDirection;

                //only sort on db column properties, ignore all custom/extended properties
                //for custom/extended properties, use manual sort/search after query result.
                searchQuery.AddSortCriteria(fieldSortOrder);
                
            }

            var members = roleService.Search(searchQuery);

            Page<Models.Role> petaPocoPage = new Page<Models.Role>()
            {
                Items = members.Entities.ToList(),
                ItemsPerPage = pageRequest.DisplayLength,
                CurrentPage = pageRequest.DisplayStart,
                TotalItems = members.Count,
                TotalPages = members.Count / pageRequest.DisplayLength
            };
            var pageResponse = DataTablesFormat.Roles(pageRequest, petaPocoPage);

            return Json(pageResponse);
        }

        //
        // GET: /Role/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Role/Create
        public ActionResult Create()
        {
            ViewModels.Role model = new ViewModels.Role();
            model.Permissions = ViewModels.Permission.ConvertAll(permissionService.All().OrderBy(r => r.Feature).ThenBy(r => r.Name).ToList());            
            model.Permissions.Find(p => p.PermissionInfo.Name == "View home page").IsSelected = true;
            return View(model);
        }

        //
        // POST: /Role/Create
        [HttpPost]
        public async Task<ActionResult> Create(ViewModels.Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (roleService.GetRoleByName(role.RoleInfo.Name, role.RoleInfo.ID) == null)
                    {
                        List<ViewModels.Permission> rolePermissions = role.Permissions.Where(p => p.IsSelected).ToList();
                        foreach (ViewModels.Permission permission in rolePermissions)
                        {
                            Models.Permission modelPermission = roleService.GetPermissionById(permission.PermissionInfo.ID);
                            role.RoleInfo.Permissions.Add(modelPermission);
                        }

                        await roleService.AddAsync(role.RoleInfo);

                        string successMessage = string.Format("Role <b>{0}</b> created successfully", role.RoleInfo.Name);
                        return RedirectToAction("Index", "Role", new { successNotification = Url.Encode(successMessage) });
                    }
                    else
                    {
                        ModelState.AddModelError("RoleInfo.Name", "Role Name already exists.");
                        role.Permissions = ViewModels.Permission.ConvertAll(permissionService.All().OrderBy(r => r.Feature).ThenBy(r => r.Name).ToList());
                        return View(role);
                    }
                }
                else
                {
                    role.Permissions = ViewModels.Permission.ConvertAll(permissionService.All().OrderBy(r => r.Feature).ThenBy(r => r.Name).ToList());
                    return View(role);
                }
            }
            catch
            {
                role.Permissions = ViewModels.Permission.ConvertAll(permissionService.All().OrderBy(r => r.Feature).ThenBy(r => r.Name).ToList());
                return View(role);
            }
        }

        //
        // GET: /Role/Edit/5

        public ActionResult Edit(int id)
        {
            Models.Role role = roleService.GetRoleById(id);
            ViewModels.Role model = new ViewModels.Role();
            model.RoleInfo = role;
            model.Permissions = ViewModels.Permission.ConvertAll(permissionService.All().OrderBy(r => r.Feature).ThenBy(r => r.Name).ToList());
            foreach (Models.Permission permission in role.Permissions)
            {
                model.Permissions.Find(p => p.PermissionInfo.Name == permission.Name).IsSelected = true;
            }
            return View(model);
        }

        //
        // POST: /Role/Edit/5

        [HttpPost]
        public async Task<ActionResult> Edit(int id, ViewModels.Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (roleService.GetRoleByName(role.RoleInfo.Name, role.RoleInfo.ID) == null)
                    {
                        Models.Role dbRole = roleService.GetRoleById(role.RoleInfo.ID);
                        dbRole.Name = role.RoleInfo.Name;
                        dbRole.Permissions.Clear();
                        List<ViewModels.Permission> rolePermissions = role.Permissions.Where(p => p.IsSelected).ToList();
                        foreach (ViewModels.Permission permission in rolePermissions)
                        {
                            Models.Permission modelPermission = roleService.GetPermissionById(permission.PermissionInfo.ID);
                            dbRole.Permissions.Add(modelPermission);
                        }

                        await roleService.UpdateAsync(dbRole);

                        string successMessage = string.Format("Role <b>{0}</b> updated successfully", role.RoleInfo.Name);
                        return RedirectToAction("Index", "Role", new { successNotification = Url.Encode(successMessage) });
                    }
                    else
                    {
                        ModelState.AddModelError("RoleInfo.Name", "Role Name already exists.");
                        role.Permissions = ViewModels.Permission.ConvertAll(permissionService.All().OrderBy(r => r.Feature).ThenBy(r => r.Name).ToList());
                        return View(role);
                    }
                }
                else
                {
                    role.Permissions = ViewModels.Permission.ConvertAll(permissionService.All().OrderBy(r => r.Feature).ThenBy(r => r.Name).ToList());
                    return View(role);
                }
            }
            catch
            {
                role.Permissions = ViewModels.Permission.ConvertAll(permissionService.All().OrderBy(r => r.Feature).ThenBy(r => r.Name).ToList());
                return View(role);
            }
        }

        public ActionResult Delete(int id)
        {
            Models.Role role = roleService.GetRoleById(id);
            return View(role);
        }

        //
        // POST: /Role/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, Models.Role role)
        {
            try
            {                
                await roleService.DeleteAsync(id);

                string successMessage = string.Format("Role <b>{0}</b> deleted successfully", role.Name);
                return RedirectToAction("Index", "Role", new { successNotification = Url.Encode(successMessage) });                
            }
            catch
            {
                return View();
            }
        }
    }
}
