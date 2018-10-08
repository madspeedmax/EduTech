using StudyReg.Web.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyReg.Web.Models
{
    public class Deck
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }

        [ForeignKey("UserId")]
        public StudyRegWebUser User { get; set; }

        public List<Card> Cards { get; set; }
    }
}
