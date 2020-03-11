using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ToDo.Api.DTOs;

namespace ToDo.Api.Services
{
    public interface IToDoItemService
    {

        List<ToDoItemDTO> GetItemsForToDoList(Guid listId);


        ToDoItemDTO CreateItemInList(Guid listId, ToDoItemDTO itemDTO);

        ToDoItemDTO UpdateItemInList(Guid listId, ToDoItemDTO itemDTO);

        ToDoItemDTO UpdatePosition(Guid listId, Guid itemId, int position);

        void DeleteItemFromList(Guid listId, Guid itemId);


    }
}
