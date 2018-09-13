using Seed.TriumphSkill.Models.DataTables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Seed.TriumphSkill.ViewModels
{
    public class DataTablesFormat
    {
        public static object Users(DataTablesPageRequest pageRequest, Page<Models.User> report)
        {
            return new
            {
                sEcho = pageRequest.Echo,
                iTotalRecords = report.TotalItems,
                iTotalDisplayRecords = report.TotalItems,
                sColumns = pageRequest.ColumnNames,
                aaData = (from i in report.Items
                          select new[]
			       		    {	
                                i.ID.ToString(CultureInfo.InvariantCulture),
			       		        i.Name,
                                i.Username,
                                i.Email
			       		    }).ToList()
            };
        }

        public static object Roles(DataTablesPageRequest pageRequest, Page<Models.Role> report)
        {
            return new
            {
                sEcho = pageRequest.Echo,
                iTotalRecords = report.TotalItems,
                iTotalDisplayRecords = report.TotalItems,
                sColumns = pageRequest.ColumnNames,
                aaData = (from i in report.Items
                          select new[]
			       		    {	
                                i.ID.ToString(CultureInfo.InvariantCulture),
			       		        i.Name
			       		    }).ToList()
            };
        }

        public static object SystemSettings(DataTablesPageRequest pageRequest, Page<Models.SystemSetting> report)
        {
            return new
            {
                sEcho = pageRequest.Echo,
                iTotalRecords = report.TotalItems,
                iTotalDisplayRecords = report.TotalItems,
                sColumns = pageRequest.ColumnNames,
                aaData = (from i in report.Items
                          select new[]
			       		    {	
                                i.ID.ToString(CultureInfo.InvariantCulture),
                                i.SettingTypeValue,
			       		        i.Name,
                                i.Value
			       		    }).ToList()
            };
        }

        public static object Villages(DataTablesPageRequest pageRequest, Page<Models.Village> report)
        {
            return new
            {
                sEcho = pageRequest.Echo,
                iTotalRecords = report.TotalItems,
                iTotalDisplayRecords = report.TotalItems,
                sColumns = pageRequest.ColumnNames,
                aaData = (from i in report.Items
                          select new[]
			       		    {	
                                i.ID.ToString(CultureInfo.InvariantCulture),
			       		        i.Code,
                                i.Name
			       		    }).ToList()
            };
        }
    }
}