using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using ToDo.Core;

namespace ToDo.Api.DTOs
{
    public class ToDoItemDTO
    { 

        public Guid ItemId { get; set; }

        [Required(AllowEmptyStrings = false), StringLength(100)]
        public string Content { get; set; }

        public int Position { get; set; }

        public bool IsCompleted { get; set; }

        public ToDoItemDTO()
        {

        }


        public ToDoItemDTO(ToDoItem item)
        {
            ItemId = item.Id;
            Content = item.Content;
            Position = item.Position;
            IsCompleted = item.IsCompleted;

        }

        public ToDoItem CreateEntity(Guid listId)
        {
            return new ToDoItem
            {
                Content = this.Content,
                Position = this.Position,
                ToDoListId = listId,
                IsCompleted = false
            };
        }

    }
}
