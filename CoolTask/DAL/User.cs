using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoolTaskManagement.DAL
{
    public class User
    {
        [Display(Name = "User ID")]
        public int UserId { get; set; }
        [Display(Name = "Employee No")]
        [StringLength(8, MinimumLength = 1)]
        [Required(ErrorMessage="Enter Employee No.") ]
        public string EmployeeNo { get; set; }
        [Required(ErrorMessage = "Enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter your position")]
        [Display(Name = "Job Position")]
        public int JobPositionId { get; set; }

        [Required(ErrorMessage = "Enter E-mail Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                ErrorMessage = "Invalid E-mail")]
        public string Email { get; set; }

       
        [Required(ErrorMessage = "Enter your address")]
        public string Address { get; set; }
        public string City { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        
        [Display(Name = "Telephone No.")]
        [Required(ErrorMessage = "Enter your telephone no")]
        public string TelephoneNo { get; set; }

        public string WebSiteUrl { get; set; }

        public Boolean IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Select your company")]
        public int CompanyID { get; set; }
        [Display(Name = "State")]
        [Required(ErrorMessage = "Select your state")]
        public int StateID { get; set;}
        [Display(Name = "Country")]
        [Required(ErrorMessage = "Select your country")]
        public int CountryId { get; set; }
        public string ImgPath { get; set; }

        public string Password { get; set; }
        
        [Display(Name = "Enter password again")]
        public string Password2 { get; set; }

        public bool RememberMe { get; set; }
        public DateTime DOB { get; set; }
        public double Salary { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        
    }



}