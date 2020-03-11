using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using ToDo.Core;

namespace ToDo.Infrastructure
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }

        public DbSet<ToDoSharedList> ToDoSharedLists { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoList>().ToTable("ToDoLists");
            modelBuilder.Entity<ToDoItem>().ToTable("ToDoItems");
            modelBuilder.Entity<ToDoSharedList>().ToTable("ToDoSharedLists");

            modelBuilder.ApplyConfiguration(new ToDoListConfiguration());
            modelBuilder.ApplyConfiguration(new ToDoItemConfiguration());
        }


    }
}
