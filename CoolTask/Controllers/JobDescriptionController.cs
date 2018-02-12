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
    public class JobDescriptionController : Controller
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
       

        // Populate Kesatuan List
        public List<JobDescription> GetAllJobDescription()
        {
            using (DataContext dc = new DataContext())
            {
                return dc.JobDescriptions.OrderBy(a => a.CompanyId).ThenBy(a => a.JobDescId).ToList();
            }
        }

        public List<JobDescription> GetJobDescriptionByCompany(int companyid)
        {
            using (DataContext dc = new DataContext())
            {
                return dc.JobDescriptions.Where(a => a.CompanyId.Equals(companyid)).OrderBy(a => a.JobDescCategory).ToList();
            }
        }


        public JobDescription GetJobDescriptionByID(int jobdescid)
        {
            using (DataContext dc = new DataContext())
            {
                return dc.JobDescriptions.Where(a => a.JobDescId.Equals(jobdescid)).FirstOrDefault();
            }
        }


      

        public JsonResult GetAllJobDescriptionJson()
        {
            using (DataContext dc = new DataContext())
            {

                return new JsonResult { Data = GetAllJobDescription(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        public JsonResult GetUserJobDescriptionJson(int userid)
        {
            using (DataContext dc = new DataContext())
            {
                User usercompany = dc.Users.Where(a => a.UserId == userid).FirstOrDefault();
                if (usercompany != null)
                {
                    return new JsonResult { Data = GetJobDescriptionByCompany(usercompany.CompanyID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else

                    return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        

        
    }
}