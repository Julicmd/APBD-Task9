using System.Security.Claims;
using Application_Auth.Data;
using Application_Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application_Auth.Controllers;

[Authorize]
public class DashBoardController :Controller
{
    private readonly AppDbContext _dbContext;
    public DashBoardController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var notes = _dbContext.UserNotes.Where(un =>un.AppUserId == userId).ToList();
        
        return View(notes);
    }
    
    [HttpGet]
    public IActionResult AddNotes()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddNotes(UserNote model)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


        var note = new UserNote
        {
            AppUserId = userId,
            Title = model.Title,
            Content = model.Content,
            CreatedAt = DateTime.UtcNow
        };
        _dbContext.UserNotes.Add(note);
        _dbContext.SaveChanges();
        
        
       return RedirectToAction("Index"); 
    }
    
    

}