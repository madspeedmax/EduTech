using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudyReg.Web.Areas.Identity.Data;
using StudyReg.Web.Data;
using StudyReg.Web.Models;

namespace StudyReg.Web.Pages.Goals
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<StudyRegWebUser> _userManager;

        public DeleteModel(ApplicationDbContext context, UserManager<StudyRegWebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Goal Goal { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            Goal = await _context.Goal.FirstOrDefaultAsync(m => m.Id == id && m.User.Id == user.Id);

            if (Goal == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            Goal = await _context.Goal.FindAsync(id);

            if (Goal != null && Goal.User.Id == user.Id)
            {
                _context.Goal.Remove(Goal);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
