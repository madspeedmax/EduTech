﻿using System;
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
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<StudyRegWebUser> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<StudyRegWebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

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
    }
}
