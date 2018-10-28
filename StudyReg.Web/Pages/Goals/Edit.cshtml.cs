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
    public class EditModel : PageModel
    {
        private readonly StudyReg.Web.Data.ApplicationDbContext _context;
        private readonly UserManager<StudyRegWebUser> _userManager;

        public EditModel(StudyReg.Web.Data.ApplicationDbContext context, UserManager<StudyRegWebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Goal Goal { get; set; }

        public SelectList DeckList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            Goal = await _context.Goal
                .Include(g => g.Deck)
                .FirstOrDefaultAsync(m => m.Id == id && m.User.Id == user.Id);

            if (Goal == null)
            {
                return NotFound();
            }

            PopulateDeckList(Goal.Deck?.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Goal.Deck = await _context.Deck.FirstOrDefaultAsync(g => g.Id == Goal.Deck.Id);
            _context.Attach(Goal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoalExists(Goal.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        public void PopulateDeckList(int? selectedDeckId)
        {
            var loggedInUser = _userManager.GetUserId(HttpContext.User);

            var decks = _context.Deck.Where(c => c.User.Id == loggedInUser).ToList();

            DeckList = new SelectList(decks, "Id", "Title", selectedDeckId);
        }

        private bool GoalExists(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            return _context.Goal.Any(e => e.Id == id && e.User.Id == userId);
        }
    }
}
