using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    internal class Potion : Item
    {
        public int HealAmount { get; set; }

        public Potion()
        {
            Type = "Potion";
        }
    }
}
