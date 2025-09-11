using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExpenseTracker
{
    class Expense
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public Expense(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{Name}; {Amount} руб.";
        }
    }

    class Program
    {
        static List<Expense> expenses = new List<Expense>();

        static void Main(string[] args)
        {
            Console.WriteLine("=== Учет ежедневных расходов ===");

            int operationsCount = GetOperationsCount();

            InputExpenses(operationsCount);

            ShowMainMenu();
        }

        static int GetOperationsCount()
        {
            int count;
            while (true)
            {
                Console.Write("Введите количество операций (2-40): ");
                if (int.TryParse(Console.ReadLine(), out count) && count >= 2 && count <= 40)
                {
                    return count;
                }
                Console.WriteLine("Ошибка! Введите число от 2 до 40.");
            }
        }

        static void InputExpenses(int count)
        {
            Console.WriteLine("\nВведите данные о расходах в формате: Название; Сумма");

            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    Console.Write($"Операция {i + 1}: ");
                    string input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Ошибка! Ввод не может быть пустым.");
                        continue;
                    }


                    string[] parts = input.Split(';', StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Ошибка! Используйте формат: Название; Сумма");
                        continue;
                    }

                    string name = parts[0].Trim();
                    string amountStr = parts[1].Trim();

                    if (decimal.TryParse(amountStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount) && amount > 0)
                    {
                        expenses.Add(new Expense(name, amount));
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка! Введите корректную сумму.");
                    }
                }
            }
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== Главное меню ===");
                Console.WriteLine("1. Вывод данных");
                Console.WriteLine("2. Статистика (среднее, максимальное, минимальное, сумма)");
                Console.WriteLine("3. Сортировка по цене (пузырьковая сортировка)");
                Console.WriteLine("4. Конвертация валюты");
                Console.WriteLine("5. Поиск по названию");
                Console.WriteLine("6. Выход");

                Console.Write("Выберите пункт меню (1-6): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowData();
                        break;
                    case "2":
                        ShowStatistics();
                        break;
                    case "3":
                        BubbleSortByPrice();
                        break;
                    case "4":
                        CurrencyConversion();
                        break;
                    case "5":
                        SearchByName();
                        break;
                    case "6":
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор! Попробуйте снова.");
                        break;
                }
            }
        }

        static void ShowData()
        {
            Console.WriteLine("\n=== Список расходов ===");
            if (expenses.Count == 0)
            {
                Console.WriteLine("Нет данных о расходах.");
                return;
            }

            for (int i = 0; i < expenses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {expenses[i]}");
            }
        }

        static void ShowStatistics()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("Нет данных для статистики.");
                return;
            }

            decimal total = expenses.Sum(e => e.Amount);
            decimal average = expenses.Average(e => e.Amount);
            decimal max = expenses.Max(e => e.Amount);
            decimal min = expenses.Min(e => e.Amount);

            Console.WriteLine("\n=== Статистика расходов ===");
            Console.WriteLine($"Общая сумма: {total:F2} руб.");
            Console.WriteLine($"Средняя сумма: {average:F2} руб.");
            Console.WriteLine($"Максимальная сумма: {max:F2} руб.");
            Console.WriteLine($"Минимальная сумма: {min:F2} руб.");
            Console.WriteLine($"Количество операций: {expenses.Count}");
        }

        static void BubbleSortByPrice()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("Нет данных для сортировки.");
                return;
            }

            Console.WriteLine("\nВыберите порядок сортировки:");
            Console.WriteLine("1. По возрастанию");
            Console.WriteLine("2. По убыванию");
            Console.Write("Ваш выбор: ");

            string sortChoice = Console.ReadLine();
            bool ascending = sortChoice == "1";

            // puzir
            for (int i = 0; i < expenses.Count - 1; i++)
            {
                for (int j = 0; j < expenses.Count - i - 1; j++)
                {
                    bool shouldSwap = ascending ?
                        expenses[j].Amount > expenses[j + 1].Amount :
                        expenses[j].Amount < expenses[j + 1].Amount;

                    if (shouldSwap)
                    {
                        var temp = expenses[j];
                        expenses[j] = expenses[j + 1];
                        expenses[j + 1] = temp;
                    }
                }
            }

            Console.WriteLine("Сортировка завершена!");
            ShowData();
        }

        static void CurrencyConversion()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("Нет данных для конвертации.");
                return;
            }

            Console.WriteLine("\n=== Конвертация валюты ===");
            Console.WriteLine("Доступные валюты:");
            Console.WriteLine("1. Доллар США (USD)");
            Console.WriteLine("2. Евро (EUR)");
            Console.WriteLine("3. Фунт стерлингов (GBP)");
            Console.WriteLine("4. Произвольный курс");

            Console.Write("Выберите валюту (1-4): ");
            string currencyChoice = Console.ReadLine();

            decimal exchangeRate = 0;
            string currencySymbol = "";

            switch (currencyChoice)
            {
                case "1":
                    exchangeRate = GetExchangeRate("доллара");
                    currencySymbol = "USD";
                    break;
                case "2":
                    exchangeRate = GetExchangeRate("евро");
                    currencySymbol = "EUR";
                    break;
                case "3":
                    exchangeRate = GetExchangeRate("фунта стерлингов");
                    currencySymbol = "GBP";
                    break;
                case "4":
                    exchangeRate = GetExchangeRate("валюты");
                    Console.Write("Введите символ валюты: ");
                    currencySymbol = Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    return;
            }

            if (exchangeRate <= 0)
            {
                Console.WriteLine("Неверный курс валюты.");
                return;
            }

            Console.WriteLine($"\n=== Расходы в {currencySymbol} (курс: {exchangeRate:F2}) ===");
            foreach (var expense in expenses)
            {
                decimal convertedAmount = expense.Amount / exchangeRate;
                Console.WriteLine($"{expense.Name}; {convertedAmount:F2} {currencySymbol}");
            }

            decimal totalRub = expenses.Sum(e => e.Amount);
            decimal totalConverted = totalRub / exchangeRate;
            Console.WriteLine($"\nОбщая сумма: {totalConverted:F2} {currencySymbol} ({totalRub:F2} руб.)");
        }

        static decimal GetExchangeRate(string currencyName)
        {
            decimal rate;
            while (true)
            {
                Console.Write($"Введите курс {currencyName} к рублю: ");
                if (decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out rate) && rate > 0)
                {
                    return rate;
                }
                Console.WriteLine("Ошибка! Введите корректный курс.");
            }
        }

        static void SearchByName()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("Нет данных для поиска.");
                return;
            }

            Console.Write("\nВведите название для поиска: ");
            string searchTerm = Console.ReadLine().ToLower();

            var results = expenses.Where(e => e.Name.ToLower().Contains(searchTerm)).ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("Совпадений не найдено.");
                return;
            }

            Console.WriteLine($"\n=== Результаты поиска ({results.Count} найдено) ===");
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }
}
