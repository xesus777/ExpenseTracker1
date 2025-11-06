
using _3ISIP323_ZHDANOVICH_PR8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMWOG_Marketplace
{
        class Program
        {
            private static Users currentUser;
            private static List<Products> shoppingCart;

            static Program()
            {
                shoppingCart = new List<Products>();
            }

            static void Main(string[] args)
            {
                Console.WriteLine("Добро пожаловать в GMWOGGG.MOWWONGG!");

                while (true)
                {
                    Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
                    Console.WriteLine("1. Просмотр товаров");
                    Console.WriteLine("2. Регистрация");
                    Console.WriteLine("3. Вход в аккаунт");
                    Console.WriteLine("4. Корзина");
                    Console.WriteLine("5. Выход");
                    Console.Write("Выберите действие: ");

                    var choice = Console.ReadLine();

                    
                }
            }

            // Просмотр товаров
            private static void ViewProducts()
            {
                Console.WriteLine("\n=== КАТАЛОГ ТОВАРОВ ===");

                var products = Core.Context.Products.Where(p => p.IsActive == true && p.Quantity > 0).ToList();

                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Id}. {product.Name}");
                    Console.WriteLine($"   Описание: {product.Description}");
                    Console.WriteLine($"   Цена: {product.Price:C}");
                    Console.WriteLine($"   В наличии: {product.Quantity} шт.");
                    Console.WriteLine();
                }

                if (currentUser != null)
                {
                    Console.Write("Хотите добавить товар в корзину? (y/n): ");
                    var addToCart = Console.ReadLine()?.ToLower();

                    if (addToCart == "y")
                    {
                        AddToCart();
                    }
                }
                else
                {
                    Console.WriteLine("Для добавления товаров в корзину необходимо войти в аккаунт.");
                }
            }

            // Добавление товара в корзину
            private static void AddToCart()
            {
                Console.Write("Введите ID товара для добавления в корзину: ");
                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    var product = Core.Context.Products.FirstOrDefault(p => p.Id == productId && p.IsActive == true && p.Quantity > 0);
                    if (product != null)
                    {
                        var existingCartItem = Core.Context.Cart.FirstOrDefault(c => c.UserId == currentUser.Id && c.ProductId == productId);

                        if (existingCartItem != null)
                        {
                            existingCartItem.Quantity += 1;

    Console.WriteLine($"Количество товара '{product.Name}' в корзине увеличено!");
                        }
                        else
                        {
                            var cartItem = new Cart
                            {
                                UserId = currentUser.Id,
                                ProductId = productId,
                                Quantity = 1
                            };
                            Core.Context.Cart.Add(cartItem);
                            Console.WriteLine($"Товар '{product.Name}' добавлен в корзину!");
                        }

                        Core.Context.SaveChanges();

                        shoppingCart.Add(product);
                    }
                    else
                    {
                        Console.WriteLine("Товар не найден или отсутствует в наличии.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный формат ID.");
                }
            }

            

            

            
            }


            

            

            

            

            
            }
        }
}
