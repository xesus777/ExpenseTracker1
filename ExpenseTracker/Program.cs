
using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityManagementSystem
{
    
    public enum Department
    {
        ComputerScience,
        Mathematics,
        Physics,
        Engineering,
        Business,
        Arts
    }

    // Базовый класс для всех людей 
    public abstract class Person
    {
        private string name;
        private int age;
        private string contactInfo;

        public string Name
        {
            get => name;
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Имя не может быть пустым");
                name = value;
            }
        }

        public int Age
        {
            get => age;
            protected set
            {
                if (value <= 0 || value > 120)
                    throw new ArgumentException("Возраст должен быть от 1 до 120 лет");
                age = value;
            }
        }

        public string ContactInfo
        {
            get => contactInfo;
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Контактная информация не может быть пустой");
                contactInfo = value;
            }
        }

        protected Person(string name, int age, string contactInfo)
        {
            Name = name;
            Age = age;
            ContactInfo = contactInfo;
        }

        public abstract void DisplayInfo();
    }

    
    public class Student : Person
    {
        
    }

    
    public class Teacher : Person
    {
        
    }

    
    public class Course
    {
        
    }

    
    public class University
    {
        
    }

    
    public class MenuManager
    {
        
    }

    // Главный класс программы
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                Console.WriteLine("Запуск системы управления университетом...");

                var menuManager = new MenuManager();
                menuManager.DisplayMainMenu();

                Console.WriteLine("Система завершена. До свидания!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}