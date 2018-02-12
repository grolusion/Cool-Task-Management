using CoolTaskManagement.DAL;
using CoolTaskManagement.DAL.Security;
using CoolTaskManagement.Models;
using CoolTaskManagement.Controllers;
using CoolTaskManagement.Helpers ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;
using System.Data.Entity.Core.Common;
using System.Globalization;



namespace CoolTaskManagement.Controllers
{
    public class TaskController : Controller
    {
        JobDescriptionController jdController = new JobDescriptionController();
        UserController uController = new UserController();
        CompanyController kController = new CompanyController();
        CountryController cController = new CountryController();
        StateController sController = new StateController();
        JobPositionController pController = new JobPositionController();
        DataContext Context = new DataContext();
        //
        // GET: /Tast/
        public ActionResult Index()
        {
            User userdata = (User)Session["UserData"];
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;
            ViewBag.Tasks = GetTask(userdata.UserId);
            ViewBag.Graph = GetSumNilaiByUser();
            return View();
        }


        public List<TaskViewModel> GetTask(int userId)
        {
            List<TaskViewModel> retunVal = new List<TaskViewModel>();
            using (DataContext dc = new DataContext())
            {
                //retunVal = dc.Tasks.Where(t => t.UserId == userId).OrderBy(t => t.AssignDate).ThenBy(t => t.TaskID).ToList<Task>();
                var firstQuery = (from u in dc.Users
                                  join k in dc.Companies on u.CompanyID equals k.CompanyId
                                  where u.UserId == userId
                                  select new
                                  {
                                      u.UserId,
                                      u.EmployeeNo,
                                      u.Name,
                                      u.JobPositionId,
                                      k.CompanyName
                                  }
                                    ).AsQueryable();

                var secondQuery = (from t in dc.Tasks
                                   join j in dc.JobDescriptions on t.JobDescTypeID equals j.JobDescId
                                   select new
                                   {
                                       t.UserId,
                                       t.EmployeeNo,
                                       t.JobDescTypeID,
                                       j.JobDescCategory,
                                       t.TaskID,
                                       t.AssignDate,
                                       t.StartingDate,
                                       t.FinishingDate,
                                       t.JobTypeDescription,
                                       t.TaskDescription,
                                       t.TaskResultNotes,
                                       t.TaskEvaluationNotes,
                                       t.Status,
                                       t.Progress,
                                       t.CreditQuality,
                                       t.QualityResult,
                                       t.ApprovedStatus,
                                       t.ApprovedBy,
                                       t.ApprovedDate
                                   }
                                   ).AsQueryable();

                retunVal = (from fq in firstQuery.DefaultIfEmpty()
                            join sq in secondQuery on fq.UserId equals sq.UserId
                            select new TaskViewModel
                             {
                                 UserId = sq.UserId,
                                 EmployeeNo = fq.EmployeeNo,
                                 Name = fq.Name,
                                 PositionId = fq.JobPositionId,
                                 TypeID = sq.JobDescTypeID,
                                 TypeDescription = sq.JobTypeDescription,
                                 TaskID = sq.TaskID,
                                 AssignDate = sq.AssignDate,
                                 StartingDate = sq.StartingDate,
                                 FinishingDate = sq.FinishingDate,
                                 TaskDescription = sq.TaskDescription,
                                 TaskResultNotes = sq.TaskResultNotes,
                                 TaskEvaluationNotes = sq.TaskEvaluationNotes,
                                 Status = sq.Status,
                                 Progress = sq.Progress,
                                 CreditQuality = sq.CreditQuality,
                                 QualityResult = sq.QualityResult,
                                 ApprovedStatus = sq.ApprovedStatus,
                                 ApprovedBy = sq.ApprovedBy,
                                 ApprovedDate = sq.ApprovedDate
                             }).ToList();


            }

            return retunVal;

        }

        private TaskViewModel GetViewTaskByID(int taskID)
        {
            TaskViewModel retunVal = new TaskViewModel();
            using (DataContext dc = new DataContext())
            {
                //retunVal = dc.Tasks.Where(t => t.UserId == userId).OrderBy(t => t.AssignDate).ThenBy(t => t.TaskID).ToList<Task>();



                var secondQuery = (from t in dc.Tasks
                                   join j in dc.JobDescriptions on t.JobDescTypeID equals j.JobDescId
                                   where t.TaskID == taskID
                                   select new
                                   {
                                       t.UserId,
                                       t.EmployeeNo,
                                       t.JobDescTypeID,
                                       j.JobDescCategory,
                                       t.TaskID,
                                       t.AssignDate,
                                       t.StartingDate,
                                       t.FinishingDate,
                                       t.JobTypeDescription,
                                       t.TaskDescription,
                                       t.TaskResultNotes,
                                       t.TaskEvaluationNotes,
                                       t.Status,
                                       t.Progress,
                                       t.CreditQuality,
                                       t.QualityResult,
                                       t.ApprovedStatus,
                                       t.ApprovedBy,
                                       t.ApprovedDate
                                   }
                                   ).AsQueryable();
                var firstQuery = (from u in dc.Users
                                  join k in dc.Companies on u.CompanyID equals k.CompanyId
                                  where u.UserId == secondQuery.FirstOrDefault().UserId
                                  select new
                                  {
                                      u.UserId,
                                      u.EmployeeNo,
                                      u.Name,
                                      u.JobPositionId,
                                      k.CompanyName
                                  }
                                   ).AsQueryable();
                retunVal = (from fq in firstQuery.DefaultIfEmpty()
                            join sq in secondQuery on fq.UserId equals sq.UserId
                            select new TaskViewModel
                             {
                                 UserId = sq.UserId,
                                 EmployeeNo = fq.EmployeeNo,
                                 Name = fq.Name,
                                 CompanyName = fq.CompanyName,
                                 
                                 TypeID = sq.JobDescTypeID,
                                 TypeDescription = sq.JobTypeDescription,
                                 TaskID = sq.TaskID,
                                 AssignDate = sq.AssignDate,
                                 StartingDate = sq.StartingDate,
                                 FinishingDate = sq.FinishingDate,
                                 TaskDescription = sq.TaskDescription,
                                 TaskResultNotes = sq.TaskResultNotes.ToString(),
                                 TaskEvaluationNotes = sq.TaskEvaluationNotes,
                                 Status = sq.Status,
                                 Progress = sq.Progress,
                                 CreditQuality = sq.CreditQuality,
                                 QualityResult = sq.QualityResult,
                                 ApprovedStatus = sq.ApprovedStatus,
                                 ApprovedBy = sq.ApprovedBy,
                                 ApprovedDate = sq.ApprovedDate
                             }).FirstOrDefault();



            }

            return retunVal;

        }



        private List<Task> GetTaskByUserId(int userID)
        {
            using (DataContext dc = new DataContext())
            {
                return dc.Tasks.Where(a => a.UserId.Equals(userID)).OrderBy(a => a.TaskID).ToList();
            }
        }

        public List<TaskSeriesModel> GetSumNilaiByUser()
        {
            User userdata = (User)Session["UserData"];
            int CompanyId = userdata.CompanyID;

            List<Task> task = null;
            task = GetTaskByUserId(userdata.UserId);


            List<TaskSeriesModel> result = (from t in task
                                            join j in Context.JobDescriptions on t.JobDescTypeID equals j.JobDescId
                                            group t by new
                                            {
                                                Bulan = t.AssignDate.Month,
                                                Tahun = t.AssignDate.Year,
                                                Jenis = j.JobDescCategory
                                            } into g
                                            select new TaskSeriesModel
                                      {

                                          Tanggal = DateTime.ParseExact(string.Format("01/{0}/{1}", g.Key.Bulan.ToString("D2"), g.Key.Tahun), "dd/mm/yyyy", CultureInfo.InvariantCulture),
                                          StatusName = g.Key.Jenis == 1 ? "Reguler" :
                                                          g.Key.Jenis == 2 ? "Tambahan" :
                                                          g.Key.Jenis == 3 ? "Khusus" :
                                                          g.Key.Jenis == 4 ? "Lainnya" : "-",


                                          Nilai = g.Sum(x => x.CreditQuality * x.Progress)
                                      }
                         ).ToList();


            return result;
        }

