using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ToDo.Core;

namespace ToDo.Infrastructure
{
    public class ToDoSharedListConfiguration : IEntityTypeConfiguration<ToDoSharedList>
    {
        public void Configure(EntityTypeBuilder<ToDoSharedList> builder)
        {
            builder.HasKey(sharedList => sharedList.Id);

            builder
                .Property(sharedList => sharedList.TimeOfSharing)
                .IsRequired(true);

            builder
                .Property(sharedList => sharedList.TimeOfSharing)
                .IsRequired(true);


            builder
                .HasOne(sharedList => sharedList.ToDoList)
                .WithMany(x => x.SharedLists)
                .HasForeignKey(x => x.ToDoListId);

        }
    }
}
