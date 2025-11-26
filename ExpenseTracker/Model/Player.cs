using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRoguelike;

namespace ExpenseTracker.Model
{
    internal class Player
    {
        public string Name { get; set; } = "Игрок";
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int BaseAttack { get; set; }
        public int BaseDefense { get; set; }
        public Weapon EquippedWeapon { get; set; }
        public Armor EquippedArmor { get; set; }
        public bool IsFrozen { get; set; }
        public bool IsDefending { get; set; }

        public int TotalAttack => BaseAttack + (EquippedWeapon?.AttackBonus ?? 0);
        public int TotalDefense => BaseDefense + (EquippedArmor?.DefenseBonus ?? 0);
        public bool IsAlive => Health > 0;

        public Player()
        {
            Health = 100;
            MaxHealth = 100;
            BaseAttack = 10;
            BaseDefense = 5;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        public void Heal(int amount)
        {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
        }

        public void FullHeal()
        {
            Health = MaxHealth;
        }

        public void ResetDefense()
        {
            IsDefending = false;
        }
    }
}
