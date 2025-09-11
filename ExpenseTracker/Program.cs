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
    }
}