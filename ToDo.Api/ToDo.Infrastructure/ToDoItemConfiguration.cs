using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Core;

namespace ToDo.Infrastructure
{
    public class ToDoItemConfiguration: IEntityTypeConfiguration<ToDoItem>
    {

        public void Configure(EntityTypeBuilder<ToDoItem> builder)
        {

            builder.HasKey(item => item.Id);

            builder.
                Property(item => item.Content).
                IsRequired();

            builder.
                Property(item => item.Position)
                .HasDefaultValue(0);

            

        }

    }
}
