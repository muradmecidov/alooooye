using BZLAND.Areas.Admin.ViewModel;
using BZLAND.DAL;
using BZLAND.Models;
using BZLAND.Utlities.Constants;
using BZLAND.Utlities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BZLAND.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamsController : Controller
    {
        private readonly AddDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TeamsController(AddDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams.Where(p => !p.IsDeleted).OrderByDescending(p => p.Id).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamVM teamVM)
        {
            if (!ModelState.IsValid) { return View(teamVM); }
            if (!teamVM.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileMustBeTypeImage);
                return View(teamVM);
            }
            if (teamVM.Photo.Length / 1024 > 200)
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileSizeMustlessThan200Kb);
                return View(teamVM);
            }
            string rootpath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team");
            string filename = Guid.NewGuid().ToString() + teamVM.Photo.FileName;
            using (FileStream fileStream = new FileStream(Path.Combine(rootpath, filename), FileMode.Create))
            {
                await teamVM.Photo.CopyToAsync(fileStream);
            }
            Team team = new Team()
            {
                Name = teamVM.Name,
                Description = teamVM.Description,
                ImagePath = filename
            };
            await _context.Teams.AddRangeAsync(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            Team team = await _context.Teams.FindAsync(id);
            if (team == null) { return NotFound(); }
            UpdateTeamVM teamVM = new UpdateTeamVM()
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description
            };


            return View(teamVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateTeamVM teamVM)
        {
            if (!ModelState.IsValid)
            {
                return View(teamVM);
            }

            Team existingTeam = await _context.Teams.FindAsync(id);
            if (existingTeam == null)
            {
                return NotFound();
            }

            if (teamVM.Photo != null && teamVM.Photo.ContentType.Contains("image/") && teamVM.Photo.Length / 1024 <= 200)
            {
                string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team");

                string existingImagePath = Path.Combine(rootPath, existingTeam.ImagePath);
                if (System.IO.File.Exists(existingImagePath))
                {
                    System.IO.File.Delete(existingImagePath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(teamVM.Photo.FileName);
                string filePath = Path.Combine(rootPath, fileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await teamVM.Photo.CopyToAsync(fileStream);
                }

                existingTeam.Name = teamVM.Name;
                existingTeam.Description = teamVM.Description;
                existingTeam.ImagePath = fileName;

                await _context.SaveChangesAsync();
            }
            else
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileMustBeTypeImage);
                return View(teamVM);
            }

            return RedirectToAction(nameof(Index));








        }

        public async Task<IActionResult> Delete(int id)
        {
            Team team = await _context.Teams.FindAsync(id);
            if (team == null) { return NotFound(); }
            string rootpath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team", team.ImagePath);
            if (System.IO.File.Exists(rootpath))
            {
                System.IO.File.Delete(rootpath);
            }
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}

