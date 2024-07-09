using JobFind.DAL;
using JobFind.Models;
using JobFind.ViewModel.Job_Announcements;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobFind.Controllers
{
    public class JobDetailController : Controller
    {
        private readonly JobFindContext _context;
        private readonly UserManager<AppUser> _userManager;

        public JobDetailController(JobFindContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var job = _context.Jobs
                              .Include(j => j.Category)
                              .Include(j => j.Nature)
                              .FirstOrDefault(j => j.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            var model = new GetJobVM
            {
                Id = job.Id,
                Name = job.Name,
                ImageUrl = job.ImageUrl,
                Description = job.Description,
                City = job.City,
                Salary = job.Salary,
                PostedDate = job.PostedDate,
                CategoryId = job.CategoryId,
                CategoryName = job.Category.Name,
                NatureId = job.NatureId,
                NatureName = job.Nature.Name,
                CompanyName = job.CompanyName
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyToJob(int jobId, IFormFile cvFile)
        {
            if (cvFile == null || cvFile.Length == 0)
            {
                ModelState.AddModelError("cvFile", "CV dosyası gereklidir.");
                return View();
            }

            var userId = _userManager.GetUserId(User);
            var job = await _context.Jobs.FindAsync(jobId);

            if (job == null)
            {
                return NotFound();
            }

            using (var memoryStream = new MemoryStream())
            {
                await cvFile.CopyToAsync(memoryStream);
                var cvData = memoryStream.ToArray();

                var jobApplication = new JobApplication
                {
                    JobId = jobId,
                    UserId = userId,
                    CVFileName = Path.GetFileName(cvFile.FileName),
                    CVContentType = cvFile.ContentType,
                    CVData = cvData,
                    AppliedDate = DateTime.UtcNow
                };

                _context.JobApplications.Add(jobApplication);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = jobId });
            }
        }
    }
}
