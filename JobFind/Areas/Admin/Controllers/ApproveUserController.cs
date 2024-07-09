using JobFind.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JobFind.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class ApproveUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ApproveUserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is required.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound($"bu Idli isdifadeci: '{userId}' Tapilmadi.");
            }

            user.IsApproved = true;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Isdifadeci yarandi.";
                return RedirectToAction(nameof(Index)); 
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                TempData["ErrorMessage"] = "Isdifadeci yaranan zaman problem yasandi.";
                return RedirectToAction(nameof(Index)); 
            }
        }

    }
    }
