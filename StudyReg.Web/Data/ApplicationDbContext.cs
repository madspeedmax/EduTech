using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyReg.Web.Areas.Identity.Data;

namespace StudyReg.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<StudyRegWebUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
