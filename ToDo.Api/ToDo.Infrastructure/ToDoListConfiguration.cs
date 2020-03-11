using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Core;

namespace ToDo.Infrastructure
{
    public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList>
    {

        public void Configure(EntityTypeBuilder<ToDoList> builder)
        {

            builder.HasKey(toDoList => toDoList.Id);

            builder
                .Property(toDoList => toDoList.Title)
                .IsRequired();

            builder
               .Property(toDoList => toDoList.Position)
               .HasDefaultValue(0);

            builder
                .Property(toDoList => toDoList.IsReminded)
                .IsRequired()
                .HasDefaultValue(false);

            builder
                .Property(toDoList => toDoList.EndDate)
                .IsRequired(false);

            builder
                .Property(toDoList => toDoList.Owner)
                .IsRequired();

            builder.
                HasMany(todoList => todoList.Items).
                WithOne(item => item.ToDoList).
                HasForeignKey(item => item.ToDoListId);


        }
    }
}
