using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ToDo.Api.DTOs;
using ToDo.Api.Exceptions;
using ToDo.Infrastructure;
using ToDo.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ToDo.Api.Services
{
    public class ToDoListService: IToDoListService {


        private readonly ToDoDbContext _context;
        private readonly ILogger<ToDoListService> _logger;

        public ToDoListService(ToDoDbContext context, ILogger<ToDoListService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public List<ToDoListDTO> GetToDoLists(string email)
        {

            _logger.LogInformation("ToDoListService -> Executing GetToDoLists()");

            //return todolists with list positions in descending order
            var toDoLists = _context.ToDoLists
                 .Include(x => x.Items)
                 .Where(x => x.Owner.Equals(email))
                 .OrderByDescending(x => x.Position)
                 .ToList();

            //sort list items in ascending order
            toDoLists.ForEach(
                list => list.Items = list.Items
                    .OrderBy(item => item.IsCompleted)
                    .ThenBy(item => item.Position)
                    .ToList()
                );

            List<ToDoListDTO> listsToReturn = new List<ToDoListDTO>();

            toDoLists.ForEach(todo => listsToReturn.Add(new ToDoListDTO(todo)));

            return listsToReturn;
            
        }

        public ToDoListDTO GetToDoListById(Guid id, string email)
        {

            _logger.LogInformation("ToDoListService -> Executing GetToDoListById()");

            var toDoList =  _context.ToDoLists
                .Include(todo => todo.Items)
                .FirstOrDefault(todo => todo.Id.Equals(id) && todo.Owner.Equals(email));

            //sort list items in ascending order
            toDoList.Items = toDoList.Items
                .OrderBy(item => item.IsCompleted)
                .ThenBy(item => item.Position)
                .ToList();

            if (toDoList == null)
            {
                throw new EntityNotFoundException();
            }

            return new ToDoListDTO(toDoList);
        }


        public ToDoListDTO CreateToDoList(ToDoListDTO toDoListDTO, string email)
        {
            _logger.LogInformation("ToDoListService -> Executing CreateToDoList()");

            //if the user didn't specify the date for the reminder
            if (toDoListDTO.EndDate == null)
            {
                toDoListDTO.IsReminded = false; //the list is not a reminded list
            } else
            {
                toDoListDTO.IsReminded = true;
            }

            ToDoList newList = toDoListDTO.CreateEntity(email);

            //new todo list position is 'highest' in the hierarchy for the specified user
            newList.Position = _context.ToDoLists
                .Where(x => x.Owner.Equals(email))
                .Count(); 

            _context.ToDoLists.Add(newList);
           _context.SaveChanges();

            toDoListDTO.Id = newList.Id;
            toDoListDTO.Position = newList.Position;

            return toDoListDTO;
        }


        public ToDoListDTO UpdateToDoList(Guid id, ToDoListDTO toDoListDTO, string email)
        {

            _logger.LogInformation("ToDoListService -> Executing UpdateToDoList()");

            //if the user didn't specify the date for the reminder
            if (toDoListDTO.EndDate == null)
            {
                toDoListDTO.IsReminded = false; //the list is not a reminded list
            }
            else
            {
                toDoListDTO.IsReminded = true;
            }

            ToDoList listToUpdate = _context
                .ToDoLists
                .Include(todo => todo.Items)
                .SingleOrDefault(todo => todo.Id.Equals(id) && todo.Owner.Equals(email));
                    
            if(listToUpdate == null)
            {
                throw new EntityNotFoundException();
            }

            listToUpdate.UpdateData(toDoListDTO.CreateEntity(email));
            _context.SaveChanges();
            return new ToDoListDTO(listToUpdate); //return an updated dto

        }


        public ToDoListDTO UpdatePosition(Guid id, int newPosition, string email)
        {

            _logger.LogInformation("ToDoListService -> Executing UpdatePosition()");

            //positions are indexed from 0, to Count()-1
            if (newPosition < 0 || newPosition >_context.ToDoLists.Count()-1)
            {
                throw new BadRequestException();
            }

            var listToUpdate = _context
                .ToDoLists
                .Include(todo => todo.Items)
                .SingleOrDefault(todo => todo.Id.Equals(id) && todo.Owner.Equals(email));

           
            if (listToUpdate == null)
            {
                throw new EntityNotFoundException();
            }

            //There are two cases when updating positions

            if(newPosition > listToUpdate.Position) //decrement all positions between new position and current position
            {
                _context.ToDoLists
                    .Where(x => x.Position > listToUpdate.Position && x.Position <= newPosition && x.Owner.Equals(email))
                    .ToList()
                    .ForEach(x => x.Position--);

            } else if (newPosition < listToUpdate.Position) //increment all positions between new position and current position
            {
                _context.ToDoLists
                    .Where(x => x.Position >= newPosition && x.Position < listToUpdate.Position && x.Owner.Equals(email))
                    .ToList()
                    .ForEach(x => x.Position++);
            }

            listToUpdate.Position = newPosition;
            _context.SaveChanges();

            return new ToDoListDTO(listToUpdate);
        }


        public void DeleteToDoList(Guid id, string email)
        {
            _logger.LogInformation("ToDoListService -> Executing DeleteToDoList()");

            ToDoList listToDelete = _context.ToDoLists
                                    .SingleOrDefault(todo => todo.Id.Equals(id) && todo.Owner.Equals(email));

            if (listToDelete == null)
            {
                throw new EntityNotFoundException();
            }

            //decrementing positions of all todo lists after the one I'm deleting 
            _context.ToDoLists
                .Where(list => list.Position > listToDelete.Position && list.Owner.Equals(email))
                .ToList()
                .ForEach( list => list.Position--);

            _context.ToDoLists.Remove(listToDelete);
            _context.SaveChanges();

        }

        public List<ToDoListDTO> SearchToDoLists(string title, string email)
        {

            List<ToDoListDTO> matchingDtos = new List<ToDoListDTO>();

            if(title != null)
            {
                _context.ToDoLists
                    .Include(x => x.Items)
                    .Where(x => x.Title != null && x.Title.Contains(title) && x.Owner.Equals(email))
                    .ToList()
                    .ForEach(list => matchingDtos.Add(new ToDoListDTO(list)));
            }
          
            return matchingDtos;
        }


        public Guid CreateSharedList(Guid toDoListId)
        {

            // get all shared references of the list
            var listShares = _context.ToDoSharedLists
                .Where(x => x.ToDoListId == toDoListId)
                .Include(x => x.ToDoList)
                .ToList();

            DateTime currentTime = DateTime.Now;
            
            // check if the todo list is already being shared
            foreach (ToDoSharedList sharedList in listShares)
            {
                if((currentTime - sharedList.TimeOfSharing).TotalHours < 0.5)
                {
                    return sharedList.Id;
                }
            }

            // check if the todo list id is valid
            var toDoList = _context.ToDoLists
                .Where(list => list.Id.Equals(toDoListId))
                .SingleOrDefault();

            if(toDoList == null)
            {
                throw new EntityNotFoundException();
            }

            // create new share reference
            ToDoSharedList newSharedList = new ToDoSharedList(currentTime, toDoListId);
            newSharedList.ToDoList = toDoList;

            _context.ToDoSharedLists.Add(newSharedList);
            _context.SaveChanges();

            return newSharedList.Id;
        }


        public ToDoListDTO GetSharedList(Guid id)
        {

            var sharedList = _context.ToDoSharedLists
                .Where(x => x.Id == id)
                .Include(x => x.ToDoList)
                .Include(x => x.ToDoList.Items)
                .FirstOrDefault();

            if(sharedList == null)
            {
                throw new EntityNotFoundException();
            }

            // check if the shared list expired
            if((DateTime.Now - sharedList.TimeOfSharing).TotalHours > 0.5)
            {
                throw new BadRequestException();
            }


            return new ToDoListDTO(sharedList.ToDoList);

        }


    }
}

