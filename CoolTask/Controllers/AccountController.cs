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
using CoolTaskManagement.Helpers;
using System.IO;
using System.Security.Cryptography;
namespace CoolTaskManagement.Controllers
{
    public class AccountController : Controller
    {
        DataContext Context = new DataContext();

        UserController uController = new UserController();
        CompanyController kController = new CompanyController();
        CountryController cController = new CountryController();
        StateController sController = new StateController();
        JobPositionController pController = new JobPositionController();

        //
        // GET: /Account/
        public ActionResult Index()
        {
            ViewBag.Companies = new SelectList(kController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.Countries = new SelectList(cController.GetAllCountry(), "CountryID", "CountryName");
            ViewBag.States = new SelectList(sController.GetAllState(), "StateID", "StateName");
            ViewBag.JobPositions = new SelectList(pController.GetAllPosition(), "PositionId", "PositionTitle");
          
     
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model, string returnUrl = "")
        {

            Session["EmployeeNo"] = model.EmployeeNo;
            Session["UserID"] = "0";
            Session["UserData"] = null;
            string pass = CoolTaskManagement.Helpers.MyHelpers.Encrypt(model.Password);
            if (ModelState.IsValid)
            {

                User user = Context.Users.Where(u => u.EmployeeNo == model.EmployeeNo && u.Password == pass).FirstOrDefault();
                
                if (user != null)
                {
                    Session["UserID"] = user.UserId.ToString();
                    Session["UserData"] = user;
                    Session.Timeout = 30;
                    var roles = user.Roles.Select(m => m.RoleName).ToArray();
                  
                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserId = user.UserId;
                    serializeModel.Name = user.Name;
                    serializeModel.PositionId = user.JobPositionId;
                    serializeModel.ImgPath = user.ImgPath;
                    serializeModel.roles = roles;
                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                            user.Email,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(60),
                             false,
                             userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                   
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (roles.Contains("Manager") || roles.Contains("Staff"))
                    {
                        return RedirectToAction("Graph", "Task");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Invalid EmployeeNo dan/atau password");
            }

            ViewBag.Companies = new SelectList(kController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.Countries = new SelectList(cController.GetAllCountry(), "CountryID", "CountryName");
            ViewBag.States = new SelectList(sController.GetAllState(), "StateID", "StateName");
            ViewBag.JobPositions = new SelectList(pController.GetAllPosition(), "PositionId", "PositionTitle");
            
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account", null);
        }


        private User GetUser(int userId)
        {
            User user = null;
            using (DataContext dc = new DataContext())
            {
                var v = (from a in dc.Users
                         join b in dc.Companies on a.CompanyID equals b.CompanyId
                         //join c in dc.Tasks on a.UserId equals c.UserId
                         where a.UserId.Equals(userId)
                         select new
                         {
                             a,
                             b
                         }).FirstOrDefault();
                if (v != null)
                {
                    user = v.a;

                }
                return user;
            }
        }

        public JsonResult GetAllStateUser(int countryID)
        {
            using (DataContext dc = new DataContext())
            {

                return new JsonResult { Data = sController.GetStateByCountry(countryID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult Register(int id = 0)
        {

            if (id > 0)
            {
                //Update
                var c = GetUser(id);
                if (c != null)
                {
                    ViewBag.Companies = new SelectList(kController.GetAllCompany(), "CompanyId", "CompanyName");
                    ViewBag.Countries = new SelectList(cController.GetAllCountry(), "CountryID", "CountryName");
                    ViewBag.States = new SelectList(sController.GetStateByCountry(c.CountryId), "StateID", "StateName");
                    ViewBag.JobPositions = new SelectList(pController.GetAllPosition(), "PositionId", "PositionTitle");
     
                }
                else
                {
                    return HttpNotFound();
                }
                return View(c);
            }
            else
            {
                    ViewBag.Companies = new SelectList(kController.GetAllCompany(), "CompanyId", "CompanyName");
                    ViewBag.Countries = new SelectList(cController.GetAllCountry(), "CountryID", "CountryName");
                    ViewBag.States = new SelectList(sController.GetAllState(), "StateID", "StateName");
                    ViewBag.JobPositions = new SelectList(pController.GetAllPosition(), "PositionId", "PositionTitle");
        
     
                return View();
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User c)
        {
            string message = "";
            bool status = false;
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
            if (c.Password != c.Password2)
            {
                message = "Password tidak sama...";
                ViewBag.Companies = new SelectList(kController.GetAllCompany(), "CompanyId", "CompanyName");
                ViewBag.Countries = new SelectList(cController.GetAllCountry(), "CountryID", "CountryName");
                ViewBag.States = new SelectList(sController.GetAllState(), "StateID", "StateName");
                ViewBag.JobPositions = new SelectList(pController.GetAllPosition(), "PositionId", "PositionTitle");
                ViewBag.Message = message;
                ViewBag.Status = status;
                return View(c);
        
       
         
            }
            if (ModelState.IsValid)
            {
                using (DataContext dc = new DataContext())
                {
                    if (c.UserId > 0)
                    {
                        //Update
                        var v = dc.Users.Where(a => a.UserId.Equals(c.UserId)).FirstOrDefault();
                        if (v != null)
                        {
                          
                            c.Password = CoolTaskManagement.Helpers.MyHelpers.Encrypt(c.Password);
                            c.Password2 = CoolTaskManagement.Helpers.MyHelpers.Encrypt(c.Password);
                            c.UpdateDate = DateTime.Now;
                            
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                    else
                    {

                        if (c != null)
                        {
                            string JobTitle = dc.JobPositions.Where(j=>j.PositionId == c.JobPositionId).FirstOrDefault().PositionTitle.ToString().Trim();
                            c.Roles = new List<Role>();
                            List<Role> roleToAdd = dc.Roles.Where(a =>a.RoleName == JobTitle).ToList<Role>();
                            foreach (var role in roleToAdd)
                            {
                                c.Roles.Add(role);
                            }
                        }
                        //Add new 
                        c.Password = CoolTaskManagement.Helpers.MyHelpers.Encrypt(c.Password);
                        c.Password2 = CoolTaskManagement.Helpers.MyHelpers.Encrypt(c.Password);
                        c.CreateDate = DateTime.Now;
                        c.UpdateDate = DateTime.Now;
                        c.RememberMe = false;
                        c.IsActive = true;
                        
                       
               
                        dc.Users.Add(c);
                    }
                    dc.SaveChanges();
                    status = true;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                message = "Error! Please try again.";
                ViewBag.Companies = new SelectList(kController.GetAllCompany(), "CompanyId", "CompanyName");
                ViewBag.Countries = new SelectList(cController.GetAllCountry(), "CountryID", "CountryName");
                ViewBag.States = new SelectList(sController.GetStateByCountry(c.CountryId), "StateID", "StateName");
                ViewBag.JobPositions = new SelectList(pController.GetAllPosition(), "PositionId", "PositionTitle");
    
        
            }
            ViewBag.Message = message;
            ViewBag.Status = status;
            return View(c);
        }
        
    }
}