using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreManagement
{
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Food,
        Books,
        Sports
    }

    public class Product
    {
        public string Code { get; private set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InStock => Quantity > 0;
        public ProductCategory Category { get; set; }

        public Product(string name, decimal price, int quantity, ProductCategory category)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название товара не может быть пустым");
            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной");
            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным");

            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        public void SetCode(string code)
        {
            Code = code;
        }

        public override string ToString()
        {
            return $"Код: {Code}, Название: {Name}, Цена: {Price:C}, Количество: {Quantity}, " +
                   $"В наличии: {(InStock ? "Да" : "Нет")}, Категория: {Category}";
        }
    }
    public class Sale
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime SaleDate { get; set; }

        public Sale(string productCode, string productName, int quantity, decimal totalAmount)
        {
            ProductCode = productCode;
            ProductName = productName;
            Quantity = quantity;
            TotalAmount = totalAmount;
            SaleDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Дата: {SaleDate:yyyy-MM-dd HH:mm}, Код: {ProductCode}, Товар: {ProductName}, " +
                   $"Количество: {Quantity}, Сумма: {TotalAmount:C}";
        }
    }
    public class Store
    {
        private List<Product> products;
        private Stack<Sale> salesHistory;
        private List<Sale> salesReport;
        private int productCounter;

        public Store()
        {
            products = new List<Product>();
            salesHistory = new Stack<Sale>();
            salesReport = new List<Sale>();
            productCounter = 1;
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            AddProduct(new Product("Ноутбук HP", 45000m, 10, ProductCategory.Electronics));
            AddProduct(new Product("Футболка", 1500m, 50, ProductCategory.Clothing));
            AddProduct(new Product("Яблоки", 120m, 100, ProductCategory.Food));
            AddProduct(new Product("Война и мир", 800m, 25, ProductCategory.Books));
            AddProduct(new Product("Футбольный мяч", 2500m, 15, ProductCategory.Sports));
        }

        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Товар не может быть null");

            string code = "1" + productCounter.ToString("D5");
            product.SetCode(code);
            products.Add(product);
            productCounter++;

            Console.WriteLine($"Товар добавлен: {product.Name} (Код: {product.Code})");
        }

        public bool RemoveProduct(string code)
        {
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                products.Remove(product);
                Console.WriteLine($"Товар удален: {product.Name} (Код: {product.Code})");
                return true;
            }

            Console.WriteLine($"Товар с кодом {code} не найден");
            return false;
        }

        public void OrderSupply(string code, int quantity)
        {
            if (quantity <= 0)
            {
                Console.WriteLine("Количество поставки должно быть положительным");
                return;
            }

            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                product.Quantity += quantity;
                Console.WriteLine($"Поставка выполнена: {product.Name} (+{quantity} шт.)");
            }
            else
            {
                Console.WriteLine($"Товар с кодом {code} не найден");
            }
        }

        public bool SellProduct(string code, int quantity)
        {
            if (quantity <= 0)
            {
                Console.WriteLine("Количество продажи должно быть положительным");
                return false;
            }

            var product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine($"Товар с кодом {code} не найден");
                return false;
            }

            if (product.Quantity < quantity)
            {
                Console.WriteLine($"Недостаточно товара на складе. Доступно: {product.Quantity}");
                return false;
            }

            product.Quantity -= quantity;
            decimal totalAmount = product.Price * quantity;

            var sale = new Sale(product.Code, product.Name, quantity, totalAmount);
            salesHistory.Push(sale);
            salesReport.Add(sale);

            Console.WriteLine($"Продажа выполнена: {product.Name} ({quantity} шт.) на сумму {totalAmount:C}");
            return true;
        }

        public void SearchByCode(string code)
        {
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                Console.WriteLine("Найден товар:");
                Console.WriteLine(product);
            }
            else
            {
                Console.WriteLine($"Товар с кодом {code} не найден");
            }
        }

        public void SearchByName(string name)
        {
            var foundProducts = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (foundProducts.Any())
            {
                Console.WriteLine($"Найдено товаров: {foundProducts.Count}");
                foreach (var product in foundProducts)
                {
                    Console.WriteLine(product);
                    Console.WriteLine("---");
                }
            }
            else
            {
                Console.WriteLine($"Товары с названием '{name}' не найдены");
            }
        }

        public void SearchByCategory(ProductCategory category)
        {
            var foundProducts = products.Where(p => p.Category == category).ToList();

            if (foundProducts.Any())
            {
                Console.WriteLine($"Товары в категории '{category}': {foundProducts.Count}");
                foreach (var product in foundProducts)
                {
                    Console.WriteLine(product);
                    Console.WriteLine("---");
                }
            }
            else
            {
                Console.WriteLine($"Товары в категории '{category}' не найдены");
            }
        }

        public void DisplayAllProducts()
        {
            if (!products.Any())
            {
                Console.WriteLine("В магазине нет товаров");
                return;
            }
            Console.WriteLine($"Все товары ({products.Count}):");
            Console.WriteLine("==========================================");

            foreach (var product in products)
            {
                Console.WriteLine(product);
                Console.WriteLine("------------------------------------------");
            }
        }

        public void ShowSalesHistory()
        {
            if (!salesHistory.Any())
            {
                Console.WriteLine("История продаж пуста");
                return;
            }

            Console.WriteLine("История продаж (последние продажи):");
            Console.WriteLine("==========================================");

            foreach (var sale in salesHistory.Reverse())
            {
                Console.WriteLine(sale);
                Console.WriteLine("------------------------------------------");
            }
        }

        public bool UndoLastSale()
        {
            if (!salesHistory.Any())
            {
                Console.WriteLine("Нет продаж для отмены");
                return false;
            }

            var lastSale = salesHistory.Pop();
            var product = products.FirstOrDefault(p => p.Code == lastSale.ProductCode);

            if (product != null)
            {
                product.Quantity += lastSale.Quantity;
                salesReport.Remove(lastSale);
                Console.WriteLine($"Отменена продажа: {lastSale.ProductName} ({lastSale.Quantity} шт.)");
                return true;
            }

            Console.WriteLine("Не удалось найти товар для отмены продажи");
            return false;
        }

        public void ShowSalesReport()
        {
            if (!salesReport.Any())
            {
                Console.WriteLine("Отчет о продажах пуст");
                return;
            }

            decimal totalRevenue = salesReport.Sum(s => s.TotalAmount);
            int totalItemsSold = salesReport.Sum(s => s.Quantity);

            Console.WriteLine("ОТЧЕТ О ПРОДАЖАХ");
            Console.WriteLine("==========================================");
            Console.WriteLine($"Всего продаж: {salesReport.Count}");
            Console.WriteLine($"Общее количество проданных товаров: {totalItemsSold} шт.");
            Console.WriteLine($"Общая выручка: {totalRevenue:C}");
            Console.WriteLine("==========================================");

            foreach (var sale in salesReport)
            {
                Console.WriteLine(sale);
                Console.WriteLine("------------------------------------------");
            }
        }
    }
    class Program
    {
        static Store store = new Store();

        static void Main(string[] args)
        {
            Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ МАГАЗИНОМ ===");

            while (true)
            {
                ShowMainMenu();
                var choice = GetUserChoice(1, 10);

                switch (choice)
                {
                    case 1:
                        AddProductMenu();
                        break;
                    case 2:
                        RemoveProductMenu();
                        break;
                    case 3:
                        OrderSupplyMenu();
                        break;
                    case 4:
                        SellProductMenu();
                        break;
                    case 5:
                        SearchMenu();
                        break;
                    case 6:
                        store.DisplayAllProducts();
                        break;
                    case 7:
                        store.ShowSalesHistory();
                        break;
                    case 8:
                        store.UndoLastSale();
                        break;
                    case 9:
                        store.ShowSalesReport();
                        break;
                    case 10:
                        Console.WriteLine("Выход из системы...");
                        return;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("\nГЛАВНОЕ МЕНЮ:");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");
            Console.WriteLine("3. Заказать поставку");
            Console.WriteLine("4. Продать товар");
            Console.WriteLine("5. Поиск товаров");
            Console.WriteLine("6. Показать все товары");
            Console.WriteLine("7. История продаж");
            Console.WriteLine("8. Отменить последнюю продажу");
            Console.WriteLine("9. Отчет о продажах");
            Console.WriteLine("10. Выход");
            Console.Write("Выберите действие (1-10): ");
        }

        static int GetUserChoice(int min, int max)
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                Console.Write($"Пожалуйста, введите число от {min} до {max}: ");
            }
        }

        static void AddProductMenu()
        {
            Console.WriteLine("\nДОБАВЛЕНИЕ ТОВАРА:");

            try
            {
                Console.Write("Введите название товара: ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Название не может быть пустым!");
                    return;
                }

                Console.Write("Введите цену: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
                {
                    Console.WriteLine("Неверная цена!");
                    return;
                }

                Console.Write("Введите количество: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
                {
                    Console.WriteLine("Неверное количество!");
                    return;
                }

                Console.WriteLine("Выберите категорию:");
                var categories = Enum.GetValues(typeof(ProductCategory));
                for (int i = 0; i < categories.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories.GetValue(i)}");
                }

                Console.Write("Введите номер категории: ");
                if (!int.TryParse(Console.ReadLine(), out int categoryIndex) ||
                    categoryIndex < 1 || categoryIndex > categories.Length)
                {
                    Console.WriteLine("Неверный номер категории!");
                    return;
                }

                ProductCategory category = (ProductCategory)(categoryIndex - 1);
                Product product = new Product(name, price, quantity, category);
                store.AddProduct(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void RemoveProductMenu()
        {
            Console.WriteLine("\nУДАЛЕНИЕ ТОВАРА:");
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine();
            store.RemoveProduct(code);
        }
        static void OrderSupplyMenu()
        {
            Console.WriteLine("\nЗАКАЗ ПОСТАВКИ:");
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine();

            Console.Write("Введите количество для поставки: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Неверное количество!");
                return;
            }

            store.OrderSupply(code, quantity);
        }

        static void SellProductMenu()
        {
            Console.WriteLine("\nПРОДАЖА ТОВАРА:");
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine();

            Console.Write("Введите количество для продажи: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Неверное количество!");
                return;
            }

            store.SellProduct(code, quantity);
        }

        static void SearchMenu()
        {
            Console.WriteLine("\nПОИСК ТОВАРОВ:");
            Console.WriteLine("1. По коду");
            Console.WriteLine("2. По названию");
            Console.WriteLine("3. По категории");
            Console.Write("Выберите тип поиска (1-3): ");

            var choice = GetUserChoice(1, 3);

            switch (choice)
            {
                case 1:
                    Console.Write("Введите код товара: ");
                    string code = Console.ReadLine();
                    store.SearchByCode(code);
                    break;
                case 2:
                    Console.Write("Введите название товара: ");
                    string name = Console.ReadLine();
                    store.SearchByName(name);
                    break;
                case 3:
                    Console.WriteLine("Выберите категорию:");
                    var categories = Enum.GetValues(typeof(ProductCategory));
                    for (int i = 0; i < categories.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {categories.GetValue(i)}");
                    }
                    Console.Write("Введите номер категории: ");
                    if (int.TryParse(Console.ReadLine(), out int categoryIndex) &&
                        categoryIndex >= 1 && categoryIndex <= categories.Length)
                    {
                        ProductCategory category = (ProductCategory)(categoryIndex - 1);
                        store.SearchByCategory(category);
                    }
                    else
                    {
                        Console.WriteLine("Неверный номер категории!");
                    }
                    break;
            }
        }
    }
}


