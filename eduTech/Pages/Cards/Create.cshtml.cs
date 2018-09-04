using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using eduTech.Models;

namespace EduTech
{
    public class CreateModel : PageModel
    {
        private readonly eduTech.Models.EduTechContext _context;

        public CreateModel(eduTech.Models.EduTechContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Card Card { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Card.Add(Card);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}