using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelDataRecorder.Model.ViewModel;

namespace TravelDataRecorder.Common
{
    public class CommonHelper
    {
        public static SearchFilter GetSearchFilter()
        {
            SearchFilter searchFilter = new SearchFilter();
            if (HttpContext.Current.Session["searchFilter"] != null)
            {
                searchFilter = (SearchFilter)HttpContext.Current.Session["searchFilter"];
            }
            return searchFilter;
        }
    }
}