﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NBD4.Data;

#nullable disable

namespace NBD4.Data.NBDMigrations
{
    [DbContext(typeof(NBDContext))]
    partial class NBDContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.16");

            modelBuilder.Entity("NBD4.Models.Bid", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("BidAmount")
                        .HasColumnType("REAL");

                    b.Property<string>("BidClientApprovalNotes")
                        .HasColumnType("TEXT");

                    b.Property<bool>("BidClientApproved")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("BidDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("BidMarkForRedesign")
                        .HasColumnType("INTEGER");

                    b.Property<string>("BidMarkForRedisignNotes")
                        .HasColumnType("TEXT");

                    b.Property<string>("BidNBDApprovalNotes")
                        .HasColumnType("TEXT");

                    b.Property<bool>("BidNBDApproved")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProjectID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ReviewDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReviewedBy")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("NBD4.Models.BidInventory", b =>
                {
                    b.Property<int>("BidID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InventoryID")
                        .HasColumnType("INTEGER");

                    b.Property<double>("MaterialExtendPrice")
                        .HasColumnType("REAL");

                    b.Property<int>("MaterialQuantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("BidID", "InventoryID");

                    b.HasIndex("InventoryID");

                    b.ToTable("BidInventories");
                });

            modelBuilder.Entity("NBD4.Models.BidLabourTypeInfo", b =>
                {
                    b.Property<int>("LabourTypeInfoID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BidID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Hours")
                        .HasColumnType("INTEGER");

                    b.Property<double>("LabourCharge")
                        .HasColumnType("REAL");

                    b.HasKey("LabourTypeInfoID", "BidID");

                    b.HasIndex("BidID");

                    b.ToTable("BidLabourTypeInfos");
                });

            modelBuilder.Entity("NBD4.Models.BidStaff", b =>
                {
                    b.Property<int>("StaffID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BidID")
                        .HasColumnType("INTEGER");

                    b.HasKey("StaffID", "BidID");

                    b.HasIndex("BidID");

                    b.ToTable("BidsStaffs");
                });

            modelBuilder.Entity("NBD4.Models.City", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProvinceID")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ProvinceID");

                    b.HasIndex("Name", "ProvinceID")
                        .IsUnique();

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("NBD4.Models.Client", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CityID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ContactFirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactLastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactMiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CityID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("NBD4.Models.Employee", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("NBD4.Models.Inventory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<double>("ListCost")
                        .HasColumnType("REAL");

                    b.Property<int>("MaterialTypeID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("MaterialTypeID");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("NBD4.Models.LabourTypeInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("CostPerHour")
                        .HasColumnType("REAL");

                    b.Property<string>("LabourTypeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<double>("PricePerHour")
                        .HasColumnType("REAL");

                    b.HasKey("ID");

                    b.ToTable("LabourTypeInfos");
                });

            modelBuilder.Entity("NBD4.Models.MaterialType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaterialTypeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("MaterialTypes");
                });

            modelBuilder.Entity("NBD4.Models.Project", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<int>("ClientID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Site")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("NBD4.Models.Province", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("NBD4.Models.Staff", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("StaffFirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("StaffLastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("StaffMiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("StaffRoleID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("StaffRoleID");

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("NBD4.Models.StaffRole", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("StaffRoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("StaffRoles");
                });

            modelBuilder.Entity("NBD4.Models.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PushAuth")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("PushEndpoint")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("PushP256DH")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeID");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("NBD4.Models.Bid", b =>
                {
                    b.HasOne("NBD4.Models.Project", "Project")
                        .WithMany("Bids")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("NBD4.Models.BidInventory", b =>
                {
                    b.HasOne("NBD4.Models.Bid", "Bid")
                        .WithMany("BidInventories")
                        .HasForeignKey("BidID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBD4.Models.Inventory", "Inventory")
                        .WithMany("BidInventories")
                        .HasForeignKey("InventoryID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Bid");

                    b.Navigation("Inventory");
                });

            modelBuilder.Entity("NBD4.Models.BidLabourTypeInfo", b =>
                {
                    b.HasOne("NBD4.Models.Bid", "Bid")
                        .WithMany("BidLabourTypeInfos")
                        .HasForeignKey("BidID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBD4.Models.LabourTypeInfo", "LabourTypeInfo")
                        .WithMany("BidLabourTypeInfos")
                        .HasForeignKey("LabourTypeInfoID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Bid");

                    b.Navigation("LabourTypeInfo");
                });

            modelBuilder.Entity("NBD4.Models.BidStaff", b =>
                {
                    b.HasOne("NBD4.Models.Bid", "Bid")
                        .WithMany("BidStaffs")
                        .HasForeignKey("BidID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBD4.Models.Staff", "Staff")
                        .WithMany("BidStaffs")
                        .HasForeignKey("StaffID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Bid");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("NBD4.Models.City", b =>
                {
                    b.HasOne("NBD4.Models.Province", "Province")
                        .WithMany("Cities")
                        .HasForeignKey("ProvinceID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("NBD4.Models.Client", b =>
                {
                    b.HasOne("NBD4.Models.City", "City")
                        .WithMany("Clients")
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("NBD4.Models.Inventory", b =>
                {
                    b.HasOne("NBD4.Models.MaterialType", "MaterialType")
                        .WithMany("Inventories")
                        .HasForeignKey("MaterialTypeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("MaterialType");
                });

            modelBuilder.Entity("NBD4.Models.Project", b =>
                {
                    b.HasOne("NBD4.Models.Client", "Client")
                        .WithMany("Projects")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("NBD4.Models.Staff", b =>
                {
                    b.HasOne("NBD4.Models.StaffRole", "StaffRole")
                        .WithMany("Staffs")
                        .HasForeignKey("StaffRoleID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("StaffRole");
                });

            modelBuilder.Entity("NBD4.Models.Subscription", b =>
                {
                    b.HasOne("NBD4.Models.Employee", "Employee")
                        .WithMany("Subscriptions")
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("NBD4.Models.Bid", b =>
                {
                    b.Navigation("BidInventories");

                    b.Navigation("BidLabourTypeInfos");

                    b.Navigation("BidStaffs");
                });

            modelBuilder.Entity("NBD4.Models.City", b =>
                {
                    b.Navigation("Clients");
                });

            modelBuilder.Entity("NBD4.Models.Client", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("NBD4.Models.Employee", b =>
                {
                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("NBD4.Models.Inventory", b =>
                {
                    b.Navigation("BidInventories");
                });

            modelBuilder.Entity("NBD4.Models.LabourTypeInfo", b =>
                {
                    b.Navigation("BidLabourTypeInfos");
                });

            modelBuilder.Entity("NBD4.Models.MaterialType", b =>
                {
                    b.Navigation("Inventories");
                });

            modelBuilder.Entity("NBD4.Models.Project", b =>
                {
                    b.Navigation("Bids");
                });

            modelBuilder.Entity("NBD4.Models.Province", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("NBD4.Models.Staff", b =>
                {
                    b.Navigation("BidStaffs");
                });

            modelBuilder.Entity("NBD4.Models.StaffRole", b =>
                {
                    b.Navigation("Staffs");
                });
#pragma warning restore 612, 618
        }
    }
}
