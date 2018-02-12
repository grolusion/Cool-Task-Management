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
    public class CompanyController : Controller
    {

        CountryController cnController = new CountryController();
        StateController stController = new StateController();
        DataContext Context = new DataContext();
        
        [CustomAuthorize(Roles = "Manager,Staff")]
        public ActionResult Index()
        {

            User userdata = (User)Session["UserData"];
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;
            using (DataContext dc = new DataContext())
            {
                string userJobTitle = dc.Roles.Where(a => a.RoleId == userdata.JobPositionId).FirstOrDefault().RoleName.ToString().Trim().ToUpper();

                if (userJobTitle == "MANAGER")
                    ViewBag.ListRecords = GetAllCompany();
                else
                    ViewBag.ListRecords = GetCompanyByUser(userdata.UserId);
            }
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
            return View();
        }

        public ActionResult Detail(int id)
        {
            User userdata = (User)Session["UserData"];
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;

            Company company = new Company();
            company = GetCompanyByID(id);
            ViewBag.ListRecords = GetCompanyByUser(userdata.UserId);
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");

            return View(company);
        }

        [CustomAuthorize(Roles = "Manager")]
        public ActionResult Create(int id = 0)
        {
            int userid = 0;
            userid = Convert.ToInt32(Session["UserID"]);
         
            User userdata = (User)Session["UserData"];
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;

            ViewBag.ListRecords = GetAllCompany();
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
            return View();


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Manager")]
        public ActionResult Create(Company c)
        {
            string message = "";
            bool status = false;
            int userid = 0;
            userid = Convert.ToInt32(Session["UserID"]);
            User u = (User)Session["UserData"];
            Session["UserData"] = u;
            Session["UserID"] = u.UserId;
            User userdata = (User)Session["UserData"];

                        if (c.CompanyDescription != null)
                                        c.CompanyDescription = "";
                        if (c.CompanyLat == null)
                            c.CompanyLat = 0;
                        if (c.CompanyLong == null)
                            c.CompanyLong = 0;
                        if (c.CompanyAddress == null)
                            c.CompanyAddress = "";
                        if (c.CompanyCity == null)
                            c.CompanyCity = "";
                        if (c.CompanyZipCode == null)
                            c.CompanyZipCode = "";
                        if (c.CompanyTelephoneNo == null)
                            c.CompanyTelephoneNo = "";
                        c.IsActive=true;
                        c.DateUpdated = DateTime.Now;

         
          

            using (DataContext dc = new DataContext())
            {

                
                var v = dc.Companies.Where(a => a.CompanyName.Equals(c.CompanyName)).FirstOrDefault();
                if (v == null)
                {

                    if (ModelState.IsValid)
                    {
                        
                        dc.Companies.Add(c);
                        dc.SaveChanges();
                        status = true;
                    }
                    else
                    {
                        //
                        var modelErrors = new List<string>();
                        foreach (var modelState in ModelState.Values)
                        {
                            foreach (var modelError in modelState.Errors)
                            {
                                modelErrors.Add(modelError.ErrorMessage);
                            }
                        }

                    }
                }
           }
            ViewBag.ListRecords = GetAllCompany();
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
            if (status)
                return RedirectToAction("Index");
            else
                return View(c);
        }

        [CustomAuthorize(Roles = "Manager")]
        public ActionResult Delete(int id)
        {
            User userdata = (User)Session["UserData"];
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;
            Company company = null;
            company= GetCompanyByID(id);
            ViewBag.ListRecords = GetAllCompany();
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");

            return View(company);
        }

        //Delete POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [CustomAuthorize(Roles = "Manager")]
        public ActionResult DeleteConfirm(int id)
        {

            User userdata = (User)Session["UserData"];
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;
            Company company = null;
            company = GetCompanyByID(id);
           
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
            ViewBag.ListRecords = GetAllCompany();
            using (DataContext dc = new DataContext())
            {
                 var companyRemove = dc.Companies.Where(a => a.CompanyId.Equals(id)).FirstOrDefault();
                if (companyRemove != null)
                {
                    dc.Companies.Remove(companyRemove);
                    dc.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound("Company Not Found!");
                }
            }
        }

        [CustomAuthorize(Roles = "Manager")]
        public ActionResult Edit(int id)
        {
            User userdata = (User)Session["UserData"];
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;
            Company company = null;
            company = GetCompanyByID(id);
            ViewBag.ListRecords = GetAllCompany();
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");

            if (company == null)
            {
                return HttpNotFound("Company Not Found!");
            }

            return View(company);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Manager")]
        public ActionResult Edit(Company c, HttpPostedFileBase file)
        {
            bool status=false ;
            User userdata = (User)Session["UserData"];
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;
          
            ViewBag.ListRecords = GetAllCompany();
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");

            using (DataContext dc = new DataContext())
            {
                var v = dc.Companies.Where(a => a.CompanyId==c.CompanyId).FirstOrDefault();
                if (v != null)
                {

                    
                    v.CompanyDescription = c.CompanyDescription;
                    v.CompanyLat = c.CompanyLat;
                    v.CompanyLong = c.CompanyLong;
                    v.CompanyAddress = c.CompanyAddress;
                    v.CompanyCity = c.CompanyCity;
                    v.CompanyZipCode = c.CompanyZipCode;
                    v.CompanyTelephoneNo = c.CompanyTelephoneNo;
                    v.DateUpdated=DateTime.Now;

                 
                    if (ModelState.IsValid)
                        {

                            dc.SaveChanges();
                            status = true;
                        }
                        else
                        {

                            status = false;
                            var modelErrors = new List<string>();
                            foreach (var modelState in ModelState.Values)
                            {
                                foreach (var modelError in modelState.Errors)
                                {
                                    modelErrors.Add(modelError.ErrorMessage);
                                }
                            }
                        }
                    }
                else
                    status = false;

            }

            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
            
            if (status)

                return RedirectToAction("Index");
            else
                return View(c);
        }
        public List<Company> GetAllCompany()
        {
            using (DataContext dc = new DataContext())
            {
                return dc.Companies.OrderBy(a => a.CompanyName).ToList();
            }
        }

        public Company GetCompanyByID(int companyid)
        {
            using (DataContext dc = new DataContext())
            {

                return dc.Companies.Where(a => a.CompanyId.Equals(companyid)).FirstOrDefault();
            }
        }


        public Company GetCompanyByUser(int userid)
        {
            using (DataContext dc = new DataContext())
            {
                int companyuser = dc.Users.Where ( a=> a.UserId == userid).FirstOrDefault().CompanyID;
                return dc.Companies.Where(a => a.CompanyId.Equals(companyuser)).FirstOrDefault();
            }
        }

        public JsonResult GetJobByCompanyJson(int CompanyId)
        {
            using (DataContext dc = new DataContext())
            {

                return new JsonResult { Data = GetCompanyByID(CompanyId), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

    }
}