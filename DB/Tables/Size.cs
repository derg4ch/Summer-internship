﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work_with_db.Tables
{
    public class Size
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ClothingItem> ClothingItems { get; set; }
    }
}
