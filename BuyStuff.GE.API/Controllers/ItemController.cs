using BuyStuff.GE.Application.Items;
using BuyStuff.GE.Application.Items.Requests;
using BuyStuff.GE.Application.Items.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BuyStuff.GE.API.Controllers
{
    /// <summary>
    /// Controller for item management
    /// </summary>
    [Route("v1/[controller]")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly IItemService itemService;
        private readonly IHttpContextAccessor _accessor;

        public ItemController(IItemService itemService, IHttpContextAccessor accessor)
        {
            this.itemService = itemService;
            _accessor = accessor;
        }

        /// <summary>
        /// Get all items
        /// </summary>
        /// <response code="200">Returns a list of all items</response>
        [HttpGet]
        [Route("GetAllItems")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ItemResponseModel>))]
        public async Task<ActionResult<List<ItemResponseModel>>> GetAllItems(CancellationToken cancellationToken)
        {
            return Ok(await itemService.GetAllItems(cancellationToken));
        }

        /// <summary>
        /// Get an item by its ID
        /// </summary>
        /// <param name="id">The ID of the item</param>
        /// <response code="200">Returns the item with the specified ID</response>
        /// <response code="404">If the item with the specified ID is not found</response>
        [HttpGet]
        [Route("GetItemById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ItemResponseModel>> GetItemById(int id, CancellationToken cancellationToken)
        {
            var item = await itemService.GetItemById(id, cancellationToken);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        /// <summary>
        /// Add a new item.
        /// </summary>
        /// <param name="request">The request model for creating the item</param>
        /// <response code="200">Returns the ID of the created item</response>
        [HttpPost("AddItem")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<ActionResult<int>> AddItem([FromForm] ItemRequestModel request, CancellationToken cancellationToken)
        {
            return Ok(await itemService.CreateItem(request, GetUserId(), cancellationToken));
        }

        /// <summary>
        /// Delete an item by its ID
        /// </summary>
        /// <param name="id">The ID of the item to delete</param>
        /// <response code="200">If the item is successfully deleted</response>
        /// <response code="404">If the item with the specified ID is not found</response>
        [HttpDelete("DeleteItem/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteItem(int id, CancellationToken cancellationToken)
        {
            await itemService.DeleteItem(id, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="request">The request model for updating the item</param>
        /// <response code="200">If the item is successfully updated</response>
        /// <response code="404">If the item with the specified ID is not found</response>
        [HttpPut("UpdateItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateItem([FromForm] ItemRequestPutModel request, CancellationToken cancellationToken)
        {
            await itemService.UpdateItem(request, cancellationToken);
            return Ok();
        }

        private string GetUserId()
        {
            var x = _accessor.HttpContext.User.Identity as ClaimsIdentity;
            return x.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
