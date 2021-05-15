﻿// <auto-generated />
using System;
using Ecommerce_NetCore_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ecommerce_NetCore_API.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ecommerce_NetCore_API.Models.BillsDatasTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Billamount")
                        .HasColumnType("int");

                    b.Property<byte[]>("Billbytearray")
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("Billdate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Billnumber")
                        .HasColumnType("int");

                    b.Property<int>("Billprofit")
                        .HasColumnType("int");

                    b.Property<int>("Customerid")
                        .HasColumnType("int");

                    b.Property<int>("Deduction")
                        .HasColumnType("int");

                    b.Property<bool>("Ispaid")
                        .HasColumnType("bit");

                    b.Property<int>("Payableamount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("billscollections");
                });

            modelBuilder.Entity("Ecommerce_NetCore_API.Models.BillsPendingTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Billnumber")
                        .HasColumnType("int");

                    b.Property<int>("Customerid")
                        .HasColumnType("int");

                    b.Property<bool>("Iscompleted")
                        .HasColumnType("bit");

                    b.Property<int>("Pendingamount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("billspending");
                });

            modelBuilder.Entity("Ecommerce_NetCore_API.Models.CustomerTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Customeraddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("customermobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("customers");
                });

            modelBuilder.Entity("Ecommerce_NetCore_API.Models.CustomerTxHistoryTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Billnumber")
                        .HasColumnType("int");

                    b.Property<int>("Customerid")
                        .HasColumnType("int");

                    b.Property<int>("Paidamount")
                        .HasColumnType("int");

                    b.Property<DateTime>("Paiddate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Paymentmode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("customertxhistory");
                });

            modelBuilder.Entity("Ecommerce_NetCore_API.Models.LoginDataTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("isLocked")
                        .HasColumnType("bit");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("loginDatas");
                });

            modelBuilder.Entity("Ecommerce_NetCore_API.Models.ProdAddCategoryTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("Ecommerce_NetCore_API.Models.ProdAddHistoryTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Size")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("prodAddHistoryData");
                });

            modelBuilder.Entity("Ecommerce_NetCore_API.Models.ProductTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("products");
                });

            modelBuilder.Entity("Ecommerce_NetCore_API.Models.ProductWithCategoryIdsTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("productWithCategoryIds");
                });

            modelBuilder.Entity("Ecommerce_NetCore_API.Models.SalewithCustIdTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Custid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prodsize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Productid")
                        .HasColumnType("int");

                    b.Property<string>("Productname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Purchasedate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalCost")
                        .HasColumnType("int");

                    b.Property<int>("Unitprice")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("saleswithCustomerIds");
                });

            modelBuilder.Entity("Ecommerce_NetCore_API.Models.StockTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Size")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("stocks");
                });
#pragma warning restore 612, 618
        }
    }
}
