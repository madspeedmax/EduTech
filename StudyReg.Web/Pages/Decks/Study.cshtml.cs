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

        public List<(int Stage, int Count)> Stages { get; set; }

        public int Remaining { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            Deck = await _context.Deck.FirstOrDefaultAsync(m => m.Id == id && m.User.Id == user.Id);

            if (Deck == null)
            {
                return NotFound();
            }

            var recentLogs = await _context.Card
                .Where(c => c.Decks.Any(d => d.DeckId == id))
                .Select(c => c.Logs.OrderByDescending(l => l.Timestamp).FirstOrDefault())
                .ToListAsync();

            Stages = new List<(int Stage, int Count)>();

            for (var stage = 0; stage <= _highestStage; stage++)
            {
                var count = 0;
                if (stage == 0)
                    count = recentLogs.Where(l => l?.Stage == null).Count();
                else
                    count = recentLogs.Where(l => l?.Stage != null && l.Stage == stage).Count();

                Stages.Add((stage, count));
            }

            return Page();
        }

        public async Task<IActionResult> OnPostStart(int deckId, int stage, int numCards)
        {
            await SetNextCard(deckId, stage, numCards);
            Remaining = numCards - 1;
            return Page();
        }

        public async Task<IActionResult> OnPostAnswer(int deckId, int cardId, bool result, int cardsLeft)
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
            await SetNextCard(deckId, currentStage, cardsLeft);

            Remaining = cardsLeft - 1;

            if (Card == null)
                return RedirectToPage("./Study", new { id = deckId });

            return Page();
        }

        private async Task SetNextCard(int deckId, int stage, int cardsLeft)
        {
            Deck = await _context.Deck
                .FirstOrDefaultAsync(m => m.Id == deckId);

            if (cardsLeft <= 0)
            {
                Card = null;
                return;
            }
                
            if (stage != 0)
            {
                Card = await _context.Card
                    .Where(c => c.Decks.Any(d => d.DeckId == deckId) &&
                    c.Logs.OrderByDescending(l => l.Timestamp).FirstOrDefault().Stage == stage)
                    .OrderBy(c => c.Logs.OrderByDescending(l => l.Timestamp).FirstOrDefault().Timestamp)
                    .FirstOrDefaultAsync();
            }
            else
            {
                Card = await _context.Card
                    .Where(c => c.Decks.Any(d => d.DeckId == deckId) && !c.Logs.Any())
                    .FirstOrDefaultAsync();
            }
        }
    }
}