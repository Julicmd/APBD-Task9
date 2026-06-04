using System.Security.Claims;
using Application_Auth.Data;
using Application_Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application_Auth.Controllers;

public class AccountController : Controller
{
    public readonly AppDbContext _dbContext;
    
    public AccountController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user =  _dbContext.AppUsers.Any(ap => ap.Email == model.Email);
        if (user)
        {
            ModelState.AddModelError("Email", "Already has an account");
            return View(model);
        }

        var hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var newUser = new AppUser
        {
            Email = model.Email,
            PasswordHash = hashPassword,
            Role =  "User",
            CreatedAt = DateTime.UtcNow
        };
        
        await _dbContext.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();
        
        return RedirectToAction("Login");

    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var user = _dbContext.AppUsers.FirstOrDefault(ap => ap.Email == model.Email);
        if (user == null)
        {
            ModelState.AddModelError(" ","Invalid credentials");
            return View(model);
        }

        var inputpass = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);
        if (!inputpass)
        {
            ModelState.AddModelError("", "Invalid credentials");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var identify = new ClaimsIdentity(claims, "CookieAuth");
        var principal = new ClaimsPrincipal(identify);
        
        await HttpContext.SignInAsync("CookieAuth", principal);


        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Login");
    }
    
}