using JobFind.DAL;
using JobFind.ViewModel.Category;
using JobFind.ViewModel.Job_Announcements;
using JobFind.ViewModel.JobVm;
using JobFind.ViewModel.Nature;
using JobFind.ViewModel.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobFind.Controllers
{
    public class JobController(JobFindContext _context) : Controller
    {
        public async Task<IActionResult> Index(string searchString, string locationFilter, int categoryId = 0, int natureId = 0, int page = 1)
        {
            int itemsPerPage = 6;

            var jobsQuery = _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Nature)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                jobsQuery = jobsQuery.Where(j =>
                    j.Name.Contains(searchString) ||
                    j.Description.Contains(searchString) ||
                    j.City.Contains(searchString) ||
                    j.CompanyName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(locationFilter))
            {
                jobsQuery = jobsQuery.Where(j =>
                    j.City.Equals(locationFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (categoryId != 0)
            {
                jobsQuery = jobsQuery.Where(j => j.CategoryId == categoryId);
            }

            if (natureId != 0)
            {
                jobsQuery = jobsQuery.Where(j => j.Nature.Id == natureId);
            }

            jobsQuery = jobsQuery.OrderByDescending(j => j.PostedDate);

            var totalItems = await jobsQuery.CountAsync();

            var jobs = await jobsQuery
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();

            var categories = await _context.Categories.ToListAsync();
            var natures = await _context.Natures.ToListAsync();

            var viewModel = new JobIndexVM
            {
                Jobs = jobs.Select(j => new GetJobVM
                {
                    Id = j.Id,
                    Name = j.Name,
                    ImageUrl = j.ImageUrl,
                    Description = j.Description,
                    City = j.City,
                    Salary = j.Salary,
                    PostedDate = j.PostedDate,
                    CategoryId = j.Category.Id,
                    CategoryName = j.Category.Name,
                    NatureId = j.Nature.Id,
                    NatureName = j.Nature.Name,
                    CompanyName = j.CompanyName
                }).ToList(),
                Categories = categories.Select(c => new GetCategoryVM
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),
                Natures = natures.Select(n => new GetAdminNatureVM
                {
                    Id = n.Id,
                    Name = n.Name
                }).ToList(),
                PaginationInfo = new PaginationInfoVM
                {
                    CurrentPage = page,
                    ItemsPerPage = itemsPerPage,
                    TotalItems = totalItems,
                    Url = "/Job/Index?page=" + page
                },
                SearchString = searchString,
                LocationFilter = locationFilter,
                SelectedCategoryId = categoryId,
                SelectedNatureId = natureId
            };

            if (categoryId != 0 && totalItems == 0)
            {
                ViewBag.CategoryMessage = "No jobs found in the selected category.";
            }

            return View(viewModel);
        }









    }
}



//    private async Task<List<GetJobVM>> GetJobsVM(List<Job> jobs)
//    {
//        var jobsVM = new List<GetJobVM>();

//        foreach (var job in jobs)
//        {
//            var jobVM = new GetJobVM
//            {
//                Id = job.Id,
//                Name = job.Name,
//                CategoryName = job.Category.Name,
//                ImageUrl=job.ImageUrl,
//                City=job.City,

//                NatureName = job.Nature.Name,
//                Description = job.Description,
//                PostedDate = job.PostedDate,
//                Salary = job.Salary
//            };

//            jobsVM.Add(jobVM);
//        }

//        return jobsVM;
//    }

//    private List<GetCategoryVM> GetCategoriesVM(List<Category> categories)
//    {
//        var categoriesVM = categories.Select(category => new GetCategoryVM
//        {
//            Id = category.Id,
//            Name = category.Name
//        }).ToList();

//        return categoriesVM;
//    }

//    private List<GetAdminNatureVM> GetNaturesVM(List<Nature> natures)
//    {
//        var naturesVM = natures.Select(nature => new GetAdminNatureVM
//        {
//            Id = nature.Id,
//            Name = nature.Name
//        }).ToList();

//        return naturesVM;
//    }

//    public async Task<IActionResult> Index(string category, string jobType, string experience, string postedWithin, string sort, int? page)
//    {
//        var jobs = _context.Jobs.Include(j => j.Category).Include(j => j.Nature).AsQueryable();

//        if (!string.IsNullOrEmpty(category))
//        {
//            jobs = jobs.Where(j => j.Category.Name == category);
//        }

//        if (!string.IsNullOrEmpty(jobType))
//        {
//            jobs = jobs.Where(j => j.Nature.Name == jobType);
//        }

//        switch (sort)
//        {
//            case "name_asc":
//                jobs = jobs.OrderBy(j => j.Name);
//                break;
//            case "name_desc":
//                jobs = jobs.OrderByDescending(j => j.Name);
//                break;
//            case "salary_asc":
//                jobs = jobs.OrderBy(j => j.Salary);
//                break;
//            case "salary_desc":
//                jobs = jobs.OrderByDescending(j => j.Salary);
//                break;
//            default:
//                break;
//        }

//        var count = await jobs.CountAsync();
//        var items = await jobs.Skip((page ?? 0) * pageSize).Take(pageSize).ToListAsync();

//        var jobsVM = await GetJobsVM(items);
//        var categoriesVM = GetCategoriesVM(await _context.Categories.ToListAsync());
//        var naturesVM = GetNaturesVM(await _context.Natures.ToListAsync());

//        var paginationInfo = new PaginationInfoVM
//        {
//            CurrentPage = page ?? 0,
//            ItemsPerPage = pageSize,
//            TotalItems = count,
//            Url = "/Job/Index?sort=" + sort + "&category=" + category + "&jobType=" + jobType + "&experience=" + experience + "&postedWithin=" + postedWithin + "&"
//        };

//        var jobIndexVM = new JobIndexVM
//        {
//            Jobs = jobsVM,
//            Categories = categoriesVM,
//            Natures = naturesVM,
//            PaginationInfo = paginationInfo
//        };

//        return View(jobIndexVM);
//    }

//    protected override void Dispose(bool disposing)
//    {
//        if (disposing)
//        {
//            _context.Dispose();
//        }
//        base.Dispose(disposing);
//    }
//} 