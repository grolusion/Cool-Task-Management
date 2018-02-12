using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoolTaskManagement.DAL
{
    public class JobPosition
    {
         [Key]
        public int PositionId { get; set; }
         public string PositionTitle { get; set; }
         public string PositionDescription { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public Boolean IsActive { get; set; }
       
    }

}
