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
    [SimpleAuthorize]
    public class SystemSettingController : Controller
    {
        private readonly ISystemSettingService systemSettingService;
        public SystemSettingController(ISystemSettingService systemSettingService)
        {
            this.systemSettingService = systemSettingService;
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
            SearchQuery<SystemSetting> searchQuery = new SearchQuery<SystemSetting>();
            searchQuery.Skip = pageRequest.DisplayStart;
            searchQuery.Take = pageRequest.DisplayLength;

            //add all search filterGroups
            if (!string.IsNullOrEmpty(pageRequest.Search))
            {
                searchQuery.AddFilter((e) => e.SettingTypeValue.Contains(pageRequest.Search) || e.Name.Contains(pageRequest.Search) || e.Value.Contains(pageRequest.Search));
            }

            //add all sort filterGroups
            var sortConditions = pageRequest.GetSortConditions();
            foreach (SortCondition condition in sortConditions)
            {
                FieldSortOrder<SystemSetting> fieldSortOrder = new FieldSortOrder<SystemSetting>();
                fieldSortOrder.Name = condition.ColumnName;
                fieldSortOrder.Direction = condition.SortDirection;

                //only sort on db column properties, ignore all custom/extended properties
                //for custom/extended properties, use manual sort/search after query result.
                searchQuery.AddSortCriteria(fieldSortOrder);

            }

            var members = systemSettingService.Search(searchQuery);

            Page<SystemSetting> petaPocoPage = new Page<SystemSetting>()
            {
                Items = members.Entities.ToList(),
                ItemsPerPage = pageRequest.DisplayLength,
                CurrentPage = pageRequest.DisplayStart,
                TotalItems = members.Count,
                TotalPages = members.Count / pageRequest.DisplayLength
            };
            var pageResponse = DataTablesFormat.SystemSettings(pageRequest, petaPocoPage);

            return Json(pageResponse);
        }

        //
        // GET: /SystemSetting/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SystemSetting/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SystemSetting/Create
        [HttpPost]
        public async Task<ActionResult> Create(SystemSetting systemSetting)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await systemSettingService.AddAsync(systemSetting);

                    string successMessage = string.Format("System Setting <b>{0}</b> created successfully", systemSetting.Name);
                    return RedirectToAction("Index", "SystemSetting", new { successNotification = Url.Encode(successMessage) });
                }
                else
                {
                    return View(systemSetting);
                }
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SystemSetting/Edit/5

        public ActionResult Edit(int id)
        {
            SystemSetting systemSetting = systemSettingService.GetSystemSettingById(id);
            return View(systemSetting);
        }

        //
        // POST: /SystemSetting/Edit/5

        [HttpPost]
        public async Task<ActionResult> Edit(int id, SystemSetting systemSetting)
        {
            try
            {
                await systemSettingService.UpdateAsync(systemSetting);

                string successMessage = string.Format("System Setting <b>{0}</b> updated successfully", systemSetting.Name);
                return RedirectToAction("Index", "SystemSetting", new { successNotification = Url.Encode(successMessage) });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            SystemSetting systemSetting = systemSettingService.GetSystemSettingById(id);
            return View(systemSetting);
        }

        //
        // POST: /SystemSetting/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, SystemSetting systemSetting)
        {
            try
            {
                await systemSettingService.DeleteAsync(id);

                string successMessage = string.Format("System Setting <b>{0}</b> deleted successfully", systemSetting.Name);
                return RedirectToAction("Index", "SystemSetting", new { successNotification = Url.Encode(successMessage) });
            }
            catch
            {
                return View();
            }
        }
    }
}
