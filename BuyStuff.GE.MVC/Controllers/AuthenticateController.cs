using BuyStuff.GE.Domain.Users.Requests;
using BuyStuff.GE.MVC.ApiServices;
using Microsoft.AspNetCore.Mvc;

namespace BuyStuff.GE.MVC.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly AuthenticationApiService _authApiService;

        public AuthenticateController(AuthenticationApiService authApiService)
        {
            _authApiService = authApiService;
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, CancellationToken cancellationToken)
        {
            var jwt =await _authApiService.Login(model, cancellationToken);
            Response.Cookies.Append("jwt", jwt);
            Response.Cookies.Append("username", model.Username);
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model, CancellationToken cancellationToken)
        {
            var jwt = await _authApiService.Register(model, cancellationToken);
            Response.Cookies.Append("jwt", jwt);
            Response.Cookies.Append("username", model.Username);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("username");
            return RedirectToAction("Index", "Home");
        }

    }
}
