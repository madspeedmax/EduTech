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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<StudyRegWebUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<StudyRegWebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Goal> Goal { get;set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            Goal = await _context.Goal
                .Include(g => g.Deck)
                .Where(g => g.User.Id == user.Id)
                .OrderByDescending(g => g.GoalDate)
                .ToListAsync();
        }
    }
}
