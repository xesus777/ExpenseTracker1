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
            
            decimal balance = 1000m; 
            bool gameOver = false;
            Random random = new Random();

            List<Products> parts = Core.Context.Products.ToList();
            

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
                
                Products brokenPart = parts[random.Next(parts.Count)];
                decimal repairPayment = brokenPart.price * 1.5m;

                Console.WriteLine($"\nПриехал клиент! Сломано: {brokenPart.name}");
                Console.WriteLine($"Оплата за ремонт: {repairPayment} руб.");
                Console.WriteLine($"Ваш баланс: {balance} руб.");
                Console.WriteLine($"Деталь '{brokenPart.name}' в наличии: {brokenPart.count} шт.");

                
                

                

                
                
            }

            
        }
    }
}
