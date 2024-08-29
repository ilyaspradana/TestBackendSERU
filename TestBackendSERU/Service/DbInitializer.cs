using System;
using System.Collections.Generic;
using System.Linq;
using TestBackendSERU.Entity;

namespace TestBackendSERU.Service
{
    public class DbInitializer
    {
        public static void Seed(VehicleDbContext context)
        {

            if (!context.User.Any())
            {
                var user = new List<User>
                {
                    new User { 
                        Name = "Admin",
                        Is_Admin = true,
                        Created_At = DateTime.Now.ToString(),
                        Updated_At =  DateTime.Now.ToString()
                    },
                    new User {
                        Name = "User",
                        Is_Admin = false,
                        Created_At = DateTime.Now.ToString(),
                        Updated_At =  DateTime.Now.ToString()
                    }
                };
                user.ForEach(a => context.User.Add(a));

                context.SaveChanges();
            }
        }
    }
}
