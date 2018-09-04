using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using eduTech.Models;

namespace EduTech
{
    public class IndexModel : PageModel
    {
        private readonly eduTech.Models.EduTechContext _context;

        public IndexModel(eduTech.Models.EduTechContext context)
        {
            _context = context;
        }

        public IList<Card> Card { get;set; }

        public async Task OnGetAsync()
        {
            Card = await _context.Card.ToListAsync();
        }
    }
}
