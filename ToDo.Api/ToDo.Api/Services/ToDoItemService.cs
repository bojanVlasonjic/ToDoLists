using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using ToDo.Api.DTOs;
using ToDo.Api.Exceptions;
using ToDo.Core;
using ToDo.Infrastructure;


namespace ToDo.Api.Services
{
    public class ToDoItemService: IToDoItemService
    {

        private readonly ToDoDbContext _context;
        private readonly ILogger<ToDoItemService> _logger;

        public ToDoItemService(ToDoDbContext context, ILogger<ToDoItemService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public List<ToDoItemDTO> GetItemsForToDoList(Guid listId)
        {

            _logger.LogInformation("ToDoItemService -> Executing GetItemsForToDoList()");

            var todoItems = _context.ToDoItems
                .Where(item => item.ToDoListId.Equals(listId))
                .OrderBy(item => item.Position)
                .ToList();

            List<ToDoItemDTO> todoItemsDTO = new List<ToDoItemDTO>();

            //iterate through items, create their dto's and populate the todoItemsDTO list
            todoItems.ForEach(item => todoItemsDTO.Add(new ToDoItemDTO(item)));

            return todoItemsDTO;
          
        }


   
        public ToDoItemDTO CreateItemInList(Guid listId, ToDoItemDTO itemDTO)
        {
            _logger.LogInformation("ToDoItemService -> Executing CreateItemInList()");

            var toDoList = _context.ToDoLists
                .SingleOrDefault(todo => todo.Id.Equals(listId)); ;

            if(toDoList == null)
            {
                throw new EntityNotFoundException();
            }

            //create item from dto
            ToDoItem itemToAdd = itemDTO.CreateEntity(listId);

            //each new Item receives the lowest position of all items in a list
            itemToAdd.Position = _context.ToDoItems.Where(x => x.ToDoListId.Equals(listId)).Count();

            //add the new item to list and update database
            toDoList.Items.Add(itemToAdd);
            _context.SaveChanges();

            //update dto data
            itemDTO.ItemId = itemToAdd.Id; 
            itemDTO.Position = itemToAdd.Position;

            return itemDTO;            
        }

       
        public ToDoItemDTO UpdateItemInList(Guid listId, ToDoItemDTO itemDTO)
        {
            _logger.LogInformation("ToDoItemService -> Executing UpdateItemInList()");

            var itemToUpdate = _context.ToDoItems
                .SingleOrDefault(item => item.ToDoListId.Equals(listId) && item.Id.Equals(itemDTO.ItemId));

            if(itemToUpdate == null)
            {
                throw new EntityNotFoundException();
            }

            itemToUpdate.Content = itemDTO.Content;
            itemToUpdate.IsCompleted = itemDTO.IsCompleted;

            _context.SaveChanges();

            itemDTO.Position = itemToUpdate.Position;
            return itemDTO;
        }


        public ToDoItemDTO UpdatePosition(Guid listId, Guid itemId, int newPosition)
        {
            _logger.LogInformation("ToDoItemService -> Executing UpdatePosition()");

            ToDoItem itemToUpdate = _context.ToDoItems
                .SingleOrDefault(item => item.ToDoListId.Equals(listId) && item.Id.Equals(itemId));

            if (itemToUpdate == null)
            {
                throw new EntityNotFoundException();
            }

            //extracting the number of items for the specified todo list
            int numOfItems = _context.ToDoItems
                .Where(x => x.ToDoListId.Equals(listId))
                .Count();

            //positions are indexed from 0, to Count()-1
            if (newPosition < 0 || newPosition > numOfItems - 1)
            {
                throw new BadRequestException();
            }

            //There are two cases when updating a position

            if (newPosition > itemToUpdate.Position)
            {

                _context.ToDoItems
                    .Where(x => x.Position > itemToUpdate.Position && x.Position <= newPosition)
                    .ToList()
                    .ForEach(x => x.Position--);

            } else if(newPosition < itemToUpdate.Position)
            {
                _context.ToDoItems
                    .Where(x => x.Position >= newPosition && x.Position < itemToUpdate.Position)
                    .ToList()
                    .ForEach(x => x.Position++);
            }

            itemToUpdate.Position = newPosition;
            _context.SaveChanges();

            return new ToDoItemDTO(itemToUpdate);
        }



        public void DeleteItemFromList(Guid listId, Guid itemId)
        {
            _logger.LogInformation("ToDoItemService -> Executing DeleteItemInList()");

            var toDoList = _context.ToDoLists
                .Include(list => list.Items)
                .Where(list => list.Id.Equals(listId))
                .SingleOrDefault();

            var itemToDelete = toDoList.Items
                .Where(item => item.Id.Equals(itemId))
                .SingleOrDefault();

            if(itemToDelete == null || toDoList == null)
            {
                throw new EntityNotFoundException();
            }

            //decrementing positions of all todo items after the one I'm deleting 
            toDoList.Items
                .Where(item => item.Position > itemToDelete.Position)
                .ToList()
                .ForEach(item => item.Position--);

            //removing item from list
            toDoList.Items.Remove(itemToDelete);
            _context.SaveChanges();
        }


    }
}
