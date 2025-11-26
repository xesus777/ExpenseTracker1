using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    internal class Armor : Item
    {
        public int DefenseBonus { get; set; }

        public Armor()
        {
            Type = "Armor";
        }
    }
}
