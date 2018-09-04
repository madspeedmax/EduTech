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
    public class DeleteModel : PageModel
    {
        private readonly eduTech.Models.EduTechContext _context;

        public DeleteModel(eduTech.Models.EduTechContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Card Card { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Card = await _context.Card.FirstOrDefaultAsync(m => m.Id == id);

            if (Card == null)
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

            Card = await _context.Card.FindAsync(id);

            if (Card != null)
            {
                _context.Card.Remove(Card);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
