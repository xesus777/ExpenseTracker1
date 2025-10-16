
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

    public class Person
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

        public Person(string name, int age, string contactInfo)
        {
            Name = name;
            Age = age;
            ContactInfo = contactInfo;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Имя: {Name}");
            Console.WriteLine($"Возраст: {Age}");
            Console.WriteLine($"Контактная информация: {ContactInfo}");
        }
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
            base.DisplayInfo();
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
            base.DisplayInfo();
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
        public List<Student> Students { get; private set; }
        public List<Teacher> Teachers { get; private set; }
        public List<Course> Courses { get; private set; }

        public University()
        {
            Students = new List<Student>();
            Teachers = new List<Teacher>();
            Courses = new List<Course>();
        }

        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student), "Студент не может быть null");

            if (Students.Any(s => s.StudentId == student.StudentId))
                throw new InvalidOperationException("Студент с таким ID уже существует");

            Students.Add(student);
            Console.WriteLine($"Студент {student.Name} добавлен в университет");
        }

        public void AddTeacher(Teacher teacher)
        {
            if (teacher == null)
                throw new ArgumentNullException(nameof(teacher), "Преподаватель не может быть null");

            if (Teachers.Any(t => t.TeacherId == teacher.TeacherId))
                throw new InvalidOperationException("Преподаватель с таким ID уже существует");

            Teachers.Add(teacher);
            Console.WriteLine($"Преподаватель {teacher.Name} добавлен в университет");
        }

        public void AddCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course), "Курс не может быть null");


        if (Courses.Any(c => c.CourseId == course.CourseId))
                throw new InvalidOperationException("Курс с таким ID уже существует");

            Courses.Add(course);
            Console.WriteLine($"Курс {course.CourseName} добавлен в университет");
        }

        public void DisplayAllStudents()
        {
            if (Students.Count == 0)
            {
                Console.WriteLine("В университете нет студентов");
                return;
            }

            Console.WriteLine("Все студенты университета:");
            foreach (var student in Students.OrderBy(s => s.StudentId))
            {
                student.DisplayInfo();
                Console.WriteLine("---");
            }
        }

        public void DisplayAllTeachers()
        {
            if (Teachers.Count == 0)
            {
                Console.WriteLine("В университете нет преподавателей");
                return;
            }

            Console.WriteLine("Все преподаватели университета:");
            foreach (var teacher in Teachers.OrderBy(t => t.TeacherId))
            {
                teacher.DisplayInfo();
                Console.WriteLine("---");
            }
        }

        public void DisplayAllCourses()
        {
            if (Courses.Count == 0)
            {
                Console.WriteLine("В университете нет курсов");
                return;
            }

            Console.WriteLine("Все курсы университета:");
            foreach (var course in Courses.OrderBy(c => c.CourseId))
            {
                course.DisplayCourseInfo();
                Console.WriteLine("---");
            }
        }

        public Student FindStudentById(int studentId)
        {
            return Students.FirstOrDefault(s => s.StudentId == studentId);
        }

        public Teacher FindTeacherById(int teacherId)
        {
            return Teachers.FirstOrDefault(t => t.TeacherId == teacherId);
        }

        public Course FindCourseById(int courseId)
        {
            return Courses.FirstOrDefault(c => c.CourseId == courseId);
        }

        public List<Student> GetTopStudents(int count = 5)
        {
            return Students
                .Where(s => s.Courses.Count > 0)
                .OrderByDescending(s => s.AverageGrade)
                .Take(count)
                .ToList();
        }

        public List<Course> GetMostPopularCourses(int count = 5)
        {
            return Courses
                .OrderByDescending(c => c.EnrolledStudents.Count)
                .Take(count)
                .ToList();
        }
    }

    public class MenuManager
    {
        private University university;

        public MenuManager()
        {
            university = new University();
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            
            try
            {
                var teacher1 = new Teacher("Иван Петров", 45, "ivan@university.com", Department.ComputerScience);
                var teacher2 = new Teacher("Мария Сидорова", 38, "maria@university.com", Department.Mathematics);

                var student1 = new Student("Алексей Иванов", 20, "alex@student.com");
                var student2 = new Student("Елена Смирнова", 19, "elena@student.com");
                var student3 = new Student("Дмитрий Козлов", 21, "dmitry@student.com");

                var course1 = new Course("Программирование на C#", "Основы программирования на C#", 30);
                var course2 = new Course("Математический анализ", "Высшая математика", 25);
                var course3 = new Course("Физика", "Общая физика", 20);

                university.AddTeacher(teacher1);

                university.AddTeacher(teacher2);
                university.AddStudent(student1);
                university.AddStudent(student2);
                university.AddStudent(student3);
                university.AddCourse(course1);
                university.AddCourse(course2);
                university.AddCourse(course3);

                teacher1.AddCourse(course1);
                teacher2.AddCourse(course2);
                teacher2.AddCourse(course3);

                student1.EnrollInCourse(course1);
                student1.EnrollInCourse(course2);
                student2.EnrollInCourse(course1);
                student3.EnrollInCourse(course2);
                student3.EnrollInCourse(course3);

                teacher1.GradeStudent(student1, course1, 85);
                teacher1.GradeStudent(student2, course1, 92);
                teacher2.GradeStudent(student1, course2, 78);
                teacher2.GradeStudent(student3, course2, 88);
                teacher2.GradeStudent(student3, course3, 95);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации данных: {ex.Message}");
            }
        }

        public void DisplayMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ УНИВЕРСИТЕТОМ ===");
                Console.WriteLine("1. Управление студентами");
                Console.WriteLine("2. Управление преподавателями");
                Console.WriteLine("3. Управление курсами");
                Console.WriteLine("4. Запись на курсы");
                Console.WriteLine("5. Просмотр всех данных");
                Console.WriteLine("6. Статистика");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите опцию: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        HandleStudentManagement();
                        break;
                    case "2":
                        HandleTeacherManagement();
                        break;
                    case "3":
                        HandleCourseManagement();
                        break;
                    case "4":
                        HandleEnrollment();
                        break;
                    case "5":
                        HandleViewAllData();
                        break;
                    case "6":
                        HandleStatistics();
                        break;
                    case "0":
                        Console.WriteLine("Выход из системы...");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void HandleStudentManagement()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ СТУДЕНТАМИ ===");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Просмотреть всех студентов");
                Console.WriteLine("3. Найти студента по ID");
                Console.WriteLine("4. Просмотреть курсы студента");
                Console.WriteLine("5. Просмотреть оценки студента");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите опцию: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        university.DisplayAllStudents();
                        WaitForKey();
                        break;
                    case "3":
                        FindStudentById();
                        break;
                    case "4":
                        ViewStudentCourses();
                        break;
                    case "5":
                        ViewStudentGrades();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        WaitForKey();
                        break;
                }
            }
        }

        private void HandleTeacherManagement()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ ПРЕПОДАВАТЕЛЯМИ ===");
                Console.WriteLine("1. Добавить преподавателя");
                Console.WriteLine("2. Просмотреть всех преподавателей");
                Console.WriteLine("3. Найти преподавателя по ID");
                Console.WriteLine("4. Просмотреть курсы преподавателя");
                Console.WriteLine("5. Назначить преподавателя на курс");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите опцию: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        AddTeacher();
                        break;
                    case "2":
                        university.DisplayAllTeachers();
                        WaitForKey();
                        break;
                    case "3":
                        FindTeacherById();
                        break;
                    case "4":
                        ViewTeacherCourses();
                        break;
                    case "5":
                        AssignTeacherToCourse();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        WaitForKey();
                        break;
                }
            }
        }

        private void HandleCourseManagement()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ КУРСАМИ ===");
                Console.WriteLine("1. Добавить курс");
                Console.WriteLine("2. Просмотреть все курсы");
                Console.WriteLine("3. Найти курс по ID");
                Console.WriteLine("4. Просмотреть студентов курса");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите опцию: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        AddCourse();
                        break;
                    case "2":
                        university.DisplayAllCourses();
                        WaitForKey();
                        break;
                    case "3":
                        FindCourseById();
                        break;
                    case "4":
                        ViewCourseStudents();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        WaitForKey();
                        break;
                }
            }
        }

        private void HandleEnrollment()
        {
            Console.Clear();
            Console.WriteLine("=== ЗАПИСЬ НА КУРСЫ ===");


            try
            {
                Console.Write("Введите ID студента: ");
                if (!int.TryParse(Console.ReadLine(), out int studentId))
                {
                    Console.WriteLine("Неверный формат ID");
                    WaitForKey();
                    return;
                }

                var student = university.FindStudentById(studentId);
                if (student == null)
                {
                    Console.WriteLine("Студент не найден");
                    WaitForKey();
                    return;
                }

                Console.Write("Введите ID курса: ");
                if (!int.TryParse(Console.ReadLine(), out int courseId))
                {
                    Console.WriteLine("Неверный формат ID");
                    WaitForKey();
                    return;
                }

                var course = university.FindCourseById(courseId);
                if (course == null)
                {
                    Console.WriteLine("Курс не найден");
                    WaitForKey();
                    return;
                }

                student.EnrollInCourse(course);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            WaitForKey();
        }

        private void HandleViewAllData()
        {
            Console.Clear();
            Console.WriteLine("=== ВСЕ ДАННЫЕ УНИВЕРСИТЕТА ===");

            Console.WriteLine("\n--- СТУДЕНТЫ ---");
            university.DisplayAllStudents();

            Console.WriteLine("\n--- ПРЕПОДАВАТЕЛИ ---");
            university.DisplayAllTeachers();

            Console.WriteLine("\n--- КУРСЫ ---");
            university.DisplayAllCourses();

            WaitForKey();
        }

        private void HandleStatistics()
        {
            Console.Clear();
            Console.WriteLine("=== СТАТИСТИКА УНИВЕРСИТЕТА ===");

            Console.WriteLine($"Общее количество студентов: {university.Students.Count}");
            Console.WriteLine($"Общее количество преподавателей: {university.Teachers.Count}");
            Console.WriteLine($"Общее количество курсов: {university.Courses.Count}");

            var topStudents = university.GetTopStudents(3);
            if (topStudents.Count > 0)
            {
                Console.WriteLine("\n--- ЛУЧШИЕ СТУДЕНТЫ ---");
                foreach (var student in topStudents)
                {
                    Console.WriteLine($"{student.Name} - Средний балл: {student.AverageGrade:F2}");
                }
            }

            var popularCourses = university.GetMostPopularCourses(3);
            if (popularCourses.Count > 0)
            {
                Console.WriteLine("\n--- ПОПУЛЯРНЫЕ КУРСЫ ---");
                foreach (var course in popularCourses)
                {
                    Console.WriteLine($"{course.CourseName} - {course.EnrolledStudents.Count} студентов");
                }
            }

            WaitForKey();
        }

        private void AddStudent()
        {
            try
            {
                Console.Write("Введите имя студента: ");
                string name = Console.ReadLine();

                Console.Write("Введите возраст студента: ");
                if (!int.TryParse(Console.ReadLine(), out int age))
                {
                    Console.WriteLine("Неверный формат возраста");
                    return;
                }

                Console.Write("Введите контактную информацию: ");
                string contactInfo = Console.ReadLine();

                var student = new Student(name, age, contactInfo);
                university.AddStudent(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении студента: {ex.Message}");
            }
            WaitForKey();
        }


        private void AddTeacher()
        {
            try
            {
                Console.Write("Введите имя преподавателя: ");
                string name = Console.ReadLine();

                Console.Write("Введите возраст преподавателя: ");
                if (!int.TryParse(Console.ReadLine(), out int age))
                {
                    Console.WriteLine("Неверный формат возраста");
                    return;
                }

                Console.Write("Введите контактную информацию: ");
                string contactInfo = Console.ReadLine();

                Console.WriteLine("Выберите отдел:");
                foreach (var dept in Enum.GetValues(typeof(Department)))
                {
                    Console.WriteLine($"{(int)dept}. {dept}");
                }
                if (!int.TryParse(Console.ReadLine(), out int deptIndex) || !Enum.IsDefined(typeof(Department), deptIndex))
                {
                    Console.WriteLine("Неверный выбор отдела");
                    return;
                }

                var department = (Department)deptIndex;
                var teacher = new Teacher(name, age, contactInfo, department);
                university.AddTeacher(teacher);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении преподавателя: {ex.Message}");
            }
            WaitForKey();
        }

        private void AddCourse()
        {
            try
            {
                Console.Write("Введите название курса: ");
                string name = Console.ReadLine();

                Console.Write("Введите описание курса: ");
                string description = Console.ReadLine();

                Console.Write("Введите максимальное количество студентов: ");
                if (!int.TryParse(Console.ReadLine(), out int maxStudents))
                {
                    Console.WriteLine("Неверный формат числа");
                    return;
                }

                var course = new Course(name, description, maxStudents);
                university.AddCourse(course);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении курса: {ex.Message}");
            }
            WaitForKey();
        }

        private void FindStudentById()
        {
            Console.Write("Введите ID студента: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID");
                return;
            }

            var student = university.FindStudentById(id);
            if (student != null)
                student.DisplayInfo();
            else
                Console.WriteLine("Студент не найден");

            WaitForKey();
        }

        private void FindTeacherById()
        {
            Console.Write("Введите ID преподавателя: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID");
                return;
            }

            var teacher = university.FindTeacherById(id);
            if (teacher != null)
                teacher.DisplayInfo();
            else
                Console.WriteLine("Преподаватель не найден");

            WaitForKey();
        }

        private void FindCourseById()
        {
            Console.Write("Введите ID курса: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID");
                return;
            }

            var course = university.FindCourseById(id);
            if (course != null)
                course.DisplayCourseInfo();
            else
                Console.WriteLine("Курс не найден");

            WaitForKey();
        }
        private void ViewStudentCourses()
        {
            Console.Write("Введите ID студента: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID");
                return;
            }

            var student = university.FindStudentById(id);
            if (student != null)
                student.ViewCourses();
            else
                Console.WriteLine("Студент не найден");

            WaitForKey();
        }

        private void ViewStudentGrades()
        {
            Console.Write("Введите ID студента: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID");
                return;
            }

            var student = university.FindStudentById(id);
            if (student != null)
                student.ViewGrades();
            else
                Console.WriteLine("Студент не найден");

            WaitForKey();
        }

        private void ViewTeacherCourses()
        {
            Console.Write("Введите ID преподавателя: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID");
                return;
            }

            var teacher = university.FindTeacherById(id);
            if (teacher != null)
                teacher.ViewTeachingCourses();
            else
                Console.WriteLine("Преподаватель не найден");

            WaitForKey();
        }

        private void ViewCourseStudents()
        {
            Console.Write("Введите ID курса: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID");
                return;
            }

            var course = university.FindCourseById(id);
            if (course != null)
                course.ViewEnrolledStudents();
            else
                Console.WriteLine("Курс не найден");

            WaitForKey();
        }

        private void AssignTeacherToCourse()
        {
            Console.Write("Введите ID преподавателя: ");
            if (!int.TryParse(Console.ReadLine(), out int teacherId))
            {
                Console.WriteLine("Неверный формат ID");
                return;
            }

            Console.Write("Введите ID курса: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Неверный формат ID");
                return;
            }

            var teacher = university.FindTeacherById(teacherId);
            var course = university.FindCourseById(courseId);

            if (teacher != null && course != null)
                teacher.AddCourse(course);
            else
                Console.WriteLine("Преподаватель или курс не найден");

            WaitForKey();
        }

        private void WaitForKey()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
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