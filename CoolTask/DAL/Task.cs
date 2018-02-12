using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoolTaskManagement.DAL
{
    public class Task
    {
        [Key]
        public long TaskID { get; set; }
        public int UserId { get; set; }
        [Display(Name = "Job Type")]
        public int JobDescTypeID { get; set; }
        public string EmployeeNo { get; set; }
        [Required]
        [Display(Name = "Assign Date")]
        public DateTime AssignDate  { get; set; }
        [Display(Name = "Starting Date")]
        public DateTime? StartingDate { get; set; }
        [Display(Name = "Finishing Date")]
        public DateTime? FinishingDate { get; set; }
        [Display(Name = "Type Description")]
        public string JobTypeDescription { get; set; }
        [Required]
        [Display(Name = "Task Description")]
	    public string TaskDescription { get; set; }
        [Display(Name = "Task Result Notes")]
	    public string TaskResultNotes { get; set; }
        [Display(Name = "Task Evaluation Notes")]
        public string TaskEvaluationNotes { get; set; }
        public int Status { get; set; } 
	    public double Progress { get; set; }
        [Display(Name = "Credit Quality")]
        public double CreditQuality { get; set; }
        [Display(Name = "Quality Result")]
        public double QualityResult { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int ApprovedStatus { get; set; }
        public string ApprovedBy{ get; set; } 
        public DateTime ApprovedDate { get; set; }
        public int CompanyId { get; set; }
        
    }


    public enum TaskState : int
    {
        New = 1,
        WorkingOn = 2,
        Completed = 3,
        Canceled = 4
    }


    public enum TaskType : int
    {
        Rutin = 1,
        Tambahan = 2,
        Khusus = 3,
        Lain = 4
    }
   
}
