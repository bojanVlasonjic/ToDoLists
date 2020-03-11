using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Core
{
    public class ToDoItem
    {

        public Guid Id { get; set; }
        public string Content { get; set; }

        public Guid ToDoListId { get; set; }

        public ToDoList ToDoList { get; set; }

        public int Position { get; set; }

        public bool IsCompleted { get; set; }

        public ToDoItem()
        {
        }

        
    }
}
