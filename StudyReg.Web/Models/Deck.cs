using StudyReg.Web.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyReg.Web.Models
{
    public class Deck
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Created { get; set; }

        [ForeignKey("UserId")]
        public StudyRegWebUser User { get; set; }
        public ICollection<DeckCard> Cards { get; set; }
    }
}
