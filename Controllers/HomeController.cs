using BZLAND.DAL;
using BZLAND.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BZLAND.Controllers
{
    public class HomeController : Controller
    {
    private readonly AddDbContext _context;
    public HomeController(AddDbContext context)
    {
          _context= context;
    }

    public async Task<IActionResult> Index()
    {
            List<Team> Teams = await _context.Teams.ToListAsync();
            return View(Teams);
    }


    }
}