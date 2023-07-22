using BuyStuff.GE.Application.Items.Requests;
using BuyStuff.GE.MVC.ApiServices;
using BuyStuff.GE.MVC.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BuyStuff.GE.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ItemApiService _itemApiService;
        private readonly IHttpContextAccessor _accessor;


        public HomeController(ItemApiService itemApiService, IHttpContextAccessor accessor)
        {
            _itemApiService = itemApiService;
            _accessor = accessor;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string SearchString, CancellationToken cancellationToken)
        {
            var items = await _itemApiService.GetAllItems(cancellationToken);
            ViewData["filter"] = SearchString;
            items = !string.IsNullOrEmpty(SearchString)
                ? items.Where(i => i.Title.ToLower().Contains(SearchString))
                : items;
            return View(items.OrderByDescending(x => x.Id));
        }

        [HttpGet]
        public async Task<IActionResult> ViewDetails(int itemId, CancellationToken token = default(CancellationToken))
        {
            var item = await _itemApiService.GetItemById(itemId, token);
            return View(item);
        }

        public ActionResult CreateItem()
        {
            return Request.Cookies["jwt"] != null ? View() : Unauthorized();
        }

        [HttpPost]
        public async Task<ActionResult> CreateItem(ItemRequestModel item, CancellationToken cancellationToken)
        {
            await _itemApiService.AddItem(item, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditItem(int itemId, CancellationToken token)
        {
            var item = await _itemApiService.GetItemById(itemId, token);
            var editItem = new ItemEditModel();
            item.Adapt(editItem);
            editItem.CurrentImages = item.Images;
            return View(editItem);
        }

        [HttpPost]
        public async Task<ActionResult> EditItem(ItemEditModel item, CancellationToken cancellationToken)
        {
            await _itemApiService.UpdateItem(item, cancellationToken);
            return RedirectToAction(nameof(ViewDetails), new { itemId = item.Id });
        }

        public async Task<IActionResult> DeleteItem( int itemId, CancellationToken token)
        {
            await _itemApiService.DeleteItem(itemId, token);
            return RedirectToAction(nameof(Index));
        }

        private string GetUserId()
        {
            var x = _accessor.HttpContext.User.Identity as ClaimsIdentity;
            return x.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
