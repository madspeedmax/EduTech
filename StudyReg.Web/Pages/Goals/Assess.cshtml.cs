﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyReg.Web.Areas.Identity.Data;
using StudyReg.Web.Data;
using StudyReg.Web.Models;

namespace StudyReg.Web.Pages.Goals
{
    public class AssessModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<StudyRegWebUser> _userManager;

        public AssessModel(ApplicationDbContext context, UserManager<StudyRegWebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Goal Goal { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            Goal = await _context.Goal
                .Include(g => g.User)
                .Include(g => g.Deck)
                .FirstOrDefaultAsync(
                    m => m.Id == id && 
                    m.User.Id == user.Id && 
                    m.GoalDate.Date < DateTime.UtcNow.Date);

            if (Goal == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var goalToEdit = await _context.Goal.Include(g => g.User).Where(g => g.Id == Goal.Id).FirstOrDefaultAsync();

            if (!ModelState.IsValid || goalToEdit == null || goalToEdit.GoalDate.Date > DateTime.UtcNow.Date || goalToEdit.User?.Id != user.Id)
            {
                return Page();
            }

            goalToEdit.SelfAssessmentExpectation = Goal.SelfAssessmentExpectation;
            goalToEdit.SelfAssessmentAdjustment = Goal.SelfAssessmentAdjustment;
            goalToEdit.Grade = Goal.Grade;
            goalToEdit.SelfAssessmentDate = DateTime.UtcNow;

            _context.Attach(goalToEdit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoalExists(Goal.Id))
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

        private bool GoalExists(int id)
        {
            return _context.Goal.Any(e => e.Id == id);
        }
    }
}
