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

namespace StudyReg.Web.Pages.Cards
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

        public IList<Card> Card { get;set; }

        public async Task OnGetAsync(string searchString)
        {
            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);

            var cards = from c in _context.Card
                         select c;

            cards.Where(c => c.User.Id == loggedInUser.Id);

            if (!String.IsNullOrEmpty(searchString))
            {
                cards = cards.Where(c => c.Title.Contains(searchString) || c.Answer.Contains(searchString));
            }

            Card = await cards.ToListAsync();
        }
    }
}
