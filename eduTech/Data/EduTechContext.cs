using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace eduTech.Models
{
    public class EduTechContext : DbContext
    {
        public EduTechContext (DbContextOptions<EduTechContext> options)
            : base(options)
        {
        }

        public DbSet<eduTech.Models.Card> Card { get; set; }
    }
}
