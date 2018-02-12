using CoolTaskManagement.DAL;
using CoolTaskManagement.DAL.Security;
using CoolTaskManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;

namespace CoolTaskManagement.Controllers
{
    public class GMapController : Controller
    {
        DataContext Context = new DataContext();
       

        //3. GetMarkerInfo - for fetch location details from database for show marker in the map
        public JsonResult GetMarkerInfo(int CompanyId)
        {
            //using (DataContext dc = new DataContext())
            {
                Company l = null;
                l = Context.Companies.Where(a => a.CompanyId.Equals(CompanyId)).FirstOrDefault();
                return new JsonResult { Data = l, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

    }
}