using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CoolTaskManagement.Models
{
    public class TaskViewModel
    {
        public int UserId { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public string CompanyName { get; set; }
        public int TypeID { get; set; }
        public string TypeDescription { get; set; }
        public long TaskID { get; set; }
        [Display(Name = "Tgl. Tugas")]
        public DateTime AssignDate { get; set; }
        [Display(Name = "Tgl. Pelaksanaan")]
        public DateTime? StartingDate { get; set; }
        [Display(Name = "Tgl. Selesai")]
        public DateTime? FinishingDate { get; set; }
        [Display(Name = "Uraian Jenis Tugas")]
        public String JobTypeDescription { get; set; }
        [Display(Name = "Uraian Pelaksanaan Tugas")]
        public string TaskDescription { get; set; }
        [Display(Name = "Catatan Pelaksanaan Tugas")]
        public string TaskResultNotes { get; set; }
        [Display(Name = "Evaluasi Pelaksanaan Tugas")]
        public string TaskEvaluationNotes { get; set; }
        public int Status { get; set; }
        public double Progress { get; set; }
        [Display(Name = "Bobot Nilai")]
        public double CreditQuality { get; set; }
        public double QualityResult { get; set; }
        [Display(Name = "Approved")]
        public int ApprovedStatus { get; set; }
        [Display(Name = "Di Setujui Oleh")]
        public string ApprovedBy { get; set; }
        [Display(Name = "Di Setujui Tgl")]
        public DateTime ApprovedDate { get; set; }
    }


    public class TaskReportModel
    {
        public int UserId { get; set; }
        public string EmployeeNo { get; set; }
        public string Nama { get; set; }
        public string JobPosition { get; set; }
        public string NamaJobDescription { get; set; }
        public int ID { get; set; }
        public string Diskripsi { get; set; }
        public long TaskID { get; set; }
        [Display(Name = "Tgl. Tugas")]
        public DateTime AssignDate { get; set; }
        [Display(Name = "Tgl. Pelaksanaan")]
        public DateTime? StartingDate { get; set; }
        [Display(Name = "Tgl. Selesai")]
        public DateTime? FinishingDate { get; set; }
        [Display(Name = "Uraian Jenis Tugas")]
        public String JobTypeDescription { get; set; }
        [Display(Name = "Uraian Pelaksanaan Tugas")]
        public string TaskDescription { get; set; }
        [Display(Name = "Catatan Pelaksanaan Tugas")]
        public string TaskResultNotes { get; set; }
        [Display(Name = "Evaluasi Pelaksanaan Tugas")]
        public string TaskEvaluationNotes { get; set; }
        public int Status { get; set; }
        public double Progress { get; set; }
        [Display(Name = "Bobot Nilai")]
        public double CreditQuality { get; set; }
        public double QualityResult { get; set; }
        [Display(Name = "Approved")]
        public int ApprovedStatus { get; set; }
        [Display(Name = "Di Setujui Oleh")]
        public string ApprovedBy { get; set; }
        [Display(Name = "Di Setujui Tgl")]
       public DateTime ApprovedDate { get; set; }

        
    }

    public class TaskSeriesModel
    {
       public  DateTime Tanggal { get; set; }
       public string StatusName  { get; set; }

       public double? Nilai { get; set; }

    
    }


    public class LineChartSeriesModel
    {
        public DateTime Tanggal { get; set; }
        public string StatusName { get; set; }

        public double Nilai { get; set; }

        public int  id { get; set; }
        public int parentid { get; set; }
    }


    
    public class TaskSeriesByStatusModel
    {
       public  string StatusName  { get; set; }

       public double Nilai { get; set; }
    }

    public class LeaderboardViewModel
    {
        public List<TaskSeriesModel> LeaderboardViewEntries { get; set; }
        public HistoicPointsViewModel HistoicPoints { get; set; }
    }


    public class HistoicPointsViewModel
    {
        public HistoicPointsViewModel()
        {
            TaskHistores = new List<TasksHistoricViewModel>();
        }
        public DateTime[] YAxisDates { get; set; }
        public List<TasksHistoricViewModel> TaskHistores { get; set; }
    }

    public class ListTanggal
    {
        
        public DateTime Tanggal { get; set; }
        

    }
    public class TasksHistoricViewModel
    {
   
        public string StatusName { get; set; }
        public List<double?> Nilai { get; set; }

        public TasksHistoricViewModel()
        {
            Nilai = new List<double?>();
        }
    }

    public class DtatsetModel
    {
        public string label { get; set; }
        public const string fillColor = "rgba(220,220,220,0.2)";
        public const string strokeColor = "rgba(220,220,220,1)";
        public const string pointColor = "rgba(220,220,220,1)";
        public const string pointStrokeColor = "#fff";
        public const string pointHighlightFill = "#fff";
        public const string pointHighlightStroke = "rgba(151,187,205,1)";
        public List<string> data { get; set; }

       
      
    }

    public class ChartModel
    {
        public List<string> labels { get; set; }
        public List<DtatsetModel> datasets { get; set; }

       
       
    }

    public  class Point
    {
        public int x { get; set; }
        public Nullable<DateTime> y { get; set; }
    }

}