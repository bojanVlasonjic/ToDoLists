using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using ToDo.Core;

namespace ToDo.Api.DTOs
{
    public class ToDoListDTO
    {

        public Guid Id { get; set; }

        public string Title { get; set; }

        public List<ToDoItemDTO> DTOItems { get; set; }

        public int Position { get; set; }

        public bool IsReminded { get; set; }

        public DateTime? EndDate { get; set; }



        public ToDoListDTO()
        {
            DTOItems = new List<ToDoItemDTO>();
        }

        public ToDoListDTO(ToDoList toDoList)
        {
            Id = toDoList.Id;
            Title = toDoList.Title;
            Position = toDoList.Position;
            IsReminded = toDoList.IsReminded;
            EndDate = toDoList.EndDate;

            DTOItems = new List<ToDoItemDTO>();

            toDoList.Items.ForEach(item => DTOItems.Add(new ToDoItemDTO(item)));
        }


        public ToDoList CreateEntity(string email)
        {
             return new ToDoList
            {
                Title = this.Title,
                IsReminded = this.IsReminded,
                EndDate = this.EndDate,
                Owner = email,
                Items = new List<ToDoItem>()
            };

        }

    }
}
