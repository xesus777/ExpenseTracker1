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

        
        
    }


    
}