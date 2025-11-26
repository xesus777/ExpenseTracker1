using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    internal class Enemy
    {
        public string Name { get; set; }
        public string Race { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public double CriticalChance { get; set; }
        public double FreezeChance { get; set; }
        public bool IgnoresDefense { get; set; }
        public bool IsBoss { get; set; }

        public bool IsAlive => Health > 0;

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        public int CalculateDamage(Player player, Random random)
        {
            int baseDamage = Attack;

            if (random.NextDouble() < CriticalChance)
            {
                baseDamage = (int)(baseDamage * 1.5);
                Console.WriteLine("Критический удар!");
            }

            if (IgnoresDefense)
            {
                return baseDamage;
            }

            return baseDamage;
        }

        public bool TryApplyFreeze(Player player, Random random)
        {
            if (random.NextDouble() < FreezeChance)
            {
                player.IsFrozen = true;
                return true;
            }
            return false;
        }
    }
}
