using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using ToDo.Api.Services;
using ToDo.Api.DTOs;
using ToDo.Api.Exceptions;
using Microsoft.AspNetCore.Authorization;
using ToDo.Api.Extensions;

namespace ToDo.Api.Controllers
{

    [Route("api/to-do-list")]
    [ApiController]
    [EnableCors("ToDoPolicy")]
    public class ToDoListController : ControllerBase
    {

        private readonly IToDoListService _service;


        public ToDoListController(IToDoListService service)
        {
            _service = service;
        }


        [HttpGet]
        [Authorize("read:to-do-list")]
        public IActionResult GetToDoLists()
        {
            return Ok(_service.GetToDoLists(User.GetEmail()));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize("read:to-do-list")]
        public IActionResult GetToDoList([FromRoute]Guid id)
        {
            ToDoListDTO toDoListDTO;

            try
            {
                toDoListDTO =_service.GetToDoListById(id, User.GetEmail());    
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }

            return Ok(toDoListDTO);
        }


  
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize("write:to-do-list")]
        public IActionResult CreateToDoList([FromBody] ToDoListDTO toDoListDTO)
        {
            ToDoListDTO newListDTO = _service.CreateToDoList(toDoListDTO, User.GetEmail());
            return CreatedAtAction(nameof(GetToDoList), new { id = newListDTO.Id }, newListDTO);
 
        }


        [HttpPut("{id}/{position}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize("write:to-do-list")]
        public IActionResult UpdatePosition([FromRoute]Guid id, [FromRoute]int position)
        {
            ToDoListDTO updatedDTO;

            try
            {
                updatedDTO = _service.UpdatePosition(id, position, User.GetEmail());
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
            catch(BadRequestException)
            {
                return BadRequest();
            }

            
            return Ok(updatedDTO);
        }



        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize("write:to-do-list")]
        public IActionResult UpdateToDoList([FromRoute]Guid id, [FromBody]ToDoListDTO toDoListDTO)
        {

            ToDoListDTO updatedDTO;

            try
            {
                updatedDTO = _service.UpdateToDoList(id, toDoListDTO, User.GetEmail());
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return Ok(updatedDTO);
        }

    

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize("remove:to-do-list")]
        public IActionResult DeleteToDoList([FromRoute]Guid id)
        {

            try
            {
                _service.DeleteToDoList(id, User.GetEmail());
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("search")]
        [Authorize("read:to-do-list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult SearchToDoList([FromQuery]string title)
        {
            return Ok(_service.SearchToDoLists(title, User.GetEmail()));
        }


        /******************************
         * To do list sharing methods 
         ******************************/

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateSharedList([FromRoute]Guid id)
        {
            Guid sharedListId;
            try
            {
                sharedListId = _service.CreateSharedList(id);
            } catch(EntityNotFoundException)
            {
                return NotFound();
            }
            return Ok(sharedListId);
        }


        [HttpGet("share/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSharedToDoList([FromRoute]Guid id)
        {

            ToDoListDTO sharedListDTO;
            
            try
            
            {
                sharedListDTO = _service.GetSharedList(id);
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
            catch(BadRequestException)
            {
                return BadRequest();
            }

            return Ok(sharedListDTO);

        }







    }
}