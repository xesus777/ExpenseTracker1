
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

            // Просмотр корзины
            private static void ViewShoppingCart()
            {
                if (currentUser == null)
                {
                    Console.WriteLine("Для просмотра корзины необходимо войти в аккаунт.");
                    return;
                }

                Console.WriteLine("\n=== КОРЗИНА ===");

                var cartItems = Core.Context.Cart
                    .Where(c => c.UserId == currentUser.Id)
                    .ToList();

                var productIds = cartItems.Select(c => c.ProductId).ToList();
                var products = Core.Context.Products.Where(p => productIds.Contains(p.Id)).ToList();

                if (!cartItems.Any() && shoppingCart.Count == 0)
                {
                    Console.WriteLine("Корзина пуста.");
                    return;
                }

                shoppingCart.Clear();
                decimal total = 0;
                int itemNumber = 1;

                foreach (var cartItem in cartItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == cartItem.ProductId);
                    if (product != null)
                    {
                        for (int i = 0; i < cartItem.Quantity; i++)
                        {
                            shoppingCart.Add(product);
                        }

                        Console.WriteLine($"{itemNumber}. {product.Name} - {product.Price:C} x {cartItem.Quantity}");
                        total += product.Price * cartItem.Quantity;
                        itemNumber++;
                    }
                }

                Console.WriteLine($"\nОбщая сумма: {total:C}");

                Console.WriteLine("\n1. Купить все товары");
                Console.WriteLine("2. Купить отдельный товар");
                Console.WriteLine("3. Удалить товар из корзины");
                Console.WriteLine("4. Вернуться в главное меню");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        PurchaseAll();
                        break;
                    case "2":
                        PurchaseSingle();
                        break;
                    case "3":
                        RemoveFromCart();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }

            

            
            }


            

            

            

            

            
            }
        }
}
