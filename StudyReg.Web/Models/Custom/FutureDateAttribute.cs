﻿using System;
using System.ComponentModel.DataAnnotations;


namespace StudyReg.Web.Models.Custom
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && (DateTime)value > DateTime.Now;
        }
    }
}
