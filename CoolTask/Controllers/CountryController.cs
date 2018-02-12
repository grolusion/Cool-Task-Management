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
using System.Data.Entity.Core.Common;

namespace CoolTaskManagement.Controllers
{
    public class CountryController : Controller
    {
        DataContext Context = new DataContext();
        //
        // GET: /Account/
        public ActionResult Index( int? id)
        {
            int userid =0;
            if (Convert.ToInt32(id)==0)
                userid = Convert.ToInt32(Session["UserID"]);
            else
                userid = Convert.ToInt32(id);
            Session["UserID"] = userid;


            //ViewBag.Companies = new SelectList(GetAllKesatuan(), "CompanyId", "CompanyName");
            //ViewBag.Countries = new SelectList(GetProvinsi(), "CountryID", "CountryName");
            //ViewBag.States = new SelectList(GetAllState(), "StateID", "StateName");
            //ViewBag.JobPositions = new SelectList(GetAllPosition(), "PositionId", "SebutanPangkat");
            //ViewBag.Tasks = new List<TaskViewModel>(GetTask(userid));
            //ViewBag.Users = GetUserLogin(userid);
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            int userid = 0;
            userid = Convert.ToInt32(Session["UserID"]);
            Session["UserID"] = userid;


            //ViewBag.Companies = new SelectList(GetAllKesatuan(), "CompanyId", "CompanyName");
            //ViewBag.Countries = new SelectList(GetProvinsi(), "CountryID", "CountryName");
            //ViewBag.States = new SelectList(GetAllState(), "StateID", "StateName");
            //ViewBag.JobPositions = new SelectList(GetAllPosition(), "PositionId", "SebutanPangkat");
            //ViewBag.Tasks = new List<TaskViewModel>(GetTask(userid));
            //ViewBag.Users = GetUserLogin(userid);
            return View();
        }
       

        // Populate Kesatuan List
        public List<Country> GetAllCountry()
        {
            using (DataContext dc = new DataContext())
            {
                return dc.Countries.OrderBy(a => a.CountryName).ThenBy(a => a.CountryID).ToList();
            }
        }

        public Country GetCountryByID(int id)
        {
            using (DataContext dc = new DataContext())
            {
                return dc.Countries.Where(a => a.CountryID.Equals(id)).FirstOrDefault();
            }
        }



        public JsonResult GetAllCountryJson()
        {
            using (DataContext dc = new DataContext())
            {
                    return new JsonResult { Data = GetAllCountry(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
               
            }
        }

        
        
    }
}