using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    internal abstract class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}
