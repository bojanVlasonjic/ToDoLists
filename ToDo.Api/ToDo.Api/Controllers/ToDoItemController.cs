using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using ToDo.Api.DTOs;
using ToDo.Api.Services;
using ToDo.Api.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace ToDo.Api.Controllers
{

    [Route("api/to-do-list/{listId}/to-do-item")]
    [ApiController]
    [EnableCors("ToDoPolicy")]
    public class ToDoItemController : ControllerBase
    {

        private readonly IToDoItemService _service;


        public ToDoItemController(IToDoItemService service)
        {
            _service = service;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize("read:to-do-item")]
        public IActionResult GetItemsForToDoList([FromRoute]Guid listId)
        {
            return Ok(_service.GetItemsForToDoList(listId));
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize("write:to-do-item")]
        public IActionResult CreateItemInList([FromRoute]Guid listId, [FromBody]ToDoItemDTO itemDTO)
        {

            ToDoItemDTO newItemDTO;

           try
            {
                newItemDTO = _service.CreateItemInList(listId, itemDTO);
            } 
            catch(EntityNotFoundException) 
            {
                return NotFound();
            }

    
            return CreatedAtAction(nameof(GetItemsForToDoList), new { id = listId }, newItemDTO);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize("write:to-do-item")]
        public IActionResult UpdateItemInList([FromRoute]Guid listId, [FromBody]ToDoItemDTO itemDTO)
        {

            ToDoItemDTO updatedItemDTO;

            try
            {
                updatedItemDTO = _service.UpdateItemInList(listId, itemDTO);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return Ok(updatedItemDTO);
        }


        [HttpPut("{itemId}/{position}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("write:to-do-item")]
        public IActionResult UpdatePosition([FromRoute]Guid listId, [FromRoute]Guid itemId, [FromRoute]int position)
        {
            ToDoItemDTO updatedItemDTO;

            try
            {
                updatedItemDTO = _service.UpdatePosition(listId, itemId, position);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch(BadRequestException)
            {
                return BadRequest();
            }


            return Ok(updatedItemDTO);
        }


        [HttpDelete("{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize("remove:to-do-item")]
        public IActionResult DeleteItemFromList([FromRoute]Guid listId, [FromRoute]Guid itemId)
        {

            try
            {
               _service.DeleteItemFromList(listId, itemId);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }




    }
}