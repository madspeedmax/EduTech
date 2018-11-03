using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyReg.Web.Areas.Identity.Services
{
    public class MessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}
