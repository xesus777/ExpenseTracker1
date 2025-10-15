
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
        private static int nextStudentId = 1;

        public int StudentId { get; private set; }
        public List<Course> Courses { get; private set; }
        public Dictionary<Course, List<double>> Grades { get; private set; }
        public double AverageGrade { get; private set; }

        public Student(string name, int age, string contactInfo) : base(name, age, contactInfo)
        {
            StudentId = nextStudentId++;
            Courses = new List<Course>();
            Grades = new Dictionary<Course, List<double>>();
            AverageGrade = 0.0;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Студент ID: {StudentId}");
            Console.WriteLine($"Имя: {Name}");
            Console.WriteLine($"Возраст: {Age}");
            Console.WriteLine($"Контактная информация: {ContactInfo}");
            Console.WriteLine($"Средний балл: {AverageGrade:F2}");
            Console.WriteLine($"Количество курсов: {Courses.Count}");
        }

        public bool EnrollInCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course), "Курс не может быть null");

            if (Courses.Contains(course))
            {
                Console.WriteLine("Студент уже записан на этот курс");
                return false;
            }

            if (course.AddStudent(this))
            {
                Courses.Add(course);
                Grades[course] = new List<double>();
                Console.WriteLine($"Студент {Name} успешно записан на курс {course.CourseName}");
                return true;
            }

            return false;
        }

        public bool LeaveCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course), "Курс не может быть null");

            if (!Courses.Contains(course))
            {
                Console.WriteLine("Студент не записан на этот курс");
                return false;
            }

            if (course.RemoveStudent(this))
            {
                Courses.Remove(course);
                Grades.Remove(course);
                CalculateAverageGrade();
                Console.WriteLine($"Студент {Name} успешно отчислен с курса {course.CourseName}");
                return true;
            }

            return false;
        }

        
        public void ViewCourses()
        {
            if (Courses.Count == 0)
            {
                Console.WriteLine("Студент не записан ни на один курс");
                return;
            }

            Console.WriteLine($"Курсы студента {Name}:");
            foreach (var course in Courses)
            {
                Console.WriteLine($"- {course.CourseName} (Преподаватель: {course.Instructor?.Name ?? "Не назначен"})");
            }
        }

        public void AddGrade(Course course, double grade)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course), "Курс не может быть null");

            if (grade < 0 || grade > 100)
                throw new ArgumentException("Оценка должна быть от 0 до 100");

            if (!Courses.Contains(course))
                throw new InvalidOperationException("Студент не записан на этот курс");

            if (!Grades.ContainsKey(course))
                Grades[course] = new List<double>();

            Grades[course].Add(grade);
            CalculateAverageGrade();
        }

        public void CalculateAverageGrade()
        {
            if (Grades.Count == 0)
            {
                AverageGrade = 0;
                return;
            }

            double total = 0;
            int count = 0;

            foreach (var courseGrades in Grades.Values)
            {
                total += courseGrades.Sum();
                count += courseGrades.Count;
            }

            AverageGrade = count > 0 ? total / count : 0;
        }

        public void ViewGrades()
        {
            if (Grades.Count == 0)
            {
                Console.WriteLine("Нет оценок");
                return;
            }

            Console.WriteLine($"Оценки студента {Name}:");
            foreach (var (course, courseGrades) in Grades)
            {
                double courseAverage = courseGrades.Count > 0 ? courseGrades.Average() : 0;
                Console.WriteLine($"- {course.CourseName}: {string.Join(", ", courseGrades)} (Среднее: {courseAverage:F2})");
            }
            Console.WriteLine($"Общий средний балл: {AverageGrade:F2}");
        }
    }

    public class Teacher : Person
    {
        private static int nextTeacherId = 1;

        public int TeacherId { get; private set; }
        public Department Department { get; private set; }
        public List<Course> TeachingCourses { get; private set; }

        public Teacher(string name, int age, string contactInfo, Department department)
            : base(name, age, contactInfo)
        {
            TeacherId = nextTeacherId++;
            Department = department;
            TeachingCourses = new List<Course>();
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Преподаватель ID: {TeacherId}");
            Console.WriteLine($"Имя: {Name}");
            Console.WriteLine($"Возраст: {Age}");
            Console.WriteLine($"Контактная информация: {ContactInfo}");
            Console.WriteLine($"Отдел: {Department}");
            Console.WriteLine($"Количество преподаваемых курсов: {TeachingCourses.Count}");
        }

        public void AddCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course), "Курс не может быть null");

            if (TeachingCourses.Contains(course))
            {
                Console.WriteLine("Преподаватель уже ведет этот курс");
                return;
            }

            course.Instructor = this;
            TeachingCourses.Add(course);
            Console.WriteLine($"Преподаватель {Name} назначен на курс {course.CourseName}");
        }

        public void RemoveCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course), "Курс не может быть null");

            
            if (!TeachingCourses.Contains(course))
            {
                Console.WriteLine("Преподаватель не ведет этот курс");
                return;
            }

            if (course.Instructor == this)
                course.Instructor = null;

            TeachingCourses.Remove(course);
            Console.WriteLine($"Преподаватель {Name} удален с курса {course.CourseName}");
        }

        public void GradeStudent(Student student, Course course, double grade)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student), "Студент не может быть null");

            if (course == null)
                throw new ArgumentNullException(nameof(course), "Курс не может быть null");

            if (!TeachingCourses.Contains(course))
                throw new InvalidOperationException("Преподаватель не ведет этот курс");

            if (!course.EnrolledStudents.Contains(student))
                throw new InvalidOperationException("Студент не записан на этот курс");

            student.AddGrade(course, grade);
            Console.WriteLine($"Оценка {grade} выставлена студенту {student.Name} по курсу {course.CourseName}");
        }

        public void ViewTeachingCourses()
        {
            if (TeachingCourses.Count == 0)
            {
                Console.WriteLine("Преподаватель не ведет ни одного курса");
                return;
            }

            Console.WriteLine($"Курсы преподавателя {Name}:");
            foreach (var course in TeachingCourses)
            {
                Console.WriteLine($"- {course.CourseName} (Студентов: {course.EnrolledStudents.Count}/{course.MaxStudents})");
            }
        }
    }

    
    public class Course
    {
        private static int nextCourseId = 1;
        private string courseName;
        private string description;
        private int maxStudents;

        public int CourseId { get; private set; }
        public string CourseName
        {
            get => courseName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название курса не может быть пустым");
                courseName = value;
            }
        }

        public string Description
        {
            get => description;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Описание курса не может быть пустым");
                description = value;
            }
        }

        public Teacher Instructor { get; set; }
        public List<Student> EnrolledStudents { get; private set; }
        public int MaxStudents
        {
            get => maxStudents;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Максимальное количество студентов должно быть положительным");
                maxStudents = value;
            }
        }

        public Course(string courseName, string description, int maxStudents)
        {
            CourseId = nextCourseId++;
            CourseName = courseName;
            Description = description;
            MaxStudents = maxStudents;
            EnrolledStudents = new List<Student>();
            Instructor = null;
        }

        public void DisplayCourseInfo()
        {
            Console.WriteLine($"Курс ID: {CourseId}");
            Console.WriteLine($"Название: {CourseName}");
            Console.WriteLine($"Описание: {Description}");
            Console.WriteLine($"Преподаватель: {Instructor?.Name ?? "Не назначен"}");
            Console.WriteLine($"Студентов: {EnrolledStudents.Count}/{MaxStudents}");
            Console.WriteLine($"Заполненность: {(double)EnrolledStudents.Count / MaxStudents * 100:F1}%");
        }

        public bool AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student), "Студент не может быть null");

            
            if (EnrolledStudents.Contains(student))
            {
                Console.WriteLine("Студент уже записан на этот курс");
                return false;
            }

            if (IsFull())
            {
                Console.WriteLine("Курс переполнен, невозможно добавить студента");
                return false;
            }

            EnrolledStudents.Add(student);
            return true;
        }

        public bool RemoveStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student), "Студент не может быть null");

            if (!EnrolledStudents.Contains(student))
            {
                Console.WriteLine("Студент не записан на этот курс");
                return false;
            }

            EnrolledStudents.Remove(student);
            return true;
        }

        public void ViewEnrolledStudents()
        {
            if (EnrolledStudents.Count == 0)
            {
                Console.WriteLine("На курс не записан ни один студент");
                return;
            }

            Console.WriteLine($"Студенты курса {CourseName}:");
            foreach (var student in EnrolledStudents)
            {
                double courseAverage = 0;
                if (student.Grades.ContainsKey(this))
                {
                    var grades = student.Grades[this];
                    courseAverage = grades.Count > 0 ? grades.Average() : 0;
                }
                Console.WriteLine($"- {student.Name} (ID: {student.StudentId}, Средний балл: {courseAverage:F2})");
            }
        }

        public bool IsFull()
        {
            return EnrolledStudents.Count >= MaxStudents;
        }

        public double GetCourseAverageGrade()
        {
            if (EnrolledStudents.Count == 0)
                return 0;

            double total = 0;
            int count = 0;

            foreach (var student in EnrolledStudents)
            {
                if (student.Grades.ContainsKey(this) && student.Grades[this].Count > 0)
                {
                    total += student.Grades[this].Average();
                    count++;
                }
            }

            return count > 0 ? total / count : 0;
        }
    }

    public class University
    {
        
    }

    
    public class MenuManager
    {
        
    }

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