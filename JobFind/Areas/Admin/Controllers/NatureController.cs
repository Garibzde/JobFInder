using JobFind.DAL;
using JobFind.Models;
using JobFind.ViewModel.Nature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobFind.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NatureController(JobFindContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Natures.Select(n=> new GetAdminNatureVM
            {
                Id= n.Id,
                Name= n.Name,
            }).ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(CreateNatureVM crtVM)
        {
            if (!ModelState.IsValid) 
            {
                return View(crtVM);
            }
            Nature nature = new Nature()
            {
                Name = crtVM.Name,

            };
            _context.Natures.Add(nature);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));


        }
            public async Task<IActionResult> Edit(int? id)
            {
            if (id < 1&&id==null) 
            {
                return BadRequest();
            }
            Nature nature= await _context.Natures.FindAsync(id);
            if (nature == null)
            {
                return NotFound();
            }
            EditNatureVM editvm = new EditNatureVM
            {
                Name = nature.Name,
            };
                return View(editvm);
            }

        public async Task<IActionResult> Edit(int id,EditNatureVM editvm)
        {
            if (id==null && id<1)
            {
                return BadRequest();
            }
            Nature nature = await _context.Natures.FindAsync(id);
            if (nature == null)
            {
                return NotFound();
            }
            nature.Name = editvm.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null && id < 1)
                return BadRequest();
            var data = await _context.Natures.FindAsync(id);
            if (data is null)
                return BadRequest();
            _context.Natures.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

    }

    
}
