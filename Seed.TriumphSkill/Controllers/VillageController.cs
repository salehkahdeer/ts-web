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
    public class VillageController : Controller
    {
        private readonly IVillageService villageService;
        public VillageController(IVillageService villageService)
        {
            this.villageService = villageService;
        }

        public JsonResult Statistics()
        {
            Statistics statistics = new Statistics();
            statistics.Kpi = villageService.All().Count().ToString();
            statistics.Link = "http://www.celusion.com";

            return Json(statistics, JsonRequestBehavior.AllowGet);
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
            SearchQuery<Village> searchQuery = new SearchQuery<Village>();
            searchQuery.Skip = pageRequest.DisplayStart;
            searchQuery.Take = pageRequest.DisplayLength;

            //add all search filterGroups
            if (!string.IsNullOrEmpty(pageRequest.Search))
            {
                searchQuery.AddFilter((e) => e.Name.Contains(pageRequest.Search) || e.Code.Contains(pageRequest.Search));
            }

            //add all sort filterGroups
            var sortConditions = pageRequest.GetSortConditions();
            foreach (SortCondition condition in sortConditions)
            {
                FieldSortOrder<Village> fieldSortOrder = new FieldSortOrder<Village>();
                fieldSortOrder.Name = condition.ColumnName;
                fieldSortOrder.Direction = condition.SortDirection;

                //only sort on db column properties, ignore all custom/extended properties
                //for custom/extended properties, use manual sort/search after query result.
                searchQuery.AddSortCriteria(fieldSortOrder);
                
            }

            var members = villageService.Search(searchQuery);

            Page<Village> petaPocoPage = new Page<Village>()
            {
                Items = members.Entities.ToList(),
                ItemsPerPage = pageRequest.DisplayLength,
                CurrentPage = pageRequest.DisplayStart,
                TotalItems = members.Count,
                TotalPages = members.Count / pageRequest.DisplayLength
            };
            var pageResponse = DataTablesFormat.Villages(pageRequest, petaPocoPage);

            return Json(pageResponse);
        }

        //
        // GET: /Village/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Village/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Village/Create
        [HttpPost]
        public async Task<ActionResult> Create(Village village)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await villageService.AddAsync(village);

                    string successMessage = string.Format("Village <b>{0}</b> created successfully", village.Name);
                    return RedirectToAction("Index", "Village", new { successNotification = Url.Encode(successMessage) });
                }
                else
                {
                    return View(village);
                }
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Village/Edit/5

        public ActionResult Edit(int id)
        {
            Village village = villageService.GetVillageById(id);
            return View(village);
        }

        //
        // POST: /Village/Edit/5

        [HttpPost]
        public async Task<ActionResult> Edit(int id, Village village)
        {
            try
            {
                await villageService.UpdateAsync(village);

                string successMessage = string.Format("Village <b>{0}</b> updated successfully", village.Name);
                return RedirectToAction("Index", "Village", new { successNotification = Url.Encode(successMessage) });                
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            Village village = villageService.GetVillageById(id);
            return View(village);
        }

        //
        // POST: /Village/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, Village village)
        {
            try
            {                
                await villageService.DeleteAsync(id);

                string successMessage = string.Format("Village <b>{0}</b> deleted successfully", village.Name);
                return RedirectToAction("Index", "Village", new { successNotification = Url.Encode(successMessage) });                
            }
            catch
            {
                return View();
            }
        }
    }
}
