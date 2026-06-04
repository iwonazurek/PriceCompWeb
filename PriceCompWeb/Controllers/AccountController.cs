using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PriceCompWeb.Models;

namespace PriceCompWeb.Controllers;

public class AccountController : Controller
{
    private readonly IConfiguration _configuration;

    public AccountController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new AdminLoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(AdminLoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var email = _configuration["AdminAccount:Email"];
        var password = _configuration["AdminAccount:Password"];

        if (!string.Equals(model.Email, email, StringComparison.OrdinalIgnoreCase) || model.Password != password)
        {
            ModelState.AddModelError(string.Empty, "Niepoprawny e-mail lub haslo.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, model.Email),
            new(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction("Index", "Admin");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
