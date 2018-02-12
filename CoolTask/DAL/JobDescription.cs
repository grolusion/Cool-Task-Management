using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoolTaskManagement.DAL
{
    public class JobDescription
    {
         [Key]
        public int JobDescId { get; set; }
       
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        public string JobDescDescription { get; set; }

        public int JobDescCategory { get; set; }
        [Display(Name = "Credit Quality")]
        public double CreditQuality { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public Boolean IsActive { get; set; }
       
    }

}
