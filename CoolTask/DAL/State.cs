using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoolTaskManagement.DAL
{
    public class State
    {
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public string StateName { get; set; }
    }
   
}
