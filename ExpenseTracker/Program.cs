using System;
using System.Collections.Generic;
using System.Linq;

namespace TextRoguelike
{
    public class Player
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

    public class Enemy
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

    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } 
    }

    public class Weapon : Item
    {
        public int AttackBonus { get; set; }

        public Weapon()
        {
            Type = "Weapon";
        }
    }

    public class Armor : Item
    {
        public int DefenseBonus { get; set; }

        public Armor()
        {
            Type = "Armor";
        }
    }

    public class Potion : Item
    {
        public int HealAmount { get; set; }

        public Potion()
        {
            Type = "Potion";
        }
    }

    public class Chest
    {
        
    }

    public class Game
    {
        
    }

    
}