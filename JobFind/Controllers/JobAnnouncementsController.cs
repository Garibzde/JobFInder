using JobFind.DAL;
using JobFind.Enums;
using JobFind.Models;
using JobFind.ViewModel.Job_Announcements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobFind.Controllers
{
    [Authorize(Roles= "CompanyUser")]
    public class JobAnnouncementsController(JobFindContext _context, IWebHostEnvironment _environment, UserManager<AppUser> _userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            var data = await _context.Jobs
                .Where(j => j.UserId == userId)
                .Select(j => new GetJobVM
                {
                    Id = j.Id,
                    Name = j.Name,
                    ImageUrl = j.ImageUrl,
                    Description = j.Description,
                    City = j.City,
                    Salary = j.Salary,
                    PostedDate = j.PostedDate,
                    CategoryId = j.CategoryId,
                    CategoryName = j.Category.Name,
                    NatureId = j.NatureId,
                    NatureName = j.Nature.Name,
                    CompanyName = j.CompanyName
                })
                .ToListAsync();

            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            ViewBag.Natures = new SelectList(_context.Natures.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobVM createVM)
        {
            if (createVM.Image == null)
            {
                ModelState.AddModelError("Image", "The Image file is required");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
                ViewBag.Natures = new SelectList(_context.Natures.ToList(), "Id", "Name");
                return View(createVM);
            }

            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(createVM.Image.FileName);

            string imageFullPath = Path.Combine(_environment.WebRootPath, "jobImages", newFileName);
            using (var stream = new FileStream(imageFullPath, FileMode.Create))
            {
                await createVM.Image.CopyToAsync(stream);
            }

            var job = new Job
            {
                Name = createVM.Name,
                ImageUrl = newFileName,
                Description = createVM.Description,
                City = createVM.City,
                Salary = createVM.Salary,
                PostedDate = createVM.PostedDate,
                CategoryId = createVM.CategoryId,
                NatureId = createVM.NatureId,
                CompanyName = createVM.CompanyName,
                UserId = userId
            };
            var category = await _context.Categories.FindAsync(createVM.CategoryId);
            if (category != null)
            {
                category.JobCount++;
            }

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var job = await _context.Jobs.FindAsync(id);
            if (job == null || job.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            ViewBag.Natures = new SelectList(_context.Natures.ToList(), "Id", "Name");

            var editVM = new EditJobVM
            {
               
                Name = job.Name,
                Description = job.Description,
                City = job.City,
                Salary = job.Salary,
                PostedDate = job.PostedDate,
                CategoryId = job.CategoryId,
                NatureId = job.NatureId,
                CompanyName = job.CompanyName
            };

            return View(editVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, EditJobVM editVM)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }

            var job = await _context.Jobs.FindAsync(id);
            if (job == null || job.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
                ViewBag.Natures = new SelectList(_context.Natures.ToList(), "Id", "Name");
                return View(editVM);
            }

            string newFileName = job.ImageUrl;
            if (editVM.Image != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(editVM.Image.FileName);
                string imgFullPath = Path.Combine(_environment.WebRootPath, "jobImages", newFileName);
                using (var stream = new FileStream(imgFullPath, FileMode.Create))
                {
                    await editVM.Image.CopyToAsync(stream);
                }
                string oldImage = Path.Combine(_environment.WebRootPath, "jobImages", job.ImageUrl);
                if (System.IO.File.Exists(oldImage))
                {
                    System.IO.File.Delete(oldImage);
                }
            }

            job.Name = editVM.Name;
            job.Description = editVM.Description;
            job.City = editVM.City;
            job.Salary = editVM.Salary;
            job.PostedDate = editVM.PostedDate;
            job.CategoryId = editVM.CategoryId;
            job.NatureId = editVM.NatureId;
            job.CompanyName = editVM.CompanyName;
            job.ImageUrl = newFileName;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }

            var job = await _context.Jobs.FindAsync(id);
            if (job == null || job.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            string imageFullPath = Path.Combine(_environment.WebRootPath, "jobImages", job.ImageUrl);
            if (System.IO.File.Exists(imageFullPath))
            {
                System.IO.File.Delete(imageFullPath);
            }
            var category = await _context.Categories.FindAsync(job.CategoryId);
            if (category != null)
            {
                category.JobCount++;
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
       
        
        public async Task<IActionResult> ViewApplications(int jobId)
        {
            var jobApplications = await _context.JobApplications
                .Include(j => j.User)
                .Where(j => j.JobId == jobId)
                .ToListAsync();

            return View(jobApplications);
        }
        public async Task<IActionResult> DownloadCV(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);

            if (jobApplication == null)
            {
                return NotFound();
            }

            return File(jobApplication.CVData, jobApplication.CVContentType, jobApplication.CVFileName);
        }

    }



}