        private Task GetTaskByID(int taskID)
        {
            using (DataContext dc = new DataContext())
            {
                return dc.Tasks.Where(a => a.TaskID.Equals(taskID)).FirstOrDefault();
            }
        }
        public JsonResult GetTaskJson(int userID)
        {
            using (DataContext dc = new DataContext())
            {

                return new JsonResult { Data = GetTask(userID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public JsonResult GetTaskByIDJson(int id)
        {
            using (DataContext dc = new DataContext())
            {

                return new JsonResult { Data = GetViewTaskByID(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        public ActionResult GetTaskByUser(int userID)
        {

            List<TaskViewModel> returnValue = GetTask(userID);

            return PartialView(returnValue);

        }

        public ActionResult Detail(int id)
        {
            User userdata = (User)Session["UserData"];
            int CompanyId = userdata.CompanyID;
            Session["TaskID"] = Convert.ToInt32(id);
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;

            Task task = new Task();
            task = GetTaskByID(id);
            ViewBag.Tasks = GetViewTaskByID(id);
            ViewBag.Companies = new SelectList(kController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");
            ViewBag.ApprovedStatus = new List<SelectListItem>(ApprovedStatus());

            ViewBag.ProgressStatus = ProgressStatus();
            ViewBag.ApprovedStatus = ApprovedStatus();

            return View(task);
        }


        public ActionResult Graph()
        {
            User userdata = (User)Session["UserData"];
            int CompanyId = userdata.CompanyID;
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;
            List<Task> task = null;
            task = GetTaskByUserId(userdata.UserId);

            List<TaskSeriesModel> result = (from t in task
                                            join j in Context.JobDescriptions on t.JobDescTypeID equals j.JobDescId
                                            group t by new
                                            {
                                                Bulan = t.AssignDate.Month,
                                                Tahun = t.AssignDate.Year,
                                                Jenis = j.JobDescCategory
                                            } into g
                                            select new TaskSeriesModel
                                            {

                                                Tanggal = DateTime.ParseExact(string.Format("01/{0}/{1}", g.Key.Bulan.ToString("D2"), g.Key.Tahun), "dd/mm/yyyy", CultureInfo.InvariantCulture),
                                                StatusName = g.Key.Jenis == 1 ? "Reguler" :
                                                                g.Key.Jenis == 2 ? "Tambahan" :
                                                                g.Key.Jenis == 3 ? "Khusus" :
                                                                g.Key.Jenis == 4 ? "Lainnya" : "-",


                                                Nilai = g.Sum(x => x.CreditQuality * x.Progress)
                                            }
                         ).ToList();

            ViewBag.Graph = result;


            List<Point> dataPoints = (from t in task
                                  join j in Context.JobDescriptions on t.JobDescTypeID equals j.JobDescId
                                  group t by new
                                  {
                                      Bulan = t.AssignDate.Month,
                                      Tahun = t.AssignDate.Year,
                                      //Jenis = j.Jenis
                                  } into g
                                  select new Point
                                  {

                                     y = DateTime.ParseExact(string.Format("01/{0}/{1}", g.Key.Bulan.ToString("D2"), g.Key.Tahun), "dd/mm/yyyy", CultureInfo.InvariantCulture),
                                      //StatusName = g.Key.Jenis == 1 ? "Reguler" :
                                      //                g.Key.Jenis == 2 ? "Tambahan" :
                                      //                g.Key.Jenis == 3 ? "Khusus" :
                                      //                g.Key.Jenis == 4 ? "Lainnya" : "-",


                                      x = Convert.ToInt32( g.Sum(x => x.CreditQuality * x.Progress))
                                  }
                         ).ToList();

            JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints, _jsonSetting);


            return View();
        }
      
        public JsonResult GetLineChart()
        {




            User userdata = (User)Session["UserData"];
            int CompanyId = userdata.CompanyID;

            List<Task> task = GetTaskByUserId(userdata.UserId);

            List<TaskSeriesModel> result = (from t in task
                                            join j in Context.JobDescriptions on t.JobDescTypeID equals j.JobDescId
                                            group t by new
                                            {
                                                Jenis = j.JobDescCategory,
                                                Bulan = t.AssignDate.Month,
                                                Tahun = t.AssignDate.Year
                                               
                                            } into g
                                            select new TaskSeriesModel
                                            {

                                                StatusName = g.Key.Jenis == 1 ? "Reguler" :
                                                                g.Key.Jenis == 2 ? "Tambahan" :
                                                                g.Key.Jenis == 3 ? "Khusus" :
                                                                g.Key.Jenis == 4 ? "Lainnya" : "-",

                                                Tanggal = DateTime.ParseExact(string.Format("01/{0}/{1}", g.Key.Bulan.ToString("D2"), g.Key.Tahun), "dd/mm/yyyy", CultureInfo.InvariantCulture),
                                                

                                                Nilai = g.Sum(x => x.CreditQuality * x.Progress)
                                            }
                         ).ToList();


            List<object> chartData = new List<object>();
            List<ListTanggal> labels = new List<ListTanggal>();
            labels = (from l in result
                      group l by new { l.Tanggal }
                          into g
                          select new ListTanggal
                          {
                              Tanggal = g.Key.Tanggal

                          }).ToList();


            chartData.Add(labels);

            List<double?> series1 = new List<double?>();
            series1 = result.Where(s1 => s1.StatusName.Contains("Reguler")).Select(s1 => s1.Nilai).ToList();
            chartData.Add(series1);

            List<double?> series2 = new List<double?>();
            series2 = result.Where(s2 => s2.StatusName.Contains("Tambahan")).Select(s2 => s2.Nilai).ToList();
            chartData.Add(series2);

            List<double?> series3 = new List<double?>();
            series3 = result.Where(s3 => s3.StatusName.Contains("Khusus")).Select(s3 => s3.Nilai).ToList();
            chartData.Add(series3);


            List<double?> series4 = new List<double?>();
            series4 = result.Where(s4 => s4.StatusName.Contains("Lainnya")).Select(s4 => s4.Nilai).ToList();
            chartData.Add(series4);


            //List<double?> series5 = new List<double?>();
            //series5 = (from t in task
            //           group t by new
            //           {
            //               Bulan = t.AssignDate.Month,
            //               Tahun = t.AssignDate.Year
            //           } into g
            //           select new 
            //           {

            //                Nilai = Convert.ToDouble(g.Sum(x => x.CreditQuality * x.Progress))
            //           }
            //             ).ToList();

            //chartData.Add(series5);

            return new JsonResult
            {
                Data = new
                {
                    success = chartData,
                    message = "Success",
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };







        }

        private List<SelectListItem> ApprovedStatus()
        {

            var listItems = new List<SelectListItem> 
        { 
              new SelectListItem { Text = "Waiting", Value = "0" }, 
              new SelectListItem { Text = "Approved", Value = "1" } 
        };
            return listItems;

        }


        private List<SelectListItem> ProgressStatus()
        {

            var listItems = new List<SelectListItem> 
        { 
              new SelectListItem { Text = "New", Value = "1" }, 
              new SelectListItem { Text = "On Working", Value = "2" } ,
              new SelectListItem { Text = "Complete", Value = "3" }, 
              new SelectListItem { Text = "On Cancel", Value = "4" } 
        };
            return listItems;

        }


        // Now Edit Part
        public ActionResult Edit(int id)
        {
            User userdata = (User)Session["UserData"];
            int CompanyId = userdata.CompanyID;
            Session["TaskID"] = Convert.ToInt32(id);
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;

            Task task = new Task();
            task = GetTaskByID(id);

            ViewBag.Companies = new SelectList(kController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");
            ViewBag.ProgressStatus = ProgressStatus();
            ViewBag.ApprovedStatus = ApprovedStatus();



            if (task == null)
            {
                return HttpNotFound("Task Not Found!");
            }

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Task c, HttpPostedFileBase file)
        {
            User userdata = (User)Session["UserData"];
            int CompanyId = userdata.CompanyID;
            int id = Convert.ToInt32(Session["TaskID"]);
            Session["TaskID"] = Convert.ToInt32(id);
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;

            Task task = new Task();
            task = GetTaskByID(id);

                        ViewBag.Companies = new SelectList(kController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");
            ViewBag.ProgressStatus = ProgressStatus();
            ViewBag.ApprovedStatus = ApprovedStatus();

            if (c.TaskID != task.TaskID)
                c.TaskID = task.TaskID;

            if (c.UserId != task.UserId)
                c.UserId = task.UserId;

            if (c.EmployeeNo != task.EmployeeNo)
                c.EmployeeNo = task.EmployeeNo;

            if ((c.TaskDescription != task.TaskDescription) && (c.TaskDescription == null || c.TaskDescription == String.Empty))
                c.TaskDescription = task.TaskDescription;

            if ((c.TaskResultNotes != task.TaskResultNotes) && (c.TaskResultNotes == null || c.TaskResultNotes == String.Empty))
                c.TaskResultNotes = task.TaskResultNotes;

            if ((c.TaskEvaluationNotes != task.TaskEvaluationNotes) && (c.TaskEvaluationNotes == null || c.TaskEvaluationNotes == String.Empty))
                c.TaskEvaluationNotes = task.TaskEvaluationNotes;

            if ((c.JobTypeDescription != task.JobTypeDescription) && (c.JobTypeDescription == null || c.JobTypeDescription == String.Empty))
                c.JobTypeDescription = task.JobTypeDescription;

            if ((c.JobTypeDescription != task.JobTypeDescription) && (c.JobTypeDescription == null || c.JobTypeDescription == String.Empty))
                c.JobTypeDescription = task.JobTypeDescription;

            if ((c.CompanyId != task.CompanyId) && (c.CompanyId == null || c.CompanyId == 0))
                c.CompanyId = task.CompanyId;

            if ((c.Status != task.Status) && (c.Status == null || c.Status == 0))
                c.Status = task.Status;

            if ((c.CompanyId != task.CompanyId) && (c.CompanyId == null || c.CompanyId == 0))
                c.CompanyId = task.CompanyId;

            if ((c.ApprovedStatus != task.ApprovedStatus) && (c.ApprovedStatus == null || c.ApprovedStatus == 0))
                c.ApprovedStatus = task.ApprovedStatus;

            if ((c.QualityResult != task.QualityResult) && (c.QualityResult == null || c.QualityResult == 0))
                c.QualityResult = task.QualityResult;

            if (c.EmployeeNo == null || c.EmployeeNo == String.Empty)
                c.EmployeeNo = userdata.EmployeeNo;
            if (c.ApprovedBy == null)
                c.ApprovedBy = String.Empty;
            if (c.TaskDescription == null)
                c.TaskDescription = String.Empty;
            if (c.TaskResultNotes == null)
                c.TaskResultNotes = String.Empty;
            if (c.TaskEvaluationNotes == null)
                c.TaskEvaluationNotes = String.Empty;
            if (c.JobTypeDescription == null)
                c.JobTypeDescription = String.Empty;
            if (c.QualityResult == null)
                c.QualityResult = 0;
            if (c.ApprovedStatus == null)
                c.ApprovedStatus = 0;
            if (c.ApprovedBy == null)
                c.ApprovedBy = String.Empty;
            if (c.UserId == null)
                c.UserId = userdata.UserId;
            if (c.Status == null)
                c.Status = 1;
            if (c.Progress == null)
                c.Progress = 0;
            if (c.QualityResult == null)
                c.QualityResult = 0;
            if (c.CompanyId == null)
                c.CompanyId = CompanyId;

            if (ModelState.IsValid)
            {


                using (DataContext dc = new DataContext())
                {
                    JobDescription jobdesc = dc.JobDescriptions.Where(j => j.JobDescId.Equals(c.JobDescTypeID)).FirstOrDefault();
                    var v = dc.Tasks.Where(a => a.TaskID.Equals(c.TaskID)).FirstOrDefault();
                    if (v != null)
                    {
                        
                        if (c.Status == 3) //TaskState.Completed 
                            c.Progress = 100;
                        
                        c.CreditQuality = jobdesc.CreditQuality;
                        c.DateUpdated = DateTime.Now;
                        if (c.ApprovedStatus == 1)
                        {
                            c.ApprovedDate = DateTime.Now;
                          
                        }
                        else
                        {
                            c.ApprovedStatus = 0;
                            c.ApprovedDate = Convert.ToDateTime("01/12/1899");
                            c.ApprovedBy = "";
                        }

                    }
                    dc.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(c);
            }
        }

        //Delete 
        public ActionResult Delete(int id)
        {
            User userdata = (User)Session["UserData"];
            int CompanyId = userdata.CompanyID;
            Session["TaskID"] = Convert.ToInt32(id);
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;
            Task task = null;
            task = GetTaskByID(id);
            ViewBag.Companies = new SelectList(kController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");
            ViewBag.ProgressStatus = ProgressStatus();
            ViewBag.ApprovedStatus = ApprovedStatus();

            return View(task);
        }

        //Delete POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {

            User userdata = (User)Session["UserData"];
            int CompanyId = userdata.CompanyID;
            Session["TaskID"] = Convert.ToInt32(id);
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;
            Task task = null;
            task = GetTaskByID(id);
            ViewBag.Companies = new SelectList(kController.GetAllCompany(), "CompanyId", "CompanyName");
            ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");
            ViewBag.ProgressStatus = ProgressStatus();
            ViewBag.ApprovedStatus = ApprovedStatus();

            using (DataContext dc = new DataContext())
            {
                var taskRemove = dc.Tasks.Where(a => a.TaskID.Equals(id)).FirstOrDefault();
                if (taskRemove != null)
                {
                    dc.Tasks.Remove(taskRemove);
                    dc.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound("Task Not Found!");
                }
            }
        }

        //Export to Excel
        public ActionResult Export()
        {
            int userid = 0;
            userid = Convert.ToInt32(Session["UserID"]);

            List<TaskViewModel> allTasks = new List<TaskViewModel>();
            allTasks = GetTask(userid);
            return View(allTasks);
        }

        [HttpPost]
        [ActionName("Export")]
        public FileResult ExportData()
        {

            int userid = 0;
            userid = Convert.ToInt32(Session["UserID"]);

            List<TaskViewModel> allTasks = new List<TaskViewModel>();
            allTasks = GetTask(userid);

            var grid = new WebGrid(source: allTasks, canPage: false, canSort: false);
            string exportData = grid.GetHtml(
                            columns: grid.Columns(
                                        grid.Column("ContactID", "Contact ID"),
                                        grid.Column("FirstName", "First Name"),
                                        grid.Column("LastName", "Last Name"),
                                        grid.Column("ContactNo1", "Contact No1"),
                                        grid.Column("ContactNo2", "Contact No2"),
                                        grid.Column("EmailID", "Email ID")
                                    )
                                ).ToHtmlString();
            return File(new System.Text.UTF8Encoding().GetBytes(exportData),
                    "application/vnd.ms-excel",
                    "Tasks.xls");

        }
        public ActionResult Create()
        {
            int userid = 0;
            userid = Convert.ToInt32(Session["UserID"]);
            UserLoginModel c = uController.GetUserLogin(userid);
            int CompanyId = c.CompanyId;
            int id = Convert.ToInt32(Session["TaskID"]);
            Session["TaskID"] = id;

            User userdata = (User)Session["UserData"];
            Session["UserData"] = userdata;
            Session["UserID"] = userdata.UserId;

            ViewBag.ProgressStatus = ProgressStatus();
            ViewBag.ApprovedStatus = ApprovedStatus();
            if (id > 0)
            {
                //Update

                if (c != null)
                {
                    ViewBag.Companies = new SelectList(kController.GetAllCompany(), "CompanyId", "CompanyName");
                    ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");

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
                ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");
                return View();
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Task c)
        {
            string message = "";
            bool status = false;
            int userid = 0;
            userid = Convert.ToInt32(Session["UserID"]);
            Session["TaskID"] = c.TaskID;
            User u = (User)Session["UserData"];
            int CompanyId = u.CompanyID;

            Session["UserData"] = u;
            Session["UserID"] = u.UserId;

            ViewBag.ProgressStatus = ProgressStatus();
            ViewBag.ApprovedStatus = ApprovedStatus();

            User userdata = (User)Session["UserData"];
            if (c.EmployeeNo == null || c.EmployeeNo == String.Empty)
                c.EmployeeNo = userdata.EmployeeNo;
            if (c.ApprovedBy == null)
                c.ApprovedBy = String.Empty;
            if (c.TaskDescription == null)
                c.TaskDescription = String.Empty;
            if (c.TaskResultNotes == null)
                c.TaskResultNotes = String.Empty;
            if (c.TaskEvaluationNotes == null)
                c.TaskEvaluationNotes = String.Empty;
            if (c.JobTypeDescription == null)
                c.JobTypeDescription = String.Empty;
            if (c.QualityResult == null)
                c.QualityResult = 0;
            if (c.ApprovedStatus == null)
                c.ApprovedStatus = 0;
            if (c.ApprovedBy == null)
                c.ApprovedBy = String.Empty;
            if (c.UserId == null)
                c.UserId = userdata.UserId;
            if (c.Status == null)
                c.Status = 1;
            if (c.Progress == null)
                c.Progress = 0;
            if (c.CreditQuality == null)
                c.CreditQuality = 0;
            if (c.CompanyId == null)
                c.CompanyId = CompanyId;


            if (ModelState.IsValid)
            {
                using (DataContext dc = new DataContext())
                {
                    JobDescription jobdesc = dc.JobDescriptions.Where(j => j.JobDescId.Equals(c.JobDescTypeID)).FirstOrDefault();
                    if (c.TaskID > 0)
                    {
                        //Update
                        var v = dc.Tasks.Where(a => a.TaskID.Equals(c.TaskID)).FirstOrDefault();
                        if (v != null)
                        {
                           
                            if (c.Status == 3) //TaskState.Completed 
                                c.Progress = 100;
                            else
                                c.Progress = c.Progress;
                            v.CreditQuality = jobdesc.CreditQuality;
                            
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                    else
                    {

                        c.TaskResultNotes= "";
                        c.ApprovedBy = "";
                        c.ApprovedStatus = 0;
                        c.ApprovedDate = DateTime.Now;
                        c.UserId = userid;
                        c.CreditQuality = jobdesc.CreditQuality;
                        c.QualityResult= 0;
                        c.Status = 1; // TaskState.New;
                        c.Progress = 0;
                        c.DateCreated = DateTime.Now;
                        c.DateUpdated = DateTime.Now;
                        c.EmployeeNo = u.EmployeeNo;
                        c.CompanyId = u.CompanyID;
                        dc.Tasks.Add(c);
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
                ViewBag.JobDescriptions = new SelectList(jdController.GetJobDescriptionByCompany(CompanyId), "JobDescId", "JobDescDescription");

            }
            ViewBag.Message = message;
            ViewBag.Status = status;
            return View(c);
        }












    }

}