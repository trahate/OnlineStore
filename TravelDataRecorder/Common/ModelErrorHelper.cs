using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelDataRecorder.Common
{
    public class ModelErrorHelper
    {
        public static List<string> GetModelError(ViewDataDictionary modalData)
        {
            List<string> lstError = new List<string>();
            foreach (System.Web.Mvc.ModelState modelState in modalData.ModelState.Values)
            {
                foreach (System.Web.Mvc.ModelError error in modelState.Errors)
                {
                    lstError.Add(error.ErrorMessage);
                }
            }
            return lstError;
        }

        public static List<string> GetModelError(ICollection<System.Web.Mvc.ModelState> values)
        {
            List<string> lstError = new List<string>();
            foreach (System.Web.Mvc.ModelState modelState in values)
            {
                foreach (System.Web.Mvc.ModelError error in modelState.Errors)
                {
                    lstError.Add(error.ErrorMessage);
                }
            }
            return lstError;
        }
    }
}