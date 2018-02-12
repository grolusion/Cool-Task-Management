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
    public class StateController : Controller
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
            //ViewBag.JobPositions = new SelectList(GetAllPosition(), "PositionId", "PositionTitle");
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
            //ViewBag.JobPositions = new SelectList(GetAllPosition(), "PositionId", "PositionTitle");
            //ViewBag.Tasks = new List<TaskViewModel>(GetTask(userid));
            //ViewBag.Users = GetUserLogin(userid);
            return View();
        }

        public List<State> GetAllState()
        {
            using (DataContext dc = new DataContext())
            {
                return dc.States.OrderBy(a => a.StateName).ToList();
            }

        }
        public List<State> GetStateByCountry(int countryID)
        {
            using (DataContext dc = new DataContext())
            {
                return dc.States.Where(a => a.CountryID.Equals(countryID)).OrderBy(a => a.CountryID).ThenBy(a => a.StateName).ToList();
            }
        }


        public JsonResult GetAllStateJson()
        {
            using (DataContext dc = new DataContext())
            {

                return new JsonResult { Data = GetAllState(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public JsonResult GetStateByCountryJson(int countryID)
        {
            using (DataContext dc = new DataContext())
            {

                return new JsonResult { Data = GetStateByCountry( countryID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


       
        
    }
}