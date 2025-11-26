using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    internal class Chest
    {
        public Item Contents { get; set; }

        public Item Open()
        {
            return Contents;
        }
    }
}
