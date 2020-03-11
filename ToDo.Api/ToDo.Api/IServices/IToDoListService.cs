using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ToDo.Api.DTOs;

namespace ToDo.Api.Services
{
    public interface IToDoListService
    {

        List<ToDoListDTO> GetToDoLists(string email);

        ToDoListDTO GetToDoListById(Guid id, string email);

        ToDoListDTO CreateToDoList(ToDoListDTO toDoListDTO, string email);

        ToDoListDTO UpdateToDoList(Guid id, ToDoListDTO toDoListDTO, string email);

        void DeleteToDoList(Guid id, string email);

        List<ToDoListDTO> SearchToDoLists(string title, string email);

        ToDoListDTO UpdatePosition(Guid id, int position, string email);

        Guid CreateSharedList(Guid toDoListId);

        ToDoListDTO GetSharedList(Guid id);

    }
}
