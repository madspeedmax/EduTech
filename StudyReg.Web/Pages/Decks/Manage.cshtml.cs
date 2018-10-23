using System;
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

namespace StudyReg.Web.Pages.Decks
{
    public class ManageModel : PageModel
    {
        private readonly StudyReg.Web.Data.ApplicationDbContext _context;
        private readonly UserManager<StudyRegWebUser> _userManager;

        public ManageModel(StudyReg.Web.Data.ApplicationDbContext context, UserManager<StudyRegWebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Deck Deck { get; set; }

        public IList<DeckCard> DeckCards {get; set;}

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            Deck = await _context.Deck
                .FirstOrDefaultAsync(m => m.Id == id && m.User.Id == user.Id);

            if (Deck == null)
            {
                return NotFound();
            }

            DeckCards = await _context.DeckCard
                .Include(m => m.Card)
                .Where(m => m.DeckId == Deck.Id)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Deck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeckExists(Deck.Id))
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

        public async Task<IActionResult> OnPostDelete(int cardId, int deckId)
        {
            var deckCard = await _context.DeckCard.FindAsync(cardId, deckId);

            if (deckCard != null)
            {
                _context.DeckCard.Remove(deckCard);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Manage", new { id = deckId });
        }

        public async Task<IActionResult> OnPostAdd(int deckId, string cardTitle, string cardAnswer)
        {
            if (string.IsNullOrWhiteSpace(cardTitle) || string.IsNullOrWhiteSpace(cardAnswer))
            {
                return RedirectToPage("./Manage", new { id = deckId });
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            Deck = await _context.Deck.FirstOrDefaultAsync(m => m.Id == deckId && m.User.Id == user.Id);

            if (Deck == null)
            {
                return RedirectToPage("./Manage", new { id = deckId });
            }

            var card = new Card()
            {
                Title = cardTitle,
                Answer = cardAnswer,
                User = user
            };

            _context.Card.Add(card);
            await _context.SaveChangesAsync();

            var deckCard = new DeckCard() { CardId = card.Id, DeckId = deckId };
            _context.Add(deckCard);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Manage", new { id = deckId });
        }

        private bool DeckExists(int id)
        {
            return _context.Deck.Any(e => e.Id == id);
        }
    }
}
