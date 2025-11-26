using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    internal class Game
    {
        Player Player { get; set; }
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

            bool gameRunning = true;

            while (gameRunning && Player.IsAlive)
            {
                ProcessTurn();

                if (Player.IsAlive)
                {
                    Console.WriteLine("\nНажмите любую клавишу для следующего хода...");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    gameRunning = false;
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
                Console.WriteLine("Вы заморожены и пропускаете ход!");
                Player.IsFrozen = false;
                return;
            }

            if (TurnCount % 10 == 0)
            {
                Console.WriteLine("Появляется БОСС!");
                int bossType = Random.Next(4);
                Enemy boss = EnemySimpleFactory.CreateBoss(bossType);
                StartBattle(boss);
            }
            else
            {
                if (Random.Next(2) == 0)
                {
                    Console.WriteLine("Вы нашли сундук!");
                    Chest chest = GenerateRandomChest();
                    OpenChest(chest);
                }
                else
                {
                    int enemyType = Random.Next(3);
                    Enemy enemy = EnemySimpleFactory.CreateEnemy(enemyType);
                    Console.WriteLine($"Появляется {enemy.Name}!");
                    StartBattle(enemy);
                }
            }
        }

        void StartBattle(Enemy enemy)
        {
            Console.WriteLine($"\n=== БОЙ С {enemy.Name.ToUpper()} ===");
            Console.WriteLine($"Здоровье врага: {enemy.Health}");
            if (enemy.IsBoss) Console.WriteLine("Это БОСС - будьте осторожны!");

            bool battleInProgress = true;

            while (battleInProgress && enemy.IsAlive && Player.IsAlive)
            {
                PlayerTurn(enemy);
                if (!enemy.IsAlive)
                {
                    battleInProgress = false;
                    break;
                }

                EnemyTurn(enemy);
            }

            if (!Player.IsAlive)
            {
                Console.WriteLine("Вы пали в бою...");
            }
            else if (!enemy.IsAlive)
            {
                Console.WriteLine($"Вы победили {enemy.Name}!");
                if (enemy.IsBoss)
                {
                    Console.WriteLine("ПОБЕДА НАД БОССОМ! Вы получаете полное лечение!");
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
                    if (enemy.Name == "Слизень")
                    {
                        int playerDamage = Player.TotalAttack;
                        enemy.TakeDamage(playerDamage-2);
                        Console.WriteLine($"Вы наносите {playerDamage} урона! (на 2 меньше тк это слизень)");
                        Console.WriteLine($"Здоровье {enemy.Name}: {enemy.Health}/{enemy.MaxHealth}");
                    }
                    else {
                        int playerDamage = Player.TotalAttack;
                        enemy.TakeDamage(playerDamage);
                        Console.WriteLine($"Вы наносите {playerDamage} урона!");
                        Console.WriteLine($"Здоровье {enemy.Name}: {enemy.Health}/{enemy.MaxHealth}");
                    }
                    
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
                Console.WriteLine("Маг наложил на вас заморозку! Вы пропустите следующий ход.");
            }

            int enemyDamage = enemy.CalculateDamage(Player, Random);

            if (Player.IsDefending)
            {
                if (Random.NextDouble() < 0.4)
                {
                    Console.WriteLine("Вы увернулись от атаки!");
                    return;
                }
                else
                {
                    double blockPercent = 0.7 + (Random.NextDouble() * 0.3);
                    int blockedDamage = (int)(Player.TotalDefense * blockPercent);
                    enemyDamage = Math.Max(0, enemyDamage - blockedDamage);
                    Console.WriteLine($"Вы блокируете {blockedDamage} урона!");
                }
            }

            if (enemy.IgnoresDefense)
            {
                Console.WriteLine($"{enemy.Name} игнорирует вашу защиту!");
            }

            Player.TakeDamage(enemyDamage);
            Console.WriteLine($"{enemy.Name} наносит вам {enemyDamage} урона!");
            Console.WriteLine($"Ваше здоровье: {Player.Health}/{Player.MaxHealth}");
        }

        void OpenChest(Chest chest)
        {
            Item item = chest.Open();

            switch (item.Type)
            {
                case "Potion":
                    Potion potion = (Potion)item;
                    Console.WriteLine($"Вы нашли зелье лечения! +{potion.HealAmount} HP");
                    Player.Heal(potion.HealAmount);
                    Console.WriteLine($"Теперь у вас {Player.Health}/{Player.MaxHealth} HP");
                    break;

                case "Weapon":
                    Weapon newWeapon = (Weapon)item;
                    Console.WriteLine($"Вы нашли оружие: {newWeapon.Name}");
                    Console.WriteLine($"Атака: +{newWeapon.AttackBonus}");
                    Console.WriteLine($"Текущее оружие: {Player.EquippedWeapon?.Name ?? "Нет"} (+{Player.EquippedWeapon?.AttackBonus ?? 0})");

                    Console.Write("Взять новое оружие? (1-да, 2-нет): ");
                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Player.EquippedWeapon = newWeapon;
                        Console.WriteLine($" Вы экипировали {newWeapon.Name}!");
                    }
                    else
                    {
                        Console.WriteLine(" Вы оставили оружие в сундуке.");
                    }
                    break;

                case "Armor":
                    Armor newArmor = (Armor)item;
                    Console.WriteLine($" Вы нашли доспехи: {newArmor.Name}");
                    Console.WriteLine($"Защита: +{newArmor.DefenseBonus}");
                    Console.WriteLine($"Текущие доспехи: {Player.EquippedArmor?.Name ?? "Нет"} (+{Player.EquippedArmor?.DefenseBonus ?? 0})");

                    Console.Write("Взять новые доспехи? (1-да, 2-нет): ");
                    choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Player.EquippedArmor = newArmor;
                        Console.WriteLine($" Вы экипировали {newArmor.Name}!");
                    }
                    else
                    {
                        Console.WriteLine(" Вы оставили доспехи в сундуке.");
                    }
                    break;
            }
        }

        

        

        Chest GenerateRandomChest()
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
}
