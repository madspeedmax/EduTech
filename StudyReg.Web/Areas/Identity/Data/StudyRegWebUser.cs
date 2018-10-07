using System;
using Microsoft.AspNetCore.Identity;

namespace StudyReg.Web.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the StudyRegWebUser class
    public class StudyRegWebUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
    }
}
