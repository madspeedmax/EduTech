using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyReg.Web.Areas.Identity.Data;
using StudyReg.Web.Data;
using StudyReg.Web.Models;

namespace StudyReg.Web.Pages.Cards
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<StudyRegWebUser> _userManager;

        public CreateModel(StudyReg.Web.Data.ApplicationDbContext context, UserManager<StudyRegWebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Card Card { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Card.User = await _userManager.GetUserAsync(HttpContext.User);

            _context.Card.Add(Card);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}