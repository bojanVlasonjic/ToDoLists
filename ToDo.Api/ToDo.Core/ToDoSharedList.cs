using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Core
{
    public class ToDoSharedList
    {
        public Guid Id { get; set; }

        public DateTime TimeOfSharing { get; set; }

        public Guid ToDoListId { get; set; }

        public ToDoList ToDoList { get; set; }

        public ToDoSharedList()
        {

        }


        public ToDoSharedList(DateTime timeOfSharing, Guid ToDoListId)
        {
            this.TimeOfSharing = timeOfSharing;
            this.ToDoListId = ToDoListId;
        }
    }
}
