using NBD4.Models;
using System.Diagnostics;
using System.Numerics;

namespace NBD4.Data
{
    public static class NBDInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            NBDContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<NBDContext>();

            try
            {

                if (!context.Clients.Any())
                {
                    context.Clients.AddRange(
                    new Client
                    {
                        Name = "John Doe",
                        Email = "john.doe@example.com",
                        Phone = "+1 (555) 123-4567",
                    },
                    new Client
                    {
                        Name = "Alice Smith",
                        Email = "alice.smith@example.com",
                        Phone = "+1 (555) 987-6543",
                    },
                    new Client
                    {
                        Name = "Bob Johnson",
                        Email = "bob.johnson@example.com",
                        Phone = "+1 (555) 789-0123",
                    },
                    new Client
                    {
                        Name = "Eva Brown",
                        Email = "eva.brown@example.com",
                        Phone = "+1 (555) 456-7890",
                    },
                    new Client
                    {
                        Name = "Michael Davis",
                        Email = "michael.davis@example.com",
                        Phone = "+1 (555) 234-5678",
                    }
                    )
                    ;
                    context.SaveChanges();
                }
                // Seed Patients if there aren't any.
                if (!context.Projects.Any())
                {
                    context.Projects.AddRange(
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-04-23"),
                        EndDate = DateTime.Parse("2024-05-23"),
                        Site = "Main entrance Mall Dr./Cinema Lane",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Client1").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-05-10"),
                        EndDate = DateTime.Parse("2024-06-10"),
                        Site = "Downtown Plaza",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Client2").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-06-01"),
                        EndDate = DateTime.Parse("2024-07-01"),
                        Site = "Shopping Center Avenue",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Client3").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-07-15"),
                        EndDate = DateTime.Parse("2024-08-15"),
                        Site = "Tech Park Tower",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Client4").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-08-05"),
                        EndDate = DateTime.Parse("2024-09-05"),
                        Site = "Riverside Gardens",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Client5").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-09-20"),
                        EndDate = DateTime.Parse("2024-10-20"),
                        Site = "Harbor View Plaza",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Client6").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-10-15"),
                        EndDate = DateTime.Parse("2024-11-15"),
                        Site = "Green Park Street",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Client7").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-11-01"),
                        EndDate = DateTime.Parse("2024-12-01"),
                        Site = "Metro Center Square",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Client8").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-12-10"),
                        EndDate = DateTime.Parse("2025-01-10"),
                        Site = "Sunset Ridge Avenue",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Client9").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2025-01-05"),
                        EndDate = DateTime.Parse("2025-02-05"),
                        Site = "Oakwood Terrace",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Client10").ID
                    }
                    );
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }
        }
    }
}
