using JobFind.DAL;
using JobFind.Models;
using JobFind.ViewModel.Category;
using JobFind.ViewModel.Slider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobFind.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SliderController(JobFindContext _context, IWebHostEnvironment environment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.Select(s => new GetAdminSliderVM
            {
                Id = s.Id,
                Name = s.Name,
                Job=s.Job,
                ImageFile=s.ImgUrl,



            }).ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(CreateSliderVM CreateVM)
        {
            if (CreateVM.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The Image file is required");
            }
            if (!ModelState.IsValid)
            {
                return View(CreateVM);
            }

            string NewFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            NewFileName += Path.GetExtension(CreateVM.ImageFile!.FileName);

            string ImageFullPath = environment.WebRootPath + "/slider/" + NewFileName;
            using (var stream = System.IO.File.Create(ImageFullPath))
            {
                CreateVM.ImageFile.CopyTo(stream);
            }
             Slider slider = new Slider()
            {
                ImgUrl = NewFileName,
                Name = CreateVM.Name,
                Job = CreateVM.Job,
                Title = CreateVM.Title,
                


            };
            _context.Sliders.Add(slider);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id <= 0)
            {
                return NotFound();
            }
            var slider = await _context.Sliders.FindAsync(Id);
            if (slider == null)
            {
                return BadRequest();
            }
            var EditVM = new EditSliderVM()
            {
                Name = slider.Name,
                Job = slider.Job,
                Title = slider.Title,
                ImageFile=null
                

            };


            return View(EditVM);
        }
        [HttpPost]

        public async Task<IActionResult> Edit(int? id, EditSliderVM editSlider)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(editSlider);
            }
            string newFileName = slider.ImgUrl;
            if (editSlider.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(editSlider.ImageFile.FileName);
                string ImgFullPath = environment.WebRootPath + "/slider/" + newFileName;
                using (var stream = System.IO.File.Create(ImgFullPath))
                {
                    editSlider.ImageFile.CopyTo(stream);
                }
                string oldImage = environment.WebRootPath + "/slider/" + newFileName;
                System.IO.File.Delete(oldImage);

            }
            slider.Name = editSlider.Name;
            slider.ImgUrl = newFileName;
            slider.Title = editSlider.Title;
           slider.Job = editSlider.Job;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            { return BadRequest(); }
            var slider = _context.Sliders.Find(id);
            if (slider == null)
            { return NotFound(); }
            string IconFullPath = environment.WebRootPath + "/slider/" + slider.ImgUrl;
            System.IO.File.Delete(IconFullPath);

            _context.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
