using StudyReg.Web.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyReg.Web.Models
{
    public class StudyLog
    {
        public Int64 Id { get; set; }
        public bool Result { get; set; }
        public int Stage { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public StudyRegWebUser User { get; set; }

        [ForeignKey("CardId")]
        [Required]
        public Card Card { get; set; }
    }
}
