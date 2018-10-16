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
    public class StudyModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<StudyRegWebUser> _userManager;

        private const int _lowestStage = 1;
        private const int _highestStage = 4;

        public StudyModel(StudyReg.Web.Data.ApplicationDbContext context, UserManager<StudyRegWebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Deck Deck { get; set; }

        public Card Card { get; set; }

        public List<int> Stages { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Deck = await _context.Deck
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Deck == null)
            {
                return NotFound();
            }

            var recentLogs = await _context.Card
                .Where(c => c.Decks.Any(d => d.DeckId == id))
                .Select(c => c.Logs.OrderByDescending(l => l.Timestamp).FirstOrDefault())
                .ToListAsync();

            Stages = recentLogs.Select(l => l?.Stage ?? 0).Distinct().OrderBy(i => i).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostStart(int deckId, int stage)
        {
            await SetNextCard(deckId, stage);
            return Page();
        }

        public async Task<IActionResult> OnPostAnswer(int deckId, int cardId, bool result)
        {
            Card = await _context.Card
                .Include(c => c.Logs)
                .FirstOrDefaultAsync(c => c.Id == cardId);

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (Card == null || user == null)
            {
                return NotFound();
            }

            var newLog = new StudyLog()
            {
                Result = result,
                Timestamp = DateTime.UtcNow,
                User = user,
                Card = Card
            };

            var mostRecentLog = Card.Logs
                .OrderByDescending(l => l.Timestamp)
                .FirstOrDefault();

            var currentStage = mostRecentLog?.Stage ?? 0;

            // If user got correct and previous log exist
            if (result && mostRecentLog != null)
            {
                if (mostRecentLog.Stage < _highestStage)
                    newLog.Stage = mostRecentLog.Stage + 1;
                else
                    newLog.Stage = _highestStage;
            }
            // Else the user got correct and no prior history,
            // Or the user got wrong - send to lowest stage
            else
            {
                newLog.Stage = _lowestStage;
            }

            _context.StudyLog.Add(newLog);
            await _context.SaveChangesAsync();

            // Get next card
            await SetNextCard(deckId, currentStage);

            if (Card == null)
                return RedirectToPage("./Study", new { id = deckId });

            return Page();
        }

        private async Task SetNextCard(int deckId, int stage)
        {
            Deck = await _context.Deck
                .FirstOrDefaultAsync(m => m.Id == deckId);

            if (stage != 0)
            {
                Card = await _context.Card
                    .Where(c => c.Decks.Any(d => d.DeckId == deckId) &&
                    c.Logs.OrderByDescending(l => l.Timestamp).FirstOrDefault().Stage == stage)
                    .FirstOrDefaultAsync();
            }
            else
            {
                Card = await _context.Card
                    .Where(c => c.Decks.Any(d => d.DeckId == deckId) && !c.Logs.Any())
                    .FirstOrDefaultAsync();
            }

            #region Old Card retrieval
            //// First prioritize cards with no study history
            //Card = await _context.Card
            //    .FirstOrDefaultAsync(c => c.Decks.Any(d => d.DeckId == deckId) && !c.Logs.Any());

            //if (Card != null)
            //    return;

            //// Else get most recent log for each card
            //var recentLogs = await _context.StudyLog.Include(l => l.Card)
            //    .Where(l => l.Card.Decks.Any(d => d.DeckId == deckId))
            //    .ToListAsync();

            //var priorityLogs  = recentLogs
            //    .GroupBy(l => l.Card)
            //    .Select(g => g.OrderByDescending(l => l.Timestamp).FirstOrDefault());

            //// Then choose card with lowest stage then oldest last studied
            //Card = priorityLogs.OrderBy(l => l.Stage).ThenBy(l => l.Timestamp).FirstOrDefault().Card;
            #endregion

            return;
        }
    }
}