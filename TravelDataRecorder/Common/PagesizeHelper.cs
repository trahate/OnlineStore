using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelDataRecorder.Model;

namespace TravelDataRecorder.Common
{
    public class PagesizeHelper
    {
        public static List<SelectListItem> GetPageSizeList()
        {
            return new List<SelectListItem>()
            {
                 new SelectListItem() { Value="5", Text= "5" },
                 new SelectListItem() { Value="10", Text= "10" },
                 new SelectListItem() { Value="15", Text= "15" },
                 new SelectListItem() { Value="25", Text= "25" },
                 new SelectListItem() { Value="50", Text= "50" },
             };
        }

        public static List<SelectListItem> GetAllDesignatedUsers(List<TravelRoleModel> data)
        {
            List<SelectListItem> lstRoles = new List<SelectListItem>();
            foreach (var item in data)
            {
                lstRoles.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Name });
            }
            return lstRoles;
        }
        public static List<SelectListItem> All()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem() { Value="All",Text="All"}
            };
        }

        public static int NumberOfRecords
        {
            get
            {
                return 10;
            }
            set
            {
            }
        }
    }
}