using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudyReg.Web.Data;
using StudyReg.Web.Models;

namespace StudyReg.Web.Pages.Decks
{
    public class IndexModel : PageModel
    {
        private readonly StudyReg.Web.Data.ApplicationDbContext _context;

        public IndexModel(StudyReg.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Deck> Deck { get;set; }

        public async Task OnGetAsync()
        {
            Deck = await _context.Deck.ToListAsync();
        }
    }
}
