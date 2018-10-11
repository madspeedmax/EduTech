using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyReg.Web.Areas.Identity.Data;
using StudyReg.Web.Data;
using StudyReg.Web.Models;

namespace StudyReg.Web.Pages.Decks
{
    public class CreateModel : PageModel
    {
        private readonly StudyReg.Web.Data.ApplicationDbContext _context;
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
        public Deck Deck { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Deck.Created = DateTime.UtcNow;
            Deck.User = await _userManager.GetUserAsync(HttpContext.User);

            _context.Deck.Add(Deck);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}