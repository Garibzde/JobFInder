using JobFind.DAL;
using JobFind.Service;
using JobFind.ViewModel.Category;
using JobFind.ViewModel.Home;
using JobFind.ViewModel.Job_Announcements;
using JobFind.ViewModel.Slider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JobFind.Controllers
{
    public class HomeController(JobFindContext _context) : Controller
    {
       

        public async Task<IActionResult> Index(string searchTerm)
        {
            var category = await _context.Categories.Select(c => new GetCategoryVM
            {
                Icon = c.Icon,
                Name = c.Name,
                JobCount = c.JobCount,
            }).ToListAsync();

            var slider = await _context.Sliders.Select(s => new GetSliderVM
            {
                Name = s.Name,
                ImageFile = s.ImgUrl,
                Title = s.Title,
                Job = s.Job,
            }).ToListAsync();

            var jobsQuery = _context.Jobs.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                jobsQuery = jobsQuery.Where(j => j.Name.Contains(searchTerm) || j.Description.Contains(searchTerm));
            }

            var job = await jobsQuery.Select(j => new GetUserJobVM
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
            }).ToListAsync();

            var viewModel = new HomePageViewModel
            {
                getCategoryVMs = category,
                getSliderVMs = slider,
                getUserJobVMs = job,
            };
            return View(viewModel);
        }
    }
}
