
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMWOG_Marketplace
{
    // Класс для пользователя
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Order> Orders { get; set; }

        public User()
        {
            Orders = new List<Order>();
        }
    }

    // Класс для товара
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    // Класс для пункта выдачи заказов
    public class PickupPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    // Класс для заказа
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Product> Products { get; set; }
        public PickupPoint SelectedPickupPoint { get; set; }
        public decimal TotalAmount { get; set; }

        public Order()
        {
            Products = new List<Product>();
            OrderDate = DateTime.Now;
        }
    }

    // Главный класс приложения
    public class MarketplaceApp
    {
        private List<User> users;
        private List<Product> products;
        private List<PickupPoint> pickupPoints;
        private List<Order> allOrders;
        private User currentUser;
        private List<Product> shoppingCart;
        private int nextOrderId;

        public MarketplaceApp()
        {
            users = new List<User>();
            products = new List<Product>();
            pickupPoints = new List<PickupPoint>();
            allOrders = new List<Order>();
            shoppingCart = new List<Product>();
            nextOrderId = 1;
            InitializeData();
        }

        // Инициализация тестовых данных
        private void InitializeData()
        {
            // Добавляем товары
            products.AddRange(new[]
            {
                new Product { Id = 1, Name = "Смартфон", Description = "Флагманский смартфон", Price = 59999.99m, Stock = 10 },
                new Product { Id = 2, Name = "Ноутбук", Description = "Игровой ноутбук", Price = 89999.99m, Stock = 5 },
                new Product { Id = 3, Name = "Наушники", Description = "Беспроводные наушники", Price = 7999.99m, Stock = 20 },
                new Product { Id = 4, Name = "Часы", Description = "Умные часы", Price = 14999.99m, Stock = 15 },
                new Product { Id = 5, Name = "Планшет", Description = "Графический планшет", Price = 29999.99m, Stock = 8 }
            });

            // Добавляем пункты выдачи
            pickupPoints.AddRange(new[]
            {
                new PickupPoint { Id = 1, Name = "ПВЗ Центральный", Address = "ул. Центральная, 1" },
                new PickupPoint { Id = 2, Name = "ПВЗ Северный", Address = "ул. Северная, 25" },
                new PickupPoint { Id = 3, Name = "ПВЗ Южный", Address = "ул. Южная, 42" },
                new PickupPoint { Id = 4, Name = "ПВЗ Западный", Address = "ул. Западная, 15" }
            });

            // Добавляем тестового пользователя
            var testUser = new User { Username = "test", Password = "123" };
            users.Add(testUser);
        }

        // Главное меню
        public void Run()
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

                switch (choice)
                {
                    case "1":
                        ViewProducts();
                        break;
                    case "2":
                        Register();
                        break;
                    case "3":
                        Login();
                        break;
                    case "4":
                        ViewShoppingCart();
                        break;
                    case "5":
                        Console.WriteLine("Спасибо за использование GMWOGGG.MOWWONGG!");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        // Просмотр товаров
        private void ViewProducts()
        {
            Console.WriteLine("\n=== КАТАЛОГ ТОВАРОВ ===");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id}. {product.Name}");
                Console.WriteLine($"   Описание: {product.Description}");
                Console.WriteLine($"   Цена: {product.Price:C}");
                Console.WriteLine($"   В наличии: {product.Stock} шт.");
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
        private void AddToCart()
        {
            Console.Write("Введите ID товара для добавления в корзину: ");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                var product = products.FirstOrDefault(p => p.Id == productId);
                if (product != null && product.Stock > 0)
                {
                    shoppingCart.Add(product);
                    Console.WriteLine($"Товар '{product.Name}' добавлен в корзину!");
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
        private void ViewShoppingCart()
        {
            if (currentUser == null)
            {
                Console.WriteLine("Для просмотра корзины необходимо войти в аккаунт.");
                return;
            }

            Console.WriteLine("\n=== КОРЗИНА ===");

            if (shoppingCart.Count == 0)
            {
                Console.WriteLine("Корзина пуста.");
                return;
            }

            decimal total = 0;
            for (int i = 0; i < shoppingCart.Count; i++)
            {
                var product = shoppingCart[i];
                Console.WriteLine($"{i + 1}. {product.Name} - {product.Price:C}");
                total += product.Price;
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

        // Покупка всех товаров
        private void PurchaseAll()
        {
            if (shoppingCart.Count == 0)
            {
                Console.WriteLine("Корзина пуста.");
                return;
            }

            var pickupPoint = SelectPickupPoint();
            if (pickupPoint == null) return;

            var order = new Order
            {
                Id = nextOrderId++,
                SelectedPickupPoint = pickupPoint
            };

            decimal total = 0;
            foreach (var product in shoppingCart)
            {
                order.Products.Add(product);
                total += product.Price;
                // Уменьшаем количество на складе
                product.Stock--;
            }

            order.TotalAmount = total;
            currentUser.Orders.Add(order);
            allOrders.Add(order);

            Console.WriteLine($"\nЗаказ #{order.Id} успешно оформлен!");
            Console.WriteLine($"Сумма заказа: {total:C}");
            Console.WriteLine($"Пункт выдачи: {pickupPoint.Name}");
            Console.WriteLine($"Адрес: {pickupPoint.Address}");

            shoppingCart.Clear();
        }

        // Покупка одного товара
        private void PurchaseSingle()
        {
            if (shoppingCart.Count == 0)
            {
                Console.WriteLine("Корзина пуста.");
                return;
            }

            Console.Write("Введите номер товара для покупки: ");
            if (int.TryParse(Console.ReadLine(), out int itemNumber) && itemNumber >= 1 && itemNumber <= shoppingCart.Count)
            {
                var product = shoppingCart[itemNumber - 1];
                var pickupPoint = SelectPickupPoint();
                if (pickupPoint == null) return;

                var order = new Order
                {
                    Id = nextOrderId++,
                    SelectedPickupPoint = pickupPoint
                };

                order.Products.Add(product);
                order.TotalAmount = product.Price;
                currentUser.Orders.Add(order);
                allOrders.Add(order);

                // Уменьшаем количество на складе
                product.Stock--;

                Console.WriteLine($"\nЗаказ #{order.Id} успешно оформлен!");
                Console.WriteLine($"Товар: {product.Name}");
                Console.WriteLine($"Сумма: {product.Price:C}");
                Console.WriteLine($"Пункт выдачи: {pickupPoint.Name}");

                shoppingCart.RemoveAt(itemNumber - 1);
            }
            else
            {
                Console.WriteLine("Неверный номер товара.");
            }
        }

        // Удаление товара из корзины
        private void RemoveFromCart()
        {
            Console.Write("Введите номер товара для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int itemNumber) && itemNumber >= 1 && itemNumber <= shoppingCart.Count)
            {
                var product = shoppingCart[itemNumber - 1];
                shoppingCart.RemoveAt(itemNumber - 1);
                Console.WriteLine($"Товар '{product.Name}' удален из корзины.");
            }
            else
            {
                Console.WriteLine("Неверный номер товара.");
            }
        }

        // Выбор пункта выдачи
        private PickupPoint SelectPickupPoint()
        {
            Console.WriteLine("\n=== ВЫБОР ПУНКТА ВЫДАЧИ ===");
            foreach (var point in pickupPoints)
            {
                Console.WriteLine($"{point.Id}. {point.Name} - {point.Address}");
            }

            
            Console.Write("Выберите пункт выдачи: ");
            if (int.TryParse(Console.ReadLine(), out int pointId))
            {
                var selectedPoint = pickupPoints.FirstOrDefault(p => p.Id == pointId);
                if (selectedPoint != null)
                {
                    return selectedPoint;
                }
            }

            Console.WriteLine("Неверный выбор пункта выдачи.");
            return null;
        }

        // Регистрация пользователя
        private void Register()
        {
            Console.WriteLine("\n=== РЕГИСТРАЦИЯ ===");
            Console.Write("Введите имя пользователя: ");
            var username = Console.ReadLine();

            if (users.Any(u => u.Username == username))
            {
                Console.WriteLine("Пользователь с таким именем уже существует.");
                return;
            }

            Console.Write("Введите пароль: ");
            var password = Console.ReadLine();
            Console.Write("Подтвердите пароль: ");
            var confirmPassword = Console.ReadLine();

            if (password != confirmPassword)
            {
                Console.WriteLine("Пароли не совпадают.");
                return;
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Имя пользователя и пароль не могут быть пустыми.");
                return;
            }

            var newUser = new User { Username = username, Password = password };
            users.Add(newUser);
            Console.WriteLine("Регистрация успешно завершена! Теперь вы можете войти в аккаунт.");
        }

        // Вход в аккаунт
        private void Login()
        {
            Console.WriteLine("\n=== ВХОД В АККАУНТ ===");
            Console.Write("Имя пользователя: ");
            var username = Console.ReadLine();
            Console.Write("Пароль: ");
            var password = Console.ReadLine();

            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                currentUser = user;
                Console.WriteLine($"Добро пожаловать, {username}!");
                UserMenu();
            }
            else
            {
                Console.WriteLine("Неверное имя пользователя или пароль.");
            }
        }

        // Меню пользователя
        private void UserMenu()
        {
            while (currentUser != null)
            {
                Console.WriteLine("\n=== ЛИЧНЫЙ КАБИНЕТ ===");
                Console.WriteLine("1. Просмотр товаров");
                Console.WriteLine("2. Корзина");
                Console.WriteLine("3. Мои заказы");
                Console.WriteLine("4. Выйти из аккаунта");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewProducts();
                        break;
                    case "2":
                        ViewShoppingCart();
                        break;
                    case "3":
                        ViewOrders();
                        break;
                    case "4":
                        currentUser = null;
                        shoppingCart.Clear();
                        Console.WriteLine("Вы вышли из аккаунта.");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        // Просмотр заказов с сортировкой
        private void ViewOrders()
        {
            if (currentUser.Orders.Count == 0)
            {
                Console.WriteLine("У вас пока нет заказов.");
                return;
            }

            
            Console.WriteLine("\n=== МОИ ЗАКАЗЫ ===");
            Console.WriteLine("1. Сортировка по дате (сначала новые)");
            Console.WriteLine("2. Сортировка по дате (сначала старые)");
            Console.Write("Выберите тип сортировки: ");

            var sortedOrders = currentUser.Orders.ToList();

            var sortChoice = Console.ReadLine();
            if (sortChoice == "2")
            {
                sortedOrders = sortedOrders.OrderBy(o => o.OrderDate).ToList();
            }
            else
            {
                sortedOrders = sortedOrders.OrderByDescending(o => o.OrderDate).ToList();
            }

            foreach (var order in sortedOrders)
            {
                Console.WriteLine($"\nЗаказ #{order.Id}");
                Console.WriteLine($"Дата: {order.OrderDate:dd.MM.yyyy HH:mm}");
                Console.WriteLine($"Пункт выдачи: {order.SelectedPickupPoint.Name}");
                Console.WriteLine($"Адрес: {order.SelectedPickupPoint.Address}");
                Console.WriteLine("Товары:");

                foreach (var product in order.Products)
                {
                    Console.WriteLine($"  - {product.Name} - {product.Price:C}");
                }

                Console.WriteLine($"Общая сумма: {order.TotalAmount:C}");
                Console.WriteLine(new string('-', 40));
            }
        }
    }

    // Главный класс программы
    class Program
    {
        static void Main(string[] args)
        {
            var app = new MarketplaceApp();
            app.Run();
        }
    }
}