﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserManagerSite.Application.Data;

#nullable disable

namespace UserManagerSite.Application.Migrations
{
    [DbContext(typeof(UserManagerSiteContext))]
    partial class UserManagerSiteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("UserManagerSite.Application.Models.Role", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("UserManagerSite.Application.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("birthdate")
                        .HasColumnType("TEXT");

                    b.Property<string>("email")
                        .HasColumnType("TEXT");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("roleid")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("roleid");

                    b.ToTable("User");
                });

            modelBuilder.Entity("UserManagerSite.Application.Models.User", b =>
                {
                    b.HasOne("UserManagerSite.Application.Models.Role", "role")
                        .WithMany("Users")
                        .HasForeignKey("roleid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("role");
                });

            modelBuilder.Entity("UserManagerSite.Application.Models.Role", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
