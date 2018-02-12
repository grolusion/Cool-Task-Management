using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoolTaskManagement.DAL
{
    public class Country
    {
        public int CountryID { get; set; }
        [Display(Name = "Country Namee")]
        public string CountryName { get; set; }
       
    }

}
