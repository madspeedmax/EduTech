﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eduTech.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Answer { get; set; }
    }
}