
namespace StudyReg.Web.Models
{
    public class DeckCard
    {
        public int CardId { get; set; }
        public int DeckId { get; set; }
        public Card Card { get; set; }
        public Deck Deck { get; set; }
    }
}
