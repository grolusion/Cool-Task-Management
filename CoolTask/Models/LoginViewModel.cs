using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CoolTaskManagement.DAL;

namespace CoolTaskManagement.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Employee No")]
        public string EmployeeNo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class UserLoginModel
    {
        public int UserId { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public int JobPositionId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string TelephoneNo { get; set; }
        public string WebSiteUrl { get; set; }
        public string JobPositionTitle { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public string State { get; set; }
        public string Country { get; set; }

        public int StateID { get; set; }
        public int CountryID { get; set; }

        public string ImgPath { get; set; }

        public DateTime DOB { get; set; }
        public double Salary { get; set; }

        public bool IsActive { get; set; }
    }
}