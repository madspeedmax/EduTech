﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eduTech.Models;

namespace EduTech
{
    public class EditModel : PageModel
    {
        private readonly eduTech.Models.EduTechContext _context;

        public EditModel(eduTech.Models.EduTechContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Card).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(Card.Id))
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

        private bool CardExists(int id)
        {
            return _context.Card.Any(e => e.Id == id);
        }
    }
}
