using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace CoolTaskManagement.DAL.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string EmployeeNo)
        {
            this.Identity = new GenericIdentity(EmployeeNo);
        }

        public int UserId { get; set; }
        public string EmployeeNo { get; set; }
        public string Nama { get; set; }
        public string JobPosition { get; set; }
        public string ImgPath { get; set; }
        public int JobDescriptionID { get; set; }
        public string[] roles { get; set; }
    }

    public class CustomPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string JobPosition { get; set; }
        public string ImgPath { get; set; }
        public int PositionId { get; set; }

        public string[] roles { get; set; }
    }
}