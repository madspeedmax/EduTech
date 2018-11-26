using StudyReg.Web.Areas.Identity.Data;
using StudyReg.Web.Models.Custom;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyReg.Web.Models
{
    public class Goal
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [Display(Name = "Goal Date")]
        [DataType(DataType.Date)]
        public DateTime GoalDate { get; set; }

        [Display(Name = "Self-Assessment Date")]
        [DataType(DataType.Date)]
        public DateTime SelfAssessmentDate { get; set; }

        [Display(Name = "Were the goal expectations met?")]
        [DataType(DataType.MultilineText)]
        [StringLength(5000)]
        public string SelfAssessmentExpectation { get; set; }

        [Display(Name = "What will you adjust for next time?")]
        [DataType(DataType.MultilineText)]
        [StringLength(5000)]
        public string SelfAssessmentAdjustment { get; set; }

        [Display(Name = "Grade Received (Leave blank if N/A)")]
        [StringLength(200)]
        public string Grade { get; set; }

        [ForeignKey("UserId")]
        public StudyRegWebUser User { get; set; }

        [ForeignKey("DeckId")]
        public Deck Deck { get; set; }
    }
}
