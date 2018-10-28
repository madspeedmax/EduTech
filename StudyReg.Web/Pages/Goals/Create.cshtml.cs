using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyReg.Web.Areas.Identity.Data;
using StudyReg.Web.Data;
using StudyReg.Web.Models;

namespace StudyReg.Web.Pages.Goals
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
            PopulateDeckList();
            return Page();
        }

        [BindProperty]
        public Goal Goal { get; set; }

        public SelectList DeckList { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Goal.Created = DateTime.UtcNow;
            Goal.User = await _userManager.GetUserAsync(HttpContext.User);
            Goal.Deck = await _context.Deck.FirstOrDefaultAsync(g => g.Id == Goal.Deck.Id);

            _context.Goal.Add(Goal);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public void PopulateDeckList()
        {
            var loggedInUser = _userManager.GetUserId(HttpContext.User);

            var decks = _context.Deck.Where(c => c.User.Id == loggedInUser).ToList();

            DeckList = new SelectList(decks, "Id", "Title");
        }
    }
}