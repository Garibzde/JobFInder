using JobFind.Interfaces;
using JobFind.Service;
using JobFind.ViewModel.Contact;
using Microsoft.AspNetCore.Mvc;

namespace JobFind.Controllers
{
    public class ContactController(IEmailService _emailService) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new ContactUsVM());
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Index(ContactUsVM model)
        {
            if (ModelState.IsValid)
            {
                string subject = model.Subject;
                string body = $"Name: {model.Name}\nEmail: {model.Email}\nMessage: {model.Message}";
                await _emailService.SendMailAsync("your-email@example.com", subject, body);

                
                string userSubject = "Thank you for contacting us";
                string userBody = $"Dear {model.Name},\n\nThank you for getting in touch with us. We have received your message and will get back to you shortly.\n\nBest regards,\nJobFind Team";
                await _emailService.SendMailAsync(model.Email, userSubject, userBody);

                ViewBag.Message = "Your message has been sent successfully!";
                return View();
            }

            return View(model);
        }
    }
}
