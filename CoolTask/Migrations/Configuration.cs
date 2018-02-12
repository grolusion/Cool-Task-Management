namespace CoolTaskManagement.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CoolTaskManagement.DAL;

    internal sealed class Configuration : DbMigrationsConfiguration<CoolTaskManagement.DAL.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        
        protected override void Seed(CoolTaskManagement.DAL.DataContext context)
        {
                        context.Roles.AddOrUpdate(
                 new Role
                 {
                     RoleName = "Manager",
                     Description = ""
                 },

                  new Role
                 {
                     RoleName = "Staff",
                     Description = ""
                 }
           	);

            context.Countries.AddOrUpdate(
                    new Country
                    {
                        CountryID = 1,
                        CountryName = "Indonesia"
                    },

                    new Country
                    {
                        CountryID = 2,
                        CountryName = "Malaysia"
                    },
                     new Country
                    {
                        CountryID = 3,
                        CountryName = "Singapura"
                    }
           	);

            context.States.AddOrUpdate(
                   new State
                   {
                       CountryID = 1,
                       StateName = "Jakarta"
                   },

                   new State
                   {
                       CountryID = 1,
                       StateName = "Jawa Barat"
                   },
                     new State
                   {
                       CountryID = 1,
                       StateName = "Jawa Tengah"
                   },
                     new State
                   {
                       CountryID = 1,
                       StateName = "Jawa Timur"
                   },
                     new State
                   {
                       CountryID = 1,
                       StateName = "Bali"
                   },
                    new State
                   {
                       CountryID = 1,
                       StateName = "Banten"
                   },
                     new State
                   {
                       CountryID = 1,
                       StateName = "Lampung"
                   },
                     new State
                   {
                       CountryID = 1,
                       StateName = "Sumatera Selatan"
                   },
                     new State
                   {
                       CountryID = 1,
                       StateName = "Bengukulu"
                   },
                     new State
                   {
                       CountryID = 1,
                       StateName = "Jambi"
                   },
                     new State
                   {
                       CountryID = 1,
                       StateName = "Sumatra Barat"
                   },
                     new State
                   {
                       CountryID = 2,
                       StateName = "Kuala Lumpur"
                   },
                     new State
                   {
                       CountryID = 2,
                       StateName = "Johor Baru"
                   },
                     new State
                   {
                       CountryID = 2,
                       StateName = "Ampang Raya"
                   },
                     new State
                   {
                       CountryID = 3,
                       StateName = "Singapore"
                   },
                     new State
                   {
                       CountryID = 3,
                       StateName = "Bukit Batok"
                   }

            );
            context.JobPositions.AddOrUpdate(
                    new JobPosition
                    {
                        PositionTitle = "Manager",
                        PositionDescription = "",
                        DateCreated = DateTime.Now,
                        DateUpdated = DateTime.Now,
                        IsActive = true
                    },

                     new JobPosition
                    {
                        PositionTitle = "Staff",
                        PositionDescription = "",
                        DateCreated = DateTime.Now,
                        DateUpdated = DateTime.Now,
                        IsActive = true
                    }
              );

            context.Companies.AddOrUpdate(
                    new Company
                    {
                        CompanyName = "LOGISFLEET PTE. LTD",
                        CompanyLat = 0,
                        CompanyLong=0,
                        DateCreated = DateTime.Now,
                        DateUpdated = DateTime.Now,
                        IsActive = true
                    },

                    new Company
                    {
                        CompanyName = "PT. KOOL ASIA TEKNOLOGI",
                        CompanyLat = 0,
                        CompanyLong=0,
                        DateCreated = DateTime.Now,
                        DateUpdated = DateTime.Now,
                        IsActive = true
                    }
              );
        }
      
    }
}
