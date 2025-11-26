using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    internal class EnemySimpleFactory
    {
        
        public static Enemy CreateEnemy(int enemyType)
        {
            
            Enemy enemy;
            switch (enemyType)
            {
                case 0:
                    enemy = new Enemy
                    {
                        Name = "Гоблин",
                        Race = "Goblin",
                        Health = 30,
                        MaxHealth = 30,
                        Attack = 8,
                        Defense = 3,
                        CriticalChance = 0.2,
                        IgnoresDefense = false
                    };

                    break;
                case 1:
                    enemy = new Enemy
                    {
                        Name = "Скелет",
                        Race = "Skeleton",
                        Health = 25,
                        MaxHealth = 25,
                        Attack = 10,
                        Defense = 2,
                        IgnoresDefense = true
                    };
                    break;
                case 2:
                    enemy = new Enemy
                    {
                        Name = "Слизень",
                        Race = "Slime",
                        Health = 10,
                        MaxHealth = 10,
                        Attack = 2,
                        Defense = 2,
                        IgnoresDefense = false
                    };
                    break;
                default:
                    enemy = new Enemy
                    {
                        Name = "Маг",
                        Race = "Mage",
                        Health = 20,
                        MaxHealth = 20,
                        Attack = 12,
                        Defense = 1,
                        FreezeChance = 0.15,
                        IgnoresDefense = false
                    };
                    break;
            }
            return enemy;

        }
        public static Enemy CreateBoss(int bossType)
        {
            Enemy boss;
            

            switch (bossType)
            {
                case 0:
                    boss = new Enemy
                    {
                        Name = "ВВГ",
                        Race = "Goblin",
                        Health = 60,
                        MaxHealth = 60,
                        Attack = 12,
                        Defense = 4,
                        CriticalChance = 0.3,
                        IgnoresDefense = false,
                        IsBoss = true
                    };
                    break;
                case 1:
                    boss = new Enemy
                    {
                        Name = "Ковальский",
                        Race = "Skeleton",
                        Health = 63,
                        MaxHealth = 63,
                        Attack = 13,
                        Defense = 3,
                        IgnoresDefense = true,
                        IsBoss = true
                    };
                    break;
                case 2:
                    boss = new Enemy
                    {
                        Name = "Архимаг C++",
                        Race = "Mage",
                        Health = 36,
                        MaxHealth = 36,
                        Attack = 19,
                        Defense = 2,
                        FreezeChance = 0.25,
                        IgnoresDefense = false,
                        IsBoss = true
                    };
                    break;
                default:
                    boss = new Enemy
                    {
                        Name = "Пестов С--",
                        Race = "Skeleton",
                        Health = 33,
                        MaxHealth = 33,
                        Attack = 18,
                        Defense = 1,
                        FreezeChance = 0.3,
                        IgnoresDefense = true,
                        IsBoss = true
                    };
                    break;
            }

            return boss;


        }

    }
}
