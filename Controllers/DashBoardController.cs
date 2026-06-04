using Application_Auth.Data;
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
        return View();
    }
    

}