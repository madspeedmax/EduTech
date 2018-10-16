using StudyReg.Web.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyReg.Web.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Answer { get; set; }

        [ForeignKey("UserId")]
        public StudyRegWebUser User { get; set; }
        public ICollection<DeckCard> Decks { get; set; }
        public ICollection<StudyLog> Logs { get; set; }
    }
}
