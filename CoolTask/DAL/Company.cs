using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoolTaskManagement.DAL
{   
    public class Company
    {
        [Key]
        [Display(Name = "Company Id")]
        public int CompanyId { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Description")]
        public string CompanyDescription { get; set; }
        [Display(Name = "Address")]
        public string CompanyAddress { get; set; }
        [Display(Name = "City")]
        public string CompanyCity { get; set; }
        [Display(Name = "Zip Code")]
        public string CompanyZipCode { get; set; }
        [Display(Name = "Telephone No")]
        public string CompanyTelephoneNo { get; set; }
        [Display(Name = "Coordinate")]
        public double CompanyLat { get; set; }
        [Display(Name = "Coordinate")]
        public double CompanyLong { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public Boolean IsActive { get; set; }
       
    }

}
