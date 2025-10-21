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
        public Item Contents { get; set; }

        public Item Open()
        {
            return Contents;
        }
    }

    public class Game
    {
        public Player Player { get; set; }
        public int TurnCount { get; set; }
        public Random Random { get; set; }
        private List<Weapon> availableWeapons;
        private List<Armor> availableArmors;

        public Game()
        {
            Player = new Player();
            Random = new Random();
            TurnCount = 0;
            InitializeItems();
        }

        private void InitializeItems()
        {
            availableWeapons = new List<Weapon>
            {
                new Weapon { Name = "Ржавый меч", AttackBonus = 3, Description = "Простой меч" },
                new Weapon { Name = "Стальной клинок", AttackBonus = 7, Description = "Качественное оружие" },
                new Weapon { Name = "Магический посох", AttackBonus = 12, Description = "Оружие с магической силой" },
                new Weapon { Name = "Легендарный клинок", AttackBonus = 20, Description = "Мощное легендарное оружие" }
            };

            availableArmors = new List<Armor>
            {
                new Armor { Name = "Кожаная броня", DefenseBonus = 2, Description = "Легкая броня" },
                new Armor { Name = "Кольчуга", DefenseBonus = 5, Description = "Надежная защита" },
                new Armor { Name = "Латные доспехи", DefenseBonus = 10, Description = "Тяжелая броня" },
                new Armor { Name = "Магические доспехи", DefenseBonus = 15, Description = "Броня с магической защитой" }
            };
        }

        public void StartGame()
        {
            Console.WriteLine("=== ТЕКСТОВЫЙ РОГАЛИК ===");
            Console.WriteLine("Добро пожаловать в игру!");
            Console.WriteLine("Каждый ход вы будете встречать врага или находить сундук.");
            Console.WriteLine("Каждые 10 ходов вас ждет встреча с боссом!");
            Console.WriteLine("Удачи!\n");

            while (Player.IsAlive)
            {
                ProcessTurn();
                if (Player.IsAlive)
                {
                    Console.WriteLine("\nНажмите любую клавишу для следующего хода...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Console.WriteLine("\n=== ИГРА ОКОНЧЕНА ===");
            Console.WriteLine($"Вы продержались {TurnCount} ходов!");
        }

        public void ProcessTurn()
        {
            TurnCount++;
            Player.ResetDefense();

            Console.WriteLine($"=== ХОД {TurnCount} ===");
            Console.WriteLine($"Здоровье: {Player.Health}/{Player.MaxHealth}");
            Console.WriteLine($"Атака: {Player.TotalAttack} | Защита: {Player.TotalDefense}");

            if (Player.IsFrozen)
            {
                Console.WriteLine("❄️ Вы заморожены и пропускаете ход!");
                Player.IsFrozen = false;
                return;
            }

            if (TurnCount % 10 == 0)
            {
                Console.WriteLine("💀 Появляется БОСС!");
                Enemy boss = GenerateRandomBoss();
                StartBattle(boss);
            }
            else
            {
                if (Random.Next(2) == 0)
                {
                    Console.WriteLine("🎁 Вы нашли сундук!");
                    Chest chest = GenerateRandomChest();
                    OpenChest(chest);
                }
                else
                {
                    Enemy enemy = GenerateRandomEnemy();
                    Console.WriteLine($"⚔️ Появляется {enemy.Name}!");
                    StartBattle(enemy);
                }
            }
        }
        public void StartBattle(Enemy enemy)
        {
            Console.WriteLine($"\n=== БОЙ С {enemy.Name.ToUpper()} ===");
            Console.WriteLine($"Здоровье врага: {enemy.Health}");
            if (enemy.IsBoss) Console.WriteLine("⚠️  Это БОСС - будьте осторожны!");

            while (enemy.IsAlive && Player.IsAlive)
            {
                PlayerTurn(enemy);
                if (!enemy.IsAlive) break;

                EnemyTurn(enemy);
            }

            if (enemy.IsAlive)
            {
                Console.WriteLine("Вы пали в бою...");
            }
            else
            {
                Console.WriteLine($"🎉 Вы победили {enemy.Name}!");
                if (enemy.IsBoss)
                {
                    Console.WriteLine("🏆 ПОБЕДА НАД БОССОМ! Вы получаете полное лечение!");
                    Player.FullHeal();
                }
            }
        }

        private void PlayerTurn(Enemy enemy)
        {
            Console.WriteLine("\n--- Ваш ход ---");
            Console.WriteLine("1. Атаковать");
            Console.WriteLine("2. Защищаться");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    int playerDamage = Player.TotalAttack;
                    enemy.TakeDamage(playerDamage);
                    Console.WriteLine($"Вы наносите {playerDamage} урона!");
                    Console.WriteLine($"Здоровье {enemy.Name}: {enemy.Health}/{enemy.MaxHealth}");
                    break;

                case "2":
                    Player.IsDefending = true;
                    Console.WriteLine("Вы готовитесь к защите...");
                    break;

                default:
                    Console.WriteLine("Неверный выбор! Вы пропускаете ход.");
                    break;
            }
        }

        private void EnemyTurn(Enemy enemy)
        {
            if (!Player.IsAlive) return;

            Console.WriteLine($"\n--- Ход {enemy.Name} ---");

            if (enemy.TryApplyFreeze(Player, Random))
            {
                Console.WriteLine("❄️ Маг наложил на вас заморозку! Вы пропустите следующий ход.");
            }

            int enemyDamage = enemy.CalculateDamage(Player, Random);

            if (Player.IsDefending)
            {
                if (Random.NextDouble() < 0.4) 
                {
                    Console.WriteLine("🎯 Вы увернулись от атаки!");
                    return;
                }
                else
                {
                    double blockPercent = 0.7 + (Random.NextDouble() * 0.3);
                    int blockedDamage = (int)(Player.TotalDefense * blockPercent);
                    enemyDamage = Math.Max(0, enemyDamage - blockedDamage);
                    Console.WriteLine($"🛡️ Вы блокируете {blockedDamage} урона!");
                }
            }

            if (enemy.IgnoresDefense)
            {
                Console.WriteLine($"💀 {enemy.Name} игнорирует вашу защиту!");
            }

            Player.TakeDamage(enemyDamage);
            Console.WriteLine($"{enemy.Name} наносит вам {enemyDamage} урона!");
            Console.WriteLine($"Ваше здоровье: {Player.Health}/{Player.MaxHealth}");
        }

        public void OpenChest(Chest chest)
        {
            Item item = chest.Open();

            switch (item.Type)
            {
                case "Potion":
                    Potion potion = (Potion)item;
                    Console.WriteLine($"🧪 Вы нашли зелье лечения! +{potion.HealAmount} HP");
                    Player.Heal(potion.HealAmount);
                    Console.WriteLine($"Теперь у вас {Player.Health}/{Player.MaxHealth} HP");
                    break;

                case "Weapon":
                    Weapon newWeapon = (Weapon)item;
                    Console.WriteLine($"⚔️ Вы нашли оружие: {newWeapon.Name}");
                    Console.WriteLine($"Атака: +{newWeapon.AttackBonus}");
                    Console.WriteLine($"Текущее оружие: {Player.EquippedWeapon?.Name ?? "Нет"} (+{Player.EquippedWeapon?.AttackBonus ?? 0})");

                    Console.Write("Взять новое оружие? (1-да, 2-нет): ");
                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Player.EquippedWeapon = newWeapon;
                        Console.WriteLine($"✅ Вы экипировали {newWeapon.Name}!");
                    }
                    else
                    {
                        Console.WriteLine("❌ Вы оставили оружие в сундуке.");
                    }
                    break;

                case "Armor":
                    Armor newArmor = (Armor)item;
                    Console.WriteLine($"🛡️ Вы нашли доспехи: {newArmor.Name}");
                    Console.WriteLine($"Защита: +{newArmor.DefenseBonus}");
                    Console.WriteLine($"Текущие доспехи: {Player.EquippedArmor?.Name ?? "Нет"} (+{Player.EquippedArmor?.DefenseBonus ?? 0})");

                    Console.Write("Взять новые доспехи? (1-да, 2-нет): ");
                    choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Player.EquippedArmor = newArmor;
                        Console.WriteLine($"✅ Вы экипировали {newArmor.Name}!");
                    }
                    else
                    {
                        Console.WriteLine("❌ Вы оставили доспехи в сундуке.");
                    }
                    break;
            }
        }

        public Enemy GenerateRandomEnemy()
        {
            int enemyType = Random.Next(3);

            return enemyType switch
            {
                0 => new Enemy
                {
                    Name = "Гоблин",
                    Race = "Goblin",
                    Health = 30,
                    MaxHealth = 30,
                    Attack = 8,
                    Defense = 3,
                    CriticalChance = 0.2,
                    IgnoresDefense = false
                },
                1 => new Enemy
                {
                    Name = "Скелет",
                    Race = "Skeleton",
                    Health = 25,
                    MaxHealth = 25,
                    Attack = 10,
                    Defense = 2,
                    IgnoresDefense = true
                },
                _ => new Enemy
                {
                    Name = "Маг",
                    Race = "Mage",
                    Health = 20,
                    MaxHealth = 20,
                    Attack = 12,
                    Defense = 1,
                    FreezeChance = 0.15,
                    IgnoresDefense = false
                }
            };
        }

        public Enemy GenerateRandomBoss()
        {
            int bossType = Random.Next(4);

            return bossType switch
            {
                0 => new Enemy
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
                },
                1 => new Enemy
                {
                    Name = "Ковальский",
                    Race = "Skeleton",
                    Health = 63,
                    MaxHealth = 63,
                    Attack = 13,
                    Defense = 3,
                    IgnoresDefense = true,
                    IsBoss = true
                },
                2 => new Enemy
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
                },
                _ => new Enemy
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
                }
            };
        }

        public Chest GenerateRandomChest()
        {
            int itemType = Random.Next(3);
            Item item;

            switch (itemType)
            {
                case 0: 
                    item = new Potion
                    {
                        Name = "Лечебное зелье",
                        HealAmount = 30 + Random.Next(21),
                        Description = "Восстанавливает здоровье"
                    };
                    break;

                case 1: 
                    item = availableWeapons[Random.Next(availableWeapons.Count)];
                    break;

                default: 
                    item = availableArmors[Random.Next(availableArmors.Count)];
                    break;
            }

            return new Chest { Contents = item };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.StartGame();
        }
    }
}