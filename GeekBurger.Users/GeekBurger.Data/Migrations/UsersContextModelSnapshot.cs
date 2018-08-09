﻿// <auto-generated />
using System;
using GeekBurger.Users.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeekBurger.Users.Data.Migrations
{
    [DbContext(typeof(UsersContext))]
    partial class UsersContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846");

            modelBuilder.Entity("GeekBurger.Users.Core.Domains.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FaceBase64");

                    b.Property<string>("GuidReference");

                    b.Property<bool>("InProcessing");

                    b.Property<Guid>("PersistedId");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Users","gbu");
                });

            modelBuilder.Entity("GeekBurger.Users.Core.Domains.UserRestriction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Ingredient")
                        .HasMaxLength(100);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserRestrictions","gbu");
                });

            modelBuilder.Entity("GeekBurger.Users.Core.Domains.UserRestriction", b =>
                {
                    b.HasOne("GeekBurger.Users.Core.Domains.User", "User")
                        .WithMany("Restrictions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
