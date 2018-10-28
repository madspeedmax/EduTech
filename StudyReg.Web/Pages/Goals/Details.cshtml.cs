using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudyReg.Web.Data;
using StudyReg.Web.Models;

namespace StudyReg.Web.Pages.Goals
{
    public class DetailsModel : PageModel
    {
        private readonly StudyReg.Web.Data.ApplicationDbContext _context;

        public DetailsModel(StudyReg.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Goal Goal { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Goal = await _context.Goal.FirstOrDefaultAsync(m => m.Id == id);

            if (Goal == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
