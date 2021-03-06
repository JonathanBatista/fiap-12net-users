﻿using GeekBurger.Users.Core.Domains;
using Microsoft.EntityFrameworkCore;

namespace GeekBurger.Users.Data.Context
{


    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRestriction> UserRestrictions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=users.db");
        }
    }
}
