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
            
        }
    }

    
}