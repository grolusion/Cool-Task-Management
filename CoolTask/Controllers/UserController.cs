using CoolTaskManagement.DAL;
using CoolTaskManagement.DAL.Security;
using CoolTaskManagement.Models;
using CoolTaskManagement.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;
using System.Globalization;

namespace CoolTaskManagement.Controllers
{


    [CustomAuthorize(Roles = "Staff,Manager")]
    public class UserController : BaseController
    {
       
        JobDescriptionController jdController = new JobDescriptionController();
        CompanyController cpController = new CompanyController();
        CountryController cnController = new CountryController();
        StateController stController = new StateController();
        JobPositionController jpController = new JobPositionController();
        DataContext Context = new DataContext();
        [CustomAuthorize(Roles = "Staff,Manager")]
        public ActionResult Index()
        {

            User userdata = (User)Session["UserData"];
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;
            ViewBag.ListRecords = GetUserList(userdata.UserId);

            using (DataContext dc = new DataContext())
            {
                string userJobTitle = dc.Roles.Where(a => a.RoleId == userdata.JobPositionId).FirstOrDefault().RoleName.ToString().Trim().ToUpper();

                if (userJobTitle == "MANAGER")
                    ViewBag.ListRecords = GetAllUserList();
                else
                    ViewBag.ListRecords = GetUserListStaff(userdata.UserId);
            }
           
            return View();
        }
        [CustomAuthorize(Roles = "Staff,Manager")]
        public ActionResult Detail(int id)
        {
            User userdata = (User)Session["UserData"];
            Session["TaskID"] = Convert.ToInt32(id);
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;

            User user = new User();
            user = GetUserByUserId(id);
            ViewBag.Users = GetUserLogin(id);
            ViewBag.Companies = new SelectList(cpController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
            ViewBag.JobPositions = new SelectList(jpController.GetAllPosition(), "PositionId", "PositionTitle");

            return View(user);
        }
        [CustomAuthorize(Roles = "Manager")]
        public ActionResult Create(int id=0)
        {
            int userid = 0;
            userid = Convert.ToInt32(Session["UserID"]);
            UserLoginModel c = GetUserLogin(userid);
            int CompanyId = c.CompanyId;
            

            User userdata = (User)Session["UserData"];
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;

            
            ViewBag.Users = GetUserLogin(id);
            ViewBag.Companies = new SelectList(cpController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
            ViewBag.JobPositions = new SelectList(jpController.GetAllPosition(), "PositionId", "PositionTitle");
            return View();
       

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Manager")]
        public ActionResult Create(User c)
        {
            string message = "";
            bool status = false;
            int userid = 0;
            userid = Convert.ToInt32(Session["UserID"]);
            User u = (User)Session["UserData"];
            int CompanyId = u.CompanyID;

            Session["UserData"] = u;
            Session["UserID"] = u.UserId;

       

            User userdata = (User)Session["UserData"];
         
            if (c.City == null)
                c.City = "";
            if (c.ZipCode == null)
                c.ZipCode = "";
            if (c.ImgPath == null)
                c.ImgPath = "avatar.png";
            if (c.WebSiteUrl == null)
                c.WebSiteUrl = "";
            if (c.RememberMe == null)
                c.RememberMe = false;
        
            c.Password  = CoolTaskManagement.Helpers.MyHelpers.Encrypt("123");
            c.Password2 = CoolTaskManagement.Helpers.MyHelpers.Encrypt("123");
            c.CreateDate = DateTime.Now;
            c.UpdateDate = DateTime.Now;
            c.RememberMe = false;
            c.IsActive = true;
            if (c.Password != c.Password2)
            {
                    ViewBag.Users = ViewBag.Users = GetUserLogin(c.UserId);
                    ViewBag.Companies = new SelectList(cpController.GetAllCompany(), "CompanyId", "CompanyName");
                    ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");
                    ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
                    ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
                    ViewBag.JobPositions = new SelectList(jpController.GetAllPosition(), "PositionId", "PositionTitle");
                    return View(c);
            }
          
            
             using (DataContext dc = new DataContext())
                {
                    
                        //Update
                        var v = dc.Users.Where(a => a.UserId.Equals(c.UserId)).FirstOrDefault();
                        if (v == null)
                        {
                            string JobTitle = dc.JobPositions.Where(j => j.PositionId == c.JobPositionId).FirstOrDefault().PositionTitle.ToString().Trim();
                            c.Roles = new List<Role>();
                            List<Role> roleToAdd = dc.Roles.Where(a => a.RoleName == JobTitle).ToList<Role>();
                            foreach (var role in roleToAdd)
                            {
                                c.Roles.Add(role);
                            }
                            
                            dc.Users.Add(c);
                    }
                    if (ModelState.IsValid)
                    {
                        dc.Users.Add(c);
                        dc.SaveChanges();
                        status=true;
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
                    ViewBag.Users =  GetUserLogin(c.UserId);
                    ViewBag.Companies = new SelectList(cpController.GetAllCompany(), "CompanyId", "CompanyName");
                    ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");
                    ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
                    ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
                    ViewBag.JobPositions = new SelectList(jpController.GetAllPosition(), "PositionId", "PositionTitle");
                   
              
            }
           
            if (status)
                return RedirectToAction("Index");
            else
                return View(c); 
        }

         [CustomAuthorize(Roles = "Manager")]
        public ActionResult Delete(int id)
        {
            User userdata = (User)Session["UserData"];
            int CompanyId = userdata.CompanyID;
            Session["TaskID"] = Convert.ToInt32(id);
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;
            User user = null;
            user = GetUserByUserId(id);
            ViewBag.Companies = new SelectList(cpController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
            ViewBag.JobPositions = new SelectList(jpController.GetAllPosition(), "PositionId", "PositionTitle");

            return View(user);
        }

        //Delete POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [CustomAuthorize(Roles = "Manager")]
        public ActionResult DeleteConfirm(int id)
        {

            User userdata = (User)Session["UserData"];
            int CompanyId = userdata.CompanyID;
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;
            User user = null;
            user = GetUserByUserId(id);
            ViewBag.Companies = new SelectList(cpController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
            ViewBag.JobPositions = new SelectList(jpController.GetAllPosition(), "PositionId", "PositionTitle");

            using (DataContext dc = new DataContext())
            {
                var userRemove = dc.Users.Where(a => a.UserId.Equals(id)).FirstOrDefault();
                if (userRemove != null)
                {
                    dc.Users.Remove(userRemove);
                    dc.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound("Employee Not Found!");
                }
            }
        }
         [CustomAuthorize(Roles = "Manager")]
        public ActionResult Edit(int id)
        {
            User userdata = (User)Session["UserData"];
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;

            User user = new User();
            user = GetUserByUserId(id);
            ViewBag.Users = GetUserLogin(id);
            ViewBag.Companies = new SelectList(cpController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
            ViewBag.JobPositions = new SelectList(jpController.GetAllPosition(), "PositionId", "PositionTitle");

            if (user == null)
            {
                return HttpNotFound("Employee Not Found!");
            }

            return View(user);
        }

  

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Manager")]
        public ActionResult Edit(User c, HttpPostedFileBase file)
        
        {
            bool status = true;
            int userid = 0;
            userid = Convert.ToInt32(Session["UserID"]);
            User u = (User)Session["UserData"];
           
            Session["UserData"] = u;
            Session["UserID"] = u.UserId;

            User userdata = (User)Session["UserData"];
            
            using (DataContext dc = new DataContext())
                {
                    var v = dc.Users.Where(a => a.EmployeeNo.Equals(c.EmployeeNo)).FirstOrDefault();
                    if (v != null)
                    {

                       
                        
                        
                        if (c.JobPositionId != v.JobPositionId)
                        {

                            string JobTitle = dc.JobPositions.Where(j => j.PositionId == v.JobPositionId).FirstOrDefault().PositionTitle.ToString().Trim();
                            c.Roles = new List<Role>();
                            List<Role> roleToRemove = dc.Roles.Where(a => a.RoleName == JobTitle).ToList<Role>();
                            foreach (var role in roleToRemove)
                            {
                                v.Roles.Remove(role);
                            }

                            JobTitle = dc.JobPositions.Where(j => j.PositionId == c.JobPositionId).FirstOrDefault().PositionTitle.ToString().Trim();
                            c.Roles = new List<Role>();
                            List<Role> roleToAdd = dc.Roles.Where(a => a.RoleName == JobTitle).ToList<Role>();
                            foreach (var role in roleToAdd)
                            {
                                v.Roles.Add(role);
                            }
                            if (c.City == null)
                                c.City = "";
                            if (c.ZipCode == null)
                                c.ZipCode = "";
                            if (c.ImgPath == null)
                                c.ImgPath = "avatar.png";
                            if (c.WebSiteUrl == null)
                                c.WebSiteUrl = "";
                            if (c.RememberMe == null)
                                c.RememberMe = false;

                            v.Name = c.Name;
                            v.JobPositionId = c.JobPositionId;
                            v.Salary = c.Salary;
                            v.StateID = c.StateID;
                            v.TelephoneNo = c.TelephoneNo;
                            v.ZipCode = c.ZipCode;
                            v.Address = c.Address;
                            v.City = c.City;
                            v.CompanyID = c.CompanyID;
                            v.CountryId = c.CountryId;
                            v.DOB = c.DOB;
                            v.Email = c.Email;
                            v.EmployeeNo = c.EmployeeNo;
                            v.Address = c.Address;
                            v.UpdateDate = DateTime.Now;
                            if (v.Password == null)
                                v.Password = CoolTaskManagement.Helpers.MyHelpers.Encrypt("123");
                                v.Password2 = CoolTaskManagement.Helpers.MyHelpers.Encrypt("123");
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

                    }
                    else
                        status = false;

                }
           
            ViewBag.Users = GetUserLogin(c.UserId);
            ViewBag.Companies = new SelectList(cpController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(userdata.CompanyID), "JobDescId", "JobDescDescription");
            ViewBag.States = new SelectList(stController.GetAllState(), "StateID", "StateName");
            ViewBag.Countries = new SelectList(cnController.GetAllCountry(), "CountryID", "CountryName");
            ViewBag.JobPositions = new SelectList(jpController.GetAllPosition(), "PositionId", "PositionTitle");
                  
            if (status)
            
                     return RedirectToAction("Index");
            else
                        return View(c); 
        }
        public UserLoginModel GetUserLogin(int userId)
        {
            UserLoginModel user = null;
            using (DataContext dc = new DataContext())
            {
                var v = (from a in dc.Users
                         join b in dc.Companies on a.CompanyID equals b.CompanyId
                         join c in dc.States on a.StateID equals c.StateID
                         join d in dc.Countries on a.CountryId equals d.CountryID
                         join e in dc.JobPositions on a.JobPositionId equals e.PositionId
                         //where a.UserId.Equals(userId)
                         select new UserLoginModel
                         {
                             UserId = a.UserId,
                             EmployeeNo = a.EmployeeNo,
                             Name = a.Name,
                             Address = a.Address,
                             City = a.City,
                             State = c.StateName,
                             Country = d.CountryName,
                             ZipCode = a.ZipCode,
                             TelephoneNo = a.TelephoneNo,
                             Email = a.Email,
                             Password = a.Password,
                             WebSiteUrl = a.WebSiteUrl,
                             CompanyId = b.CompanyId,
                             CompanyName = b.CompanyName,
                             StateID = c.StateID,
                             CountryID = c.CountryID,
                             DOB = a.DOB,
                             Salary = a.Salary,
                             IsActive = a.IsActive,
                             JobPositionId = e.PositionId,
                             JobPositionTitle = e.PositionTitle,
                             ImgPath = a.ImgPath

                            

                         }).ToList();

                if (v != null)
                {
                    user = v.FirstOrDefault();
                }

            }
            return user;
        }

        public User GetUserByUserId(int userid)
        {
            using (DataContext dc = new DataContext())
            {
                return dc.Users.Where(a => a.UserId==userid).FirstOrDefault();
            }
        }
        public List<UserLoginModel> GetUserList(int userId)
        {
            List<UserLoginModel> user = null;
            using (DataContext dc = new DataContext())
            {
                user = (from a in dc.Users
                         join b in dc.Companies on a.CompanyID equals b.CompanyId
                         join c in dc.States on a.StateID equals c.StateID
                         join d in dc.Countries on a.CountryId equals d.CountryID
                         join e in dc.JobPositions on a.JobPositionId equals e.PositionId
                         where a.UserId.Equals(userId)
                         select new UserLoginModel
                         {
                             UserId = a.UserId,
                             EmployeeNo = a.EmployeeNo,
                             Name = a.Name,
                             Address = a.Address,
                             City = a.City,
                             State = c.StateName,
                             Country = d.CountryName,
                             ZipCode = a.ZipCode,
                             TelephoneNo = a.TelephoneNo,
                             Email = a.Email,
                             Password = a.Password,
                             WebSiteUrl = a.WebSiteUrl,
                             CompanyId = b.CompanyId,
                             CompanyName = b.CompanyName,
                             StateID = c.StateID,
                             CountryID = c.CountryID,
                             DOB =  a.DOB,
                             Salary = a.Salary,
                             IsActive = a.IsActive,
                             JobPositionId = e.PositionId,
                             JobPositionTitle = e.PositionTitle,
                             ImgPath =a.ImgPath

                         }).ToList();

              
            }
            return user;
        }

         [CustomAuthorize(Roles = "Manager")]
        public List<UserLoginModel> GetAllUserList()
        {
            List<UserLoginModel> user = null;
            using (DataContext dc = new DataContext())
            {
                user = (from a in dc.Users
                        join b in dc.Companies on a.CompanyID equals b.CompanyId
                        join c in dc.States on a.StateID equals c.StateID
                        join d in dc.Countries on a.CountryId equals d.CountryID
                        join e in dc.JobPositions on a.JobPositionId equals e.PositionId
                     
                        select new UserLoginModel
                        {
                            UserId = a.UserId,
                            EmployeeNo = a.EmployeeNo,
                            Name = a.Name,
                            Address = a.Address,
                            City = a.City,
                            State = c.StateName,
                            Country = d.CountryName,
                            ZipCode = a.ZipCode,
                            TelephoneNo = a.TelephoneNo,
                            Email = a.Email,
                            Password = a.Password,
                            WebSiteUrl = a.WebSiteUrl,
                            CompanyId = b.CompanyId,
                            CompanyName = b.CompanyName,
                            StateID = c.StateID,
                            CountryID = c.CountryID,
                            DOB = a.DOB,
                            Salary = a.Salary,
                            IsActive = a.IsActive,
                            JobPositionId = e.PositionId,
                            JobPositionTitle = e.PositionTitle,
                            ImgPath = a.ImgPath

                        }).ToList();


            }
            return user;
        }
        [CustomAuthorize(Roles = "Staff,Manager")]
        public List<UserLoginModel> GetUserListStaff(int userId)
        {
            List<UserLoginModel> user = null;
            using (DataContext dc = new DataContext())
            {
                User UserStaf = dc.Users.Where(u=>u.UserId==userId).FirstOrDefault();
                user = (from a in dc.Users
                        join b in dc.Companies on a.CompanyID equals b.CompanyId
                        join c in dc.States on a.StateID equals c.StateID
                        join d in dc.Countries on a.CountryId equals d.CountryID
                        join e in dc.JobPositions on a.JobPositionId equals e.PositionId
                        where  a.JobPositionId.Equals(UserStaf.JobPositionId )
                        select new UserLoginModel
                        {
                            UserId = a.UserId,
                            EmployeeNo = a.EmployeeNo,
                            Name = a.Name,
                            Address = a.Address,
                            City = a.City,
                            State = c.StateName,
                            Country = d.CountryName,
                            ZipCode = a.ZipCode,
                            TelephoneNo = a.TelephoneNo,
                            Email = a.Email,
                            Password = a.Password,
                            WebSiteUrl = a.WebSiteUrl,
                            CompanyId = b.CompanyId,
                            CompanyName = b.CompanyName,
                            StateID = c.StateID,
                            CountryID = c.CountryID,
                            DOB = a.DOB,
                            Salary = a.Salary,
                            IsActive = a.IsActive,
                            JobPositionId = e.PositionId,
                            JobPositionTitle = e.PositionTitle,
                            ImgPath = a.ImgPath

                        }).ToList();


            }
            return user;
        }
        
    }
}