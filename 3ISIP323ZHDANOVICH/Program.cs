using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP323ZHDANOVICH
{
    internal class Program
    {
        static void Main()
        {
            // Начальные настройки игры
            decimal balance = 1000m; // Начальный баланс
            bool gameOver = false;
            Random random = new Random();

            // Список деталей: название, цена, количество
            Dictionary<string, (decimal price, int quantity)> parts = new Dictionary<string, (decimal, int)>
        {
            {"двигатель", (200m, 2)},
            {"тормоза", (50m, 3)},
            {"аккумулятор", (80m, 2)},
            {"шины", (40m, 4)},
            {"фара", (30m, 3)},
            {"стекло", (60m, 2)}
        };

            // Список возможных поломок
            string[] possibleProblems = { "двигатель", "тормоза", "аккумулятор", "шины", "фара", "стекло" };

            Console.WriteLine("=== АВТОМАСТЕРСКАЯ ===");
            Console.WriteLine("Добро пожаловать в автомастерскую!");
            Console.WriteLine($"Ваш начальный баланс: {balance} руб.");
            Console.WriteLine("Правила:");
            Console.WriteLine("- Принимайте клиентов и ремонтируйте их машины");
            Console.WriteLine("- Если возьметесь за ремонт без нужной детали - штраф 100 руб.");
            Console.WriteLine("- Если откажетесь от ремонта - штраф 50 руб.");
            Console.WriteLine("- Покупайте детали в магазине");
            Console.WriteLine("- Игра окончена, когда деньги закончатся");
            Console.WriteLine("------------------------");

            while (!gameOver)
            {
                // Генерация случайной поломки
                string brokenPart = possibleProblems[random.Next(possibleProblems.Length)];
                decimal repairPayment = parts[brokenPart].price * 1.5m; // Оплата за ремонт (+50%)

                Console.WriteLine($"\nПриехал клиент! Сломано: {brokenPart}");
                Console.WriteLine($"Оплата за ремонт: {repairPayment} руб.");
                Console.WriteLine($"Ваш баланс: {balance} руб.");
                Console.WriteLine($"Деталь '{brokenPart}' в наличии: {parts[brokenPart].quantity} шт.");

                // Показ меню
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1 - Починить машину");
                Console.WriteLine("2 - Отказаться от ремонта");
                Console.WriteLine("3 - Купить детали в магазине");
                Console.WriteLine("4 - Показать склад");
                Console.WriteLine("0 - Выйти из игры");

                Console.Write("Ваш выбор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Починить машину
                        if (parts[brokenPart].quantity > 0)
                        {
                            // Есть деталь - успешный ремонт
                            parts[brokenPart] = (parts[brokenPart].price, parts[brokenPart].quantity - 1);
                            balance += repairPayment;
                            Console.WriteLine($"✅ Вы успешно починили {brokenPart}!");
                            Console.WriteLine($"💰 Получено: {repairPayment} руб.");
                        }
                        else
                        {
                            // Нет детали - штраф
                            balance -= 100m;
                            Console.WriteLine("❌ У вас нет нужной детали! Штраф 100 руб.");
                            Console.WriteLine("Клиент уехал недовольный!");
                        }
                        break;

                    case "2": // Отказаться от ремонта
                        balance -= 50m;
                        Console.WriteLine("🚫 Вы отказались от ремонта. Штраф 50 руб.");
                        Console.WriteLine("Клиент уехал искать другую мастерскую.");
                        break;
                    case "3": // Магазин деталей
                        Console.WriteLine("\n🏪 МАГАЗИН ДЕТАЛЕЙ:");
                        Console.WriteLine("Доступные детали:");

                        int index = 1;
                        foreach (var part in parts)
                        {
                            Console.WriteLine($"{index} - {part.Key}: {part.Value.price} руб. (на складе: {part.Value.quantity})");
                            index++;
                        }

                        Console.Write("Выберите номер детали для покупки: ");
                        if (int.TryParse(Console.ReadLine(), out int partChoice) && partChoice >= 1 && partChoice <= parts.Count)
                        {
                            string selectedPart = possibleProblems[partChoice - 1];
                            Console.Write($"Сколько '{selectedPart}' хотите купить? ");

                            if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                            {
                                decimal totalCost = parts[selectedPart].price * quantity;

                                if (balance >= totalCost)
                                {
                                    balance -= totalCost;
                                    parts[selectedPart] = (parts[selectedPart].price, parts[selectedPart].quantity + quantity);
                                    Console.WriteLine($"✅ Куплено {quantity} шт. '{selectedPart}' за {totalCost} руб.");
                                }
                                else
                                {
                                    Console.WriteLine("❌ Недостаточно денег для покупки!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("❌ Неверное количество!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("❌ Неверный выбор детали!");
                        }
                        break;

                    case "4": // Показать склад
                        Console.WriteLine("\n📦 СКЛАД ДЕТАЛЕЙ:");
                        foreach (var part in parts)
                        {
                            Console.WriteLine($"- {part.Key}: {part.Value.quantity} шт. (цена: {part.Value.price} руб.)");
                        }
                        Console.WriteLine($"💰 Баланс: {balance} руб.");
                        break;

                    case "0": // Выйти из игры
                        gameOver = true;
                        Console.WriteLine("Спасибо за игру!");
                        break;

                    default:
                        Console.WriteLine("❌ Неверный выбор! Попробуйте снова.");
                        break;
                }

                // Проверка на окончание игры
                if (balance <= 0)
                {
                    gameOver = true;
                    Console.WriteLine("\n💀 ИГРА ОКОНЧЕНА!");
                    Console.WriteLine("У вас закончились деньги!");
                    Console.WriteLine("Вы банкрот!");
                }

                // Небольшая пауза между ходами
                if (!gameOver)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine($"\nИтоговый баланс: {balance} руб.");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
