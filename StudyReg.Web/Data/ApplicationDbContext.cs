using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyReg.Web.Areas.Identity.Data;
using StudyReg.Web.Models;

namespace StudyReg.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<StudyRegWebUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<StudyReg.Web.Models.Card> Card { get; set; }
        public DbSet<StudyReg.Web.Models.Deck> Deck { get; set; }
    }
}
