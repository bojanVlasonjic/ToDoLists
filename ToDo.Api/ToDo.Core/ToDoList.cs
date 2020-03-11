using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Core
{
    public class ToDoList
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public List<ToDoItem> Items { get; set; }

        public bool IsReminded { get; set; }

        public DateTime? EndDate { get; set; }

        public int Position { get; set; }

        public string Owner { get; set; }

        public List<ToDoSharedList> SharedLists { get; set; }

        public ToDoList()
        {
            Items = new List<ToDoItem>();
        }


        public void UpdateData(ToDoList updatedList)
        {
            Title = updatedList.Title;
            IsReminded = updatedList.IsReminded;
            EndDate = updatedList.EndDate;
        }

        
    }
}
