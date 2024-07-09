using JobFind.DAL;
using JobFind.Models;

using JobFind.ViewModel.CV;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using iText.IO.Image;

using Org.BouncyCastle.Crypto;

namespace JobFind.Controllers
{
    [Authorize]
    public class CVController(JobFindContext _context, IWebHostEnvironment _environment, UserManager<AppUser> _userManager) : Controller
    {
       

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            var data = await _context.CVs
                .Where(c => c.UserId == userId)
                .Select(c => new GetCvVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    Surname = c.Surname,
                    Uni=c.UniversityName,
                    Certificate = c.Certificate,
                    GraduatedAt = c.GraduatedAt,
                    Description = c.Description,
                    Skills1 = c.Skills1,
                    Skills2= c.Skills2,
                    Skills3 = c.Skills3,
                    UrlImage = c.UrlImage,
                    Phone = c.Phone,
                    Email = c.Email,
                    City = c.City,
                    EmployerAddress = c.EmployerAddress,
                    ReferanceNum = c.ReferanceNum,
                    MoreInformation=c.İnformationAboutYourself
                })
                .ToListAsync();

            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCvVm createVM)
        {
            if (createVM.UrlImage == null)
            {
                ModelState.AddModelError("ImageFile", "The Image file is required");
            }
            if (!ModelState.IsValid)
            {
                return View(createVM);
            }

            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(createVM.UrlImage!.FileName);

            string imageFullPath = _environment.WebRootPath + "/cv/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                createVM.UrlImage.CopyTo(stream);
            }

            CV cv = new CV()
            {
                Name = createVM.Name,
                Surname = createVM.Surname,
                UniversityName=createVM.Uni,
                Certificate = createVM.Certificate,
                GraduatedAt = createVM.GraduatedAt,
                Description = createVM.Description,
                Skills1 = createVM.Skills1,
                Skills2= createVM.Skills2,
                Skills3= createVM.Skills3,
                UrlImage = newFileName,
                Phone = createVM.Phone,
                Email = createVM.Email,
                City = createVM.City,
                EmployerAddress = createVM.EmployerAddress,
                ReferanceNum = createVM.ReferanceNum,
                İnformationAboutYourself= createVM.MoreInformation,
                UserId = userId 
            };

            _context.CVs.Add(cv);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var cv = await _context.CVs.FindAsync(id);
            if (cv == null || cv.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            var editVM = new EditCvVM()
            {
                Name = cv.Name,
                Surname = cv.Surname,
                Uni=cv.UniversityName,
                Certificate = cv.Certificate,
                GraduatedAt = cv.GraduatedAt,
                Description = cv.Description,
                Skills1 = cv.Skills1,
                Skills2=cv.Skills2,
                Skills3= cv.Skills3,
                UrlImage = null,
                Phone = cv.Phone,
                Email = cv.Email,
                City = cv.City,
                EmployerAddress = cv.EmployerAddress,
                ReferanceNum = cv.ReferanceNum,
                MoreInformation=cv.İnformationAboutYourself,
            };

            return View(editVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, EditCvVM editVM)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }

            var cv = await _context.CVs.FindAsync(id);
            if (cv == null || cv.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(editVM);
            }

            string newFileName = cv.UrlImage;
            if (editVM.UrlImage != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(editVM.UrlImage.FileName);
                string imgFullPath = _environment.WebRootPath + "/cv/" + newFileName;
                using (var stream = System.IO.File.Create(imgFullPath))
                {
                    editVM.UrlImage.CopyTo(stream);
                }
                string oldImage = _environment.WebRootPath + "/cv/" + newFileName;
                System.IO.File.Delete(oldImage);
            }

            cv.Name = editVM.Name;
            cv.Surname = editVM.Surname;
            cv.UniversityName = editVM.Uni;
            cv.Certificate = editVM.Certificate;
            cv.GraduatedAt = editVM.GraduatedAt;
            cv.Description = editVM.Description;
            cv.Skills1 = editVM.Skills1;
            cv.Skills2 = editVM.Skills2;
            cv.Skills3 = editVM.Skills3;
            cv.UrlImage = newFileName;
            cv.Phone = editVM.Phone;
            cv.Email = editVM.Email;
            cv.City = editVM.City;
            cv.EmployerAddress = editVM.EmployerAddress;
            cv.ReferanceNum = editVM.ReferanceNum;
            cv.İnformationAboutYourself = editVM.MoreInformation;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }

            var cv = await _context.CVs.FindAsync(id);
            if (cv == null || cv.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            string iconFullPath = _environment.WebRootPath + "/cv/" + cv.UrlImage;
            System.IO.File.Delete(iconFullPath);

            _context.CVs.Remove(cv);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> GeneratePdf(int? id)
        {
            if (id <= 0 || id is null)
            {
                return BadRequest();
            }

            var CV = await _context.CVs.FindAsync(id);
            if (CV == null)
            {
                return NotFound();
            }

            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                if (!string.IsNullOrEmpty(CV.UrlImage))
                {
                    Image img = new Image(ImageDataFactory.Create(_environment.WebRootPath + "/cv/" + CV.UrlImage));
                    document.Add(img);
                }
                document.Add(new Paragraph("Name: " + CV.Name));
                document.Add(new Paragraph("Surname: " + CV.Surname));
                document.Add(new Paragraph("Universty: " + CV.UniversityName));
                document.Add(new Paragraph("Certificate: " + CV.Certificate));
                document.Add(new Paragraph("Graduated At: " + CV.GraduatedAt));
                document.Add(new Paragraph("Languages: " + CV.Description));
                document.Add(new Paragraph("Skills: " + CV.Skills1+","+CV.Skills2+","+CV.Skills3));
                document.Add(new Paragraph("Phone: " + CV.Phone));
                document.Add(new Paragraph("Email: " + CV.Email));
                document.Add(new Paragraph("City: " + CV.City));
                document.Add(new Paragraph("Employer Address: " + CV.EmployerAddress));
                document.Add(new Paragraph("Reference Number: " + CV.ReferanceNum));
                document.Add(new Paragraph("More Infarmation: " + CV.İnformationAboutYourself));

                document.Close();
                return File(stream.ToArray(), "application/pdf", "CV.pdf");
            }
        }
    }
}