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

namespace StudyReg.Web.Pages.Decks
{
    public class IndexModel : PageModel
    {
        private readonly StudyReg.Web.Data.ApplicationDbContext _context;
        private readonly UserManager<StudyRegWebUser> _userManager;

        public IndexModel(StudyReg.Web.Data.ApplicationDbContext context, UserManager<StudyRegWebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Deck> Deck { get;set; }

        public async Task OnGetAsync()
        {
            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);
            Deck = await _context.Deck.Where(c => c.User.Id == loggedInUser.Id).ToListAsync();
        }
    }
}
