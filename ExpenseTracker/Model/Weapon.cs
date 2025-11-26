using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    internal class Weapon : Item
    {
        public int AttackBonus { get; set; }

        public Weapon()
        {
            Type = "Weapon";
        }
    
    }
}
