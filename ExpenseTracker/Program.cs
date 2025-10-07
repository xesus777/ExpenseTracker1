using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement
{
    
    public enum Genre
    {
        Fiction,
        Science,
        History,
        Fantasy,
        Mystery
    }

    
    public class Book
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        
        public Book(int id, string title, string author, Genre genre, int year, decimal price)
        {
            Id = id;
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            Price = price;
        }

        
        public void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Название: {Title}");
            Console.WriteLine($"Автор: {Author}");
            Console.WriteLine($"Жанр: {Genre}");
            Console.WriteLine($"Год издания: {Year}");
            Console.WriteLine($"Цена: {Price:C}");
            Console.WriteLine(new string('-', 40));
        }
    }

   
    public class Library
    {
        private List<Book> books;
        private int nextId;

        public Library()
        {
            books = new List<Book>();
            nextId = 1;
        }

        
        public List<Book> GetAllBooks()
        {
            return books;
        }

        
        public void AddBook(string title, string author, Genre genre, int year, decimal price)
        {
            var book = new Book(nextId, title, author, genre, year, price);
            books.Add(book);
            nextId++;
            Console.WriteLine($"Книга успешно добавлена с ID: {book.Id}");
        }

        public bool RemoveBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
                Console.WriteLine($"Книга с ID {id} успешно удалена.");
                return true;
            }
            else
            {
                Console.WriteLine($"Книга с ID {id} не найдена.");
                return false;
            }
        }

        
        public List<Book> FindBooksByTitle(string title)
        {
            return books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Book> FindBooksByAuthor(string author)
        {
            return books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Book> FindBooksByGenre(Genre genre)
        {
            return books.Where(b => b.Genre == genre).ToList();
        }

        
        public List<Book> SortBooksByTitle()
        {
            return books.OrderBy(b => b.Title).ToList();
        }

        public List<Book> SortBooksByYear()
        {
            return books.OrderBy(b => b.Year).ToList();
        }

        public List<Book> SortBooksByYearDescending()
        {
            return books.OrderByDescending(b => b.Year).ToList();
        }

        
        public Book GetMostExpensiveBook()
        {
            return books.OrderByDescending(b => b.Price).FirstOrDefault();
        }

        public Book GetCheapestBook()
        {
            return books.OrderBy(b => b.Price).FirstOrDefault();
        }

        
        public Dictionary<string, int> GroupBooksByAuthor()
        {
            return books.GroupBy(b => b.Author)
                       .ToDictionary(g => g.Key, g => g.Count());
        }

        
        public void AddTestData()
        {
            AddBook("Война и мир", "Лев Толстой", Genre.Fiction, 1869, 1200.50m);
            AddBook("Преступление и наказание", "Фёдор Достоевский", Genre.Fiction, 1866, 950.75m);
            AddBook("1984", "Джордж Оруэлл", Genre.Fiction, 1949, 800.00m);
            AddBook("Краткая история времени", "Стивен Хокинг", Genre.Science, 1988, 1500.00m);
            AddBook("Сто лет одиночества", "Габриэль Гарсиа Маркес", Genre.Fantasy, 1967, 1100.25m);
            AddBook("Мастер и Маргарита", "Михаил Булгаков", Genre.Fantasy, 1967, 1300.00m);
            AddBook("Анна Каренина", "Лев Толстой", Genre.Fiction, 1877, 1050.00m);
        }

        
        public void DisplayAllBooks()
        {
            if (!books.Any())
            {
                Console.WriteLine("В библиотеке нет книг.");
                return;
            }

            Console.WriteLine($"\nВсего книг в библиотеке: {books.Count}");
            Console.WriteLine(new string('=', 50));
            foreach (var book in books)
            {
                book.DisplayInfo();
            }
        }

        
        public void DisplayBooks(List<Book> booksToDisplay)
        {
            if (!booksToDisplay.Any())
            {
                Console.WriteLine("Книги не найдены.");
                return;
            }

            Console.WriteLine($"\nНайдено книг: {booksToDisplay.Count}");
            Console.WriteLine(new string('=', 50));
            foreach (var book in booksToDisplay)
            {
                book.DisplayInfo();
            }
        }
    }


    public class Menu
    {
        private Library library;

        public Menu()
        {
            library = new Library();
            library.AddTestData();
        }

        public void DisplayMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА УЧЁТА БИБЛИОТЕКИ ===");
                Console.WriteLine("1. Показать все книги");
                Console.WriteLine("2. Добавить книгу");
                Console.WriteLine("3. Удалить книгу");
                Console.WriteLine("4. Найти книги");
                Console.WriteLine("5. Сортировать книги");
                Console.WriteLine("6. Самая дорогая/дешёвая книга");
                Console.WriteLine("7. Группировка по авторам");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        DisplayAllBooks();
                        break;
                    case "2":
                        AddBookMenu();
                        break;
                    case "3":
                        RemoveBookMenu();
                        break;
                    case "4":
                        FindBooksMenu();
                        break;
                    case "5":
                        SortBooksMenu();
                        break;
                    case "6":
                        ShowPriceExtremesMenu();
                        break;
                    case "7":
                        ShowAuthorsGroupMenu();
                        break;
                    case "0":
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void DisplayAllBooks()
        {
            Console.Clear();
            Console.WriteLine("=== ВСЕ КНИГИ В БИБЛИОТЕКЕ ===");
            library.DisplayAllBooks();
            WaitForUser();
        }

        public void AddBookMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ НОВОЙ КНИГИ ===");

            string title = GetValidatedStringInput("Введите название книги: ");
            string author = GetValidatedStringInput("Введите автора книги: ");
            Genre genre = GetValidatedGenreInput("Введите жанр книги: ");
            int year = GetValidatedYearInput("Введите год издания: ");
            decimal price = GetValidatedPriceInput("Введите цену книги: ");

            library.AddBook(title, author, genre, year, price);
            WaitForUser();
        }

        public void RemoveBookMenu()
        {
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ КНИГИ ===");
            library.DisplayAllBooks();

            if (library.GetAllBooks().Any())
            {
                int id = GetValidatedIdInput("Введите ID книги для удаления: ");
                library.RemoveBook(id);
            }
            WaitForUser();
        }

        public void FindBooksMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ПОИСК КНИГ ===");
            Console.WriteLine("1. Поиск по названию");
            Console.WriteLine("2. Поиск по автору");
            Console.WriteLine("3. Поиск по жанру");
            Console.Write("Выберите тип поиска: ");

            var choice = Console.ReadLine();
            List<Book> foundBooks = new List<Book>();

            switch (choice)
            {
                case "1":
                    string title = GetValidatedStringInput("Введите название для поиска: ");
                    foundBooks = library.FindBooksByTitle(title);
                    break;
                case "2":
                    string author = GetValidatedStringInput("Введите автора для поиска: ");
                    foundBooks = library.FindBooksByAuthor(author);
                    break;
                case "3":
                    Genre genre = GetValidatedGenreInput("Введите жанр для поиска: ");
                    foundBooks = library.FindBooksByGenre(genre);
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    WaitForUser();
                    return;
            }

            Console.WriteLine("\n=== РЕЗУЛЬТАТЫ ПОИСКА ===");
            library.DisplayBooks(foundBooks);
            WaitForUser();
        }

        public void SortBooksMenu()
        {
            Console.Clear();
            Console.WriteLine("=== СОРТИРОВКА КНИГ ===");
            Console.WriteLine("1. Сортировка по названию (А-Я)");
            Console.WriteLine("2. Сортировка по году издания (по возрастанию)");
            Console.WriteLine("3. Сортировка по году издания (по убыванию)");
            Console.Write("Выберите тип сортировки: ");

            var choice = Console.ReadLine();
            List<Book> sortedBooks = new List<Book>();

            switch (choice)
            {
                case "1":
                    sortedBooks = library.SortBooksByTitle();
                    Console.WriteLine("\n=== КНИГИ, ОТСОРТИРОВАННЫЕ ПО НАЗВАНИЮ ===");
                    break;
                case "2":
                    sortedBooks = library.SortBooksByYear();
                    Console.WriteLine("\n=== КНИГИ, ОТСОРТИРОВАННЫЕ ПО ГОДУ (ПО ВОЗРАСТАНИЮ) ===");
                    break;
                case "3":
                    sortedBooks = library.SortBooksByYearDescending();
                    Console.WriteLine("\n=== КНИГИ, ОТСОРТИРОВАННЫЕ ПО ГОДУ (ПО УБЫВАНИЮ) ===");
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    WaitForUser();
                    return;
            }

            library.DisplayBooks(sortedBooks);
            WaitForUser();
        }

        public void ShowPriceExtremesMenu()
        {
            Console.Clear();
            Console.WriteLine("=== САМАЯ ДОРОГАЯ И САМАЯ ДЕШЁВАЯ КНИГИ ===");

            var mostExpensive = library.GetMostExpensiveBook();
            var cheapest = library.GetCheapestBook();

            if (mostExpensive != null && cheapest != null)
            {
                Console.WriteLine("\n=== САМАЯ ДОРОГАЯ КНИГА ===");
                mostExpensive.DisplayInfo();

                Console.WriteLine("\n=== САМАЯ ДЕШЁВАЯ КНИГА ===");
                cheapest.DisplayInfo();
            }
            else
            {
                Console.WriteLine("В библиотеке нет книг.");
            }

            WaitForUser();
        }

        public void ShowAuthorsGroupMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ГРУППИРОВКА КНИГ ПО АВТОРАМ ===");

            var authorsGroup = library.GroupBooksByAuthor();

            if (authorsGroup.Any())
            {
                Console.WriteLine("\nКоличество книг по авторам:");
                Console.WriteLine(new string('-', 30));
                foreach (var author in authorsGroup.OrderByDescending(a => a.Value))
                {
                    Console.WriteLine($"{author.Key}: {author.Value} книг(и)");
                }
            }
            else
            {
                Console.WriteLine("В библиотеке нет книг.");
            }

            WaitForUser();
        }

        
        private string GetValidatedStringInput(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Ошибка: поле не может быть пустым. Попробуйте снова.");
                }
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }

        private int GetValidatedYearInput(string prompt)
        {
            int year;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out year))
                {
                    int currentYear = DateTime.Now.Year;
                    if (year >= 1000 && year <= currentYear)
                    {
                        return year;
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: год должен быть между 1000 и {currentYear}. Попробуйте снова.");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: введите корректный год. Попробуйте снова.");
                }
            }
        }

        private decimal GetValidatedPriceInput(string prompt)
        {
            decimal price;
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out price))
                {
                    if (price >= 0)
                    {
                        return price;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: цена не может быть отрицательной. Попробуйте снова.");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: введите корректную цену. Попробуйте снова.");
                }
            }
        }

        private Genre GetValidatedGenreInput(string prompt)
        {
            Console.WriteLine(prompt);
            Console.WriteLine("Доступные жанры:");
            var genres = Enum.GetValues(typeof(Genre));
            for (int i = 0; i < genres.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {genres.GetValue(i)}");
            }

            while (true)
            {
                Console.Write("Выберите номер жанра: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= genres.Length)
                {
                    return (Genre)(choice - 1);
                }
                else
                {
                    Console.WriteLine($"Ошибка: введите число от 1 до {genres.Length}. Попробуйте снова.");
                }
            }
        }

        private int GetValidatedIdInput(string prompt)
        {
            int id;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out id) && id > 0)
                {
                    return id;
                }
                else
                {
                    Console.WriteLine("Ошибка: введите корректный положительный ID. Попробуйте снова.");
                }
            }
        }

        
    }



    
}