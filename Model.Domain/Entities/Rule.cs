﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Entities
{
    public class Rule : BaseEntity
    {

        public decimal MinValue { get; set; } = 0.0m;
        public decimal MaxValue { get; set; } = decimal.MaxValue;
        public bool Action { get; set; }
        public Category Category { get; set; }
        public bool IsActive { get; set; }
    }
}