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
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                if (!context.Provinces.Any())
                {
                    var provinces = new List<Province>
                    {
                        new Province { ID = "ON", Name = "Ontario"},
                        new Province { ID = "PE", Name = "Prince Edward Island"},
                        new Province { ID = "NB", Name = "New Brunswick"},
                        new Province { ID = "BC", Name = "British Columbia"},
                        new Province { ID = "NL", Name = "Newfoundland and Labrador"},
                        new Province { ID = "SK", Name = "Saskatchewan"},
                        new Province { ID = "NS", Name = "Nova Scotia"},
                        new Province { ID = "MB", Name = "Manitoba"},
                        new Province { ID = "QC", Name = "Quebec"},
                        new Province { ID = "YT", Name = "Yukon"},
                        new Province { ID = "NU", Name = "Nunavut"},
                        new Province { ID = "NT", Name = "Northwest Territories"},
                        new Province { ID = "AB", Name = "Alberta"}
                    };
                    context.Provinces.AddRange(provinces);
                    context.SaveChanges();
                }
                if (!context.Cities.Any())
                {
                    var cities = new List<City>
                    {
                        new City { Name = "Toronto", ProvinceID="ON" },
                        new City { Name = "Halifax", ProvinceID="NS" },
                        new City { Name = "Calgary", ProvinceID="AB" },
                        new City { Name = "Winnipeg", ProvinceID="MB", },
                        new City { Name = "Stratford", ProvinceID="ON" },
                        new City { Name = "St. Catharines", ProvinceID="ON" },
                        new City { Name = "Stratford", ProvinceID="PE" },
                        new City { Name = "Ancaster", ProvinceID="ON" },
                        new City { Name = "Vancouver", ProvinceID="BC" },
                        new City { Name = "Montreal", ProvinceID = "QC" },
                        new City { Name = "Quebec City", ProvinceID = "QC" },
                        new City { Name = "Edmonton", ProvinceID = "AB" },
                        new City { Name = "Ottawa", ProvinceID = "ON" },
                        new City { Name = "Fredericton", ProvinceID = "NB" },
                        new City { Name = "Victoria", ProvinceID = "BC" },
                        new City { Name = "Regina", ProvinceID = "SK" },
                        new City { Name = "Charlottetown", ProvinceID = "PE" },
                        new City { Name = "Yellowknife", ProvinceID = "NT" },
                        new City { Name = "Whitehorse", ProvinceID = "YT" },
                        new City { Name = "Iqaluit", ProvinceID = "NU" },
                        new City { Name = "Saskatoon", ProvinceID = "SK" },
                        new City { Name = "London", ProvinceID = "ON" },
                        new City { Name = "Windsor", ProvinceID = "ON" },
                    };
                    context.Cities.AddRange(cities);
                    context.SaveChanges();
                }                
                

                if (!context.Clients.Any())
                {
                    context.Clients.AddRange(
                    new Client
                    {
                        Name = "Tech Innovators Ltd",
                        ContactFirstName="John",
                        ContactMiddleName="N",
                        ContactLastName="Doe",
                        Email = "john.doe@example.com",
                        Phone = "5559876543",
                        Street = "123 Main St",
                        PostalCode = "M1M1M1",
                        CityID = 2
                    },
                    new Client
                    {
                        Name = "Global Solutions Inc",
                        Email = "alice.smith@example.com",
                        ContactFirstName = "Alice",
                        ContactMiddleName = "D",
                        ContactLastName = "Smith",
                        Phone = "7896541230",
                        Street = "456 Elm St",
                        PostalCode = "V2V2V2",
                        CityID = 3
                    },
                    new Client
                    {
                        Name = "Pinnacle Enterprises",
                        ContactFirstName = "Bob",
                        ContactMiddleName = "N",
                        ContactLastName = "Johnson",
                        Email = "bob.johnson@example.com",
                        Phone = "5553334444",
                        Street = "789 Oak St",
                        PostalCode = "K3K3K3",
                        CityID = 4
                    },
                    new Client
                    {
                        Name = "Infinite Dynamics Co",
                        ContactFirstName = "Eva",
                        ContactMiddleName = "N",
                        ContactLastName = "Brown",
                        Email = "eva.brown@example.com",
                        Phone = "7778889999",
                        Street = "101 Pine St",
                        PostalCode = "R4R4R4",
                        CityID = 5
                    },
                    new Client
                    {
                        Name = "EagleEye Technologies",
                        ContactFirstName = "Michael",
                        ContactMiddleName = "N",
                        ContactLastName = "Davis",
                        Email = "michael.davis@example.com",
                        Phone = "6665554444",
                        Street = "202 Cedar St",
                        PostalCode = "L5L5L5",
                        CityID = 6
                    }
                    )
                    ;
                    context.SaveChanges();
                }
                
                if (!context.Projects.Any())
                {
                    context.Projects.AddRange(
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-04-23"),
                        EndDate = DateTime.Parse("2024-05-23"),
                        Site = "Main entrance Mall Dr./Cinema Lane",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Tech Innovators Ltd").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-05-10"),
                        EndDate = DateTime.Parse("2024-06-10"),
                        Site = "Downtown Plaza",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Tech Innovators Ltd").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-06-01"),
                        EndDate = DateTime.Parse("2024-07-01"),
                        Site = "Shopping Center Avenue",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Global Solutions Inc").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-07-15"),
                        EndDate = DateTime.Parse("2024-08-15"),
                        Site = "Tech Park Tower",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Global Solutions Inc").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-08-05"),
                        EndDate = DateTime.Parse("2024-09-05"),
                        Site = "Riverside Gardens",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Pinnacle Enterprises").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-09-20"),
                        EndDate = DateTime.Parse("2024-10-20"),
                        Site = "Harbor View Plaza",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Pinnacle Enterprises").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-10-15"),
                        EndDate = DateTime.Parse("2024-11-15"),
                        Site = "Green Park Street",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Infinite Dynamics Co").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-11-01"),
                        EndDate = DateTime.Parse("2024-12-01"),
                        Site = "Metro Center Square",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "Infinite Dynamics Co").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2024-12-10"),
                        EndDate = DateTime.Parse("2025-01-10"),
                        Site = "Sunset Ridge Avenue",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "EagleEye Technologies").ID
                    },
                    new Project
                    {
                        StartDate = DateTime.Parse("2025-01-05"),
                        EndDate = DateTime.Parse("2025-02-05"),
                        Site = "Oakwood Terrace",
                        ClientID = context.Clients.FirstOrDefault(c => c.Name == "EagleEye Technologies").ID
                    }
                    );
                    context.SaveChanges();
                }
                if (!context.Bids.Any())
                {
                    var projectTechInnovators1 = context.Projects.FirstOrDefault(p => p.Site == "Main entrance Mall Dr./Cinema Lane" && p.Client.Name == "Tech Innovators Ltd");
                    var projectTechInnovators2 = context.Projects.FirstOrDefault(p => p.Site == "Downtown Plaza" && p.Client.Name == "Tech Innovators Ltd");
                    var projectGlobalSolutions1 = context.Projects.FirstOrDefault(p => p.Site == "Shopping Center Avenue" && p.Client.Name == "Global Solutions Inc");
                    var projectGlobalSolutions2 = context.Projects.FirstOrDefault(p => p.Site == "Tech Park Tower" && p.Client.Name == "Global Solutions Inc");
                    var projectPinnacleEnterprises1 = context.Projects.FirstOrDefault(p => p.Site == "Riverside Gardens" && p.Client.Name == "Pinnacle Enterprises");
                    var projectPinnacleEnterprises2 = context.Projects.FirstOrDefault(p => p.Site == "Harbor View Plaza" && p.Client.Name == "Pinnacle Enterprises");
                    var projectInfiniteDynamics1 = context.Projects.FirstOrDefault(p => p.Site == "Green Park Street" && p.Client.Name == "Infinite Dynamics Co");
                    var projectInfiniteDynamics2 = context.Projects.FirstOrDefault(p => p.Site == "Metro Center Square" && p.Client.Name == "Infinite Dynamics Co");
                    var projectEagleEyeTechnologies1 = context.Projects.FirstOrDefault(p => p.Site == "Sunset Ridge Avenue" && p.Client.Name == "EagleEye Technologies");
                    var projectEagleEyeTechnologies2 = context.Projects.FirstOrDefault(p => p.Site == "Oakwood Terrace" && p.Client.Name == "EagleEye Technologies");

                    context.Bids.AddRange(
                        new Bid
                        {
                            BidDate = DateTime.Parse("2024-04-25"),
                            BidAmount = 2500.00,
                            BidNBDApproved = true,
                            BidNBDApprovalNotes = "Approved without issues",
                            BidClientApproved = true,
                            BidClientApprovalNotes = "Client accepted the bid",
                            BidMarkForRedesign = false,
                            ReviewDate = DateTime.Parse("2024-04-22"),
                            ReviewedBy = "Jane Doe",
                            ProjectID = projectTechInnovators1.ID
                        },
                        new Bid
                        {
                            BidDate = DateTime.Parse("2024-05-15"),
                            BidAmount = 1800.00,
                            BidNBDApproved = true,
                            BidNBDApprovalNotes = "Approved with minor revisions",
                            BidClientApproved = false,
                            BidClientApprovalNotes = "Client requested further clarification",
                            BidMarkForRedesign = true,
                            ReviewDate = null,
                            ReviewedBy = null,
                            ProjectID = projectTechInnovators2.ID
                        },
                        new Bid
                        {
                            BidDate = DateTime.Parse("2024-06-05"),
                            BidAmount = 3200.00,
                            BidNBDApproved = false,
                            BidNBDApprovalNotes = "Under review by NBD team",
                            BidClientApproved = false,
                            BidClientApprovalNotes = "Awaiting client response",
                            BidMarkForRedesign = false,
                            ReviewDate = null,
                            ReviewedBy = null,
                            ProjectID = projectGlobalSolutions1.ID
                        },
                        new Bid
                        {
                            BidDate = DateTime.Parse("2024-07-01"),
                            BidAmount = 2800.00,
                            BidNBDApproved = true,
                            BidNBDApprovalNotes = "Approved without issues",
                            BidClientApproved = true,
                            BidClientApprovalNotes = "Client accepted the bid",
                            BidMarkForRedesign = false,
                            ReviewDate = DateTime.Parse("2024-06-25"),
                            ReviewedBy = "John Smith",
                            ProjectID = projectGlobalSolutions2.ID
                        },
                        new Bid
                        {
                            BidDate = DateTime.Parse("2024-08-05"),
                            BidAmount = 4000.00,
                            BidNBDApproved = true,
                            BidNBDApprovalNotes = "Approved with minor revisions",
                            BidClientApproved = false,
                            BidClientApprovalNotes = "Client requested further clarification",
                            BidMarkForRedesign = true,
                            ReviewDate = null,
                            ReviewedBy = null,
                            ProjectID = projectPinnacleEnterprises1.ID
                        },
                        new Bid
                        {
                            BidDate = DateTime.Parse("2024-09-25"),
                            BidAmount = 3500.00,
                            BidNBDApproved = true,
                            BidNBDApprovalNotes = "Approved without issues",
                            BidClientApproved = true,
                            BidClientApprovalNotes = "Client accepted the bid",
                            BidMarkForRedesign = false,
                            ReviewDate = DateTime.Parse("2024-09-20"),
                            ReviewedBy = "Alice Johnson",
                            ProjectID = projectPinnacleEnterprises2.ID
                        },
                        new Bid
                        {
                            BidDate = DateTime.Parse("2024-11-01"),
                            BidAmount = 3000.00,
                            BidNBDApproved = false,
                            BidNBDApprovalNotes = "Under review by NBD team",
                            BidClientApproved = false,
                            BidClientApprovalNotes = "Awaiting client response",
                            BidMarkForRedesign = true,
                            ReviewDate = null,
                            ReviewedBy = null,
                            ProjectID = projectInfiniteDynamics1.ID
                        },
                        new Bid
                        {
                            BidDate = DateTime.Parse("2024-12-10"),
                            BidAmount = 2800.00,
                            BidNBDApproved = false,
                            BidNBDApprovalNotes = "Under review by NBD team",
                            BidClientApproved = false,
                            BidClientApprovalNotes = "Awaiting client response",
                            BidMarkForRedesign = true,
                            ReviewDate = null,
                            ReviewedBy = null,
                            ProjectID = projectInfiniteDynamics2.ID
                        },
                        new Bid
                        {
                            BidDate = DateTime.Parse("2025-01-15"),
                            BidAmount = 3200.00,
                            BidNBDApproved = true,
                            BidNBDApprovalNotes = "Approved without issues",
                            BidClientApproved = true,
                            BidClientApprovalNotes = "Client accepted the bid",
                            BidMarkForRedesign = false,
                            ReviewDate = DateTime.Parse("2024-12-28"),
                            ReviewedBy = "Alex Johnson",
                            ProjectID = projectEagleEyeTechnologies1.ID
                        },
                        new Bid
                        {
                            BidDate = DateTime.Parse("2025-02-01"),
                            BidAmount = 3000.00,
                            BidNBDApproved = true,
                            BidNBDApprovalNotes = "Approved with minor revisions",
                            BidClientApproved = false,
                            BidClientApprovalNotes = "Client requested further clarification",
                            BidMarkForRedesign = true,
                            ReviewDate = null,
                            ReviewedBy = null,
                            ProjectID = projectEagleEyeTechnologies2.ID
                        }
                    );
                    context.SaveChanges();

                }

                if (!context.MaterialTypes.Any())
                {
                    var materialTypes = new List<MaterialType>
                    {
                        new MaterialType { MaterialTypeName= "Plants"},
                        new MaterialType { MaterialTypeName= "Pottery"},
                        new MaterialType { MaterialTypeName= "Materials"}
                    };
                    context.MaterialTypes.AddRange(materialTypes);
                    context.SaveChanges();
                }
                if (!context.Inventories.Any())
                {
                    var inventories = new List<Inventory>
                    {
                        new Inventory { Code = "lacco", Description = "lacco australasica", Size = "15 gal", ListCost = 749.00, MaterialTypeID=1 },
                        new Inventory { Code = "arenga", Description = "arenga pinnata", Size = "15 gal", ListCost = 516.00, MaterialTypeID=1 },
                        new Inventory { Code = "cham", Description = "chamaedorea", Size = "15 gal", ListCost = 499.00 , MaterialTypeID=1},
                        new Inventory { Code = "cera", Description = "ceratozamia molongo", Size = "14 in", ListCost = 400.00, MaterialTypeID=1},
                        new Inventory { Code = "areca", Description = "arecastum coco", Size = "15 gal", ListCost = 458.00, MaterialTypeID=1},
                        new Inventory { Code = "cary", Description = "caryota mitis", Size = "7 gal", ListCost = 233.00, MaterialTypeID=1 },
                        new Inventory { Code = "gmti5", Description = "green ti 5 gal", Size = "5 gal",ListCost = 154.00, MaterialTypeID=1},
                        new Inventory { Code = "gmti7", Description = "green ti 7 gal", Size = "7 gal", ListCost = 234.00, MaterialTypeID=1 },
                        new Inventory { Code = "ficus", Description = "ficus green gem", Size = "14 in",  ListCost = 150.00, MaterialTypeID=1},
                        new Inventory { Code = "ficusl7", Description = "ficus green gen 17", Size = "17 in", ListCost = 400.00, MaterialTypeID=1 },
                        new Inventory { Code = "margi", Description = "marginata", Size = "2 gal", ListCost = 75.00, MaterialTypeID=1 },
                        new Inventory { Code = "TCP50", Description = "t/c pot 50 gal", Size = "50 gal",  ListCost = 110.95, MaterialTypeID=2},
                         new Inventory { Code = "GP50", Description = "granite pot 50 gal", Size = "50 gal",  ListCost = 195.00, MaterialTypeID=2 },
                         new Inventory { Code = "TCF03", Description = "t/c figure-swan", Size = "Not specified", ListCost = 45.00, MaterialTypeID=2 },
                         new Inventory { Code = "MBB30", Description = "marble bird bath 30 in", Size = "30 in", ListCost = 250.00, MaterialTypeID=2 },
                         new Inventory { Code = "GFN48", Description = "granite foundation 48 in", Size = "48 in", ListCost = 750.00, MaterialTypeID=2 },
                         new Inventory { Code = "CBRK5", Description = "Decorative cedar bark", Size = "Bag (5 cu ft)", ListCost = 15.95, MaterialTypeID= 3},
                        new Inventory { Code = "CRGRN", Description = "Crushed granite",Size = "Not specified", ListCost = 14.00, MaterialTypeID=3},
                        new Inventory { Code = "PGRV", Description = "Pea gravel",Size = "Not specified", ListCost = 20.00, MaterialTypeID=3 },
                        new Inventory { Code = "GRV1", Description = "1\" gravel", Size = "Not specified",ListCost = 12.00, MaterialTypeID=3 },
                        new Inventory { Code = "TSOIL", Description = "Topsoil", Size = "Not specified",ListCost = 20.00, MaterialTypeID=3},
                        new Inventory { Code = "PBLKG", Description = "Patio block-grey",Size = "Not specified", ListCost = 0.84, MaterialTypeID=3 },
                        new Inventory { Code = "PBLKR", Description = "Patio block-red",Size = "Not specified", ListCost = 0.84, MaterialTypeID=3 }





                    };
                    context.Inventories.AddRange(inventories);
                    context.SaveChanges();
                }

                if (!context.BidInventories.Any())
                {
                    var bidId = 4; // Replace /* some condition */ with your actual condition to find the bid ID

                    context.BidInventories.AddRange(
                        new BidInventory
                        {
                            InventoryID = context.Inventories.FirstOrDefault(i => i.Code == "lacco").ID,
                            BidID = bidId,
                            MaterialQuantity = 1,
                            MaterialExtendPrice = 749.0,
                        },
                        new BidInventory
                        {
                            InventoryID = context.Inventories.FirstOrDefault(i => i.Code == "arenga").ID,
                            BidID = bidId,
                            MaterialQuantity = 2,
                            MaterialExtendPrice = 200.0,
                        },
                        new BidInventory
                        {
                            InventoryID = context.Inventories.FirstOrDefault(i => i.Code == "cham").ID,
                            BidID = bidId,
                            MaterialQuantity = 3,
                            MaterialExtendPrice = 450.0,
                        });
                    context.SaveChanges();
                }
                if (!context.LabourTypeInfos.Any())
                {
                    var labourTypeInfos = new List<LabourTypeInfo>
                    {
                        new LabourTypeInfo {  LabourTypeName = "Production Worker ", PricePerHour=30.0,CostPerHour=18.0},
                        new LabourTypeInfo {  LabourTypeName = "Designer ", PricePerHour=65.0,CostPerHour=40.0},
                        new LabourTypeInfo {  LabourTypeName = "Equipment Operator ", PricePerHour=65.0,CostPerHour=45.0},
                        new LabourTypeInfo {  LabourTypeName = "Botanist ", PricePerHour=75.0,CostPerHour=50.0}
                    };
                    context.LabourTypeInfos.AddRange(labourTypeInfos);
                    context.SaveChanges();
                }
				if (!context.StaffRoles.Any())
				{
					var staffRoles = new List<StaffRole>
					{
						new StaffRole { StaffRoleName= "Designer"}
					};
					context.StaffRoles.AddRange(staffRoles);
					context.SaveChanges();
				}
				if (!context.Staffs.Any())
				{
					var staffs = new List<Staff>
					{
						new Staff { StaffFirstName = "Tamara",
                            StaffLastName = "Bakken",
                            Email = "tamara@example.com", 
                            Phone = "1234567890", 
                            StaffRoleID = 1},
                        new Staff { StaffFirstName = "Sara",
                            StaffLastName = "Bakken",
                            Email = "tamara@examplse.com",
                            Phone = "1234557890",
                            StaffRoleID = 1}
                    };
					context.Staffs.AddRange(staffs);
					context.SaveChanges();
				}
				//BidStaff
				if (!context.BidsStaffs.Any())
				{
                    context.BidsStaffs.AddRange(
                        new BidStaff
                        {
                            StaffID = context.Staffs.FirstOrDefault(c => c.StaffFirstName == "Tamara" && c.StaffLastName == "Bakken").ID,
                            BidID = 4

                        });
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
