using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalyzer
{
    public class TextStatistics
    {
        public int TotalWords { get; set; }
        public string ShortestWord { get; set; }
        public string LongestWord { get; set; }
        public int TotalSentences { get; set; }
        public int VowelCount { get; set; }
        public int ConsonantCount { get; set; }
        public Dictionary<char, int> LetterFrequency { get; set; }
        public DateTime AnalysisDate { get; set; }
        public string TextPreview { get; set; }

        public TextStatistics()
        {
            LetterFrequency = new Dictionary<char, int>();
        }

        public void DisplayStatistics()
        {
            Console.WriteLine("\n=== ДЕТАЛЬНАЯ СТАТИСТИКА ТЕКСТА ===");
            Console.WriteLine($"Дата анализа: {AnalysisDate:dd.MM.yyyy HH:mm:ss}");
            Console.WriteLine($"Предпросмотр текста: {TextPreview}...");
            Console.WriteLine($"Общее количество слов: {TotalWords}");
            Console.WriteLine($"Количество предложений: {TotalSentences}");
            Console.WriteLine($"Самое короткое слово: '{ShortestWord}'");
            Console.WriteLine($"Самое длинное слово: '{LongestWord}'");
            Console.WriteLine($"Гласные буквы: {VowelCount}");
            Console.WriteLine($"Согласные буквы: {ConsonantCount}");
            Console.WriteLine($"Всего букв: {VowelCount + ConsonantCount}");

            Console.WriteLine("\nЧастота букв (отсортировано по убыванию):");
            var sortedFreq = LetterFrequency.OrderByDescending(x => x.Value);
            foreach (var entry in sortedFreq)
            {
                double percentage = (double)entry.Value / (VowelCount + ConsonantCount) * 100;
                Console.WriteLine($"  '{entry.Key}': {entry.Value} раз ({percentage:F1}%)");
            }
            Console.WriteLine("====================================\n");
        }

        public void DisplayCompactStatistics(int index)
        {
            Console.WriteLine($"{index}. [{AnalysisDate:dd.MM.yyyy HH:mm}] Слов: {TotalWords}, " +
                             $"Предложений: {TotalSentences}, Букв: {VowelCount + ConsonantCount}");
            Console.WriteLine($"   Краткий просмотр: {TextPreview}...");
        }
    }

    public class TextAnalyzer
    {
        private List<TextStatistics> statisticsHistory;
        private readonly char[] vowels = { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я',
                                          'a', 'e', 'i', 'o', 'u', 'y' };
        private readonly char[] sentenceSeparators = { '.', '!', '?' };

        public TextAnalyzer()
        {
            statisticsHistory = new List<TextStatistics>();
        }

        public string GetTextFromUser()
        {
            string text;
            do
            {
                Console.WriteLine("\nПожалуйста, введите текст (минимум 100 символов):");
                Console.WriteLine("---------------------------------------------------");
                text = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(text))
                {
                    Console.WriteLine("Ошибка: Текст не может быть пустым!");
                    continue;
                }

                if (text.Length < 100)
                {
                    Console.WriteLine($"Ошибка: Текст должен содержать минимум 100 символов. Сейчас: {text.Length}");
                    Console.WriteLine("Попробуйте еще раз.");
                }

            } while (string.IsNullOrWhiteSpace(text) || text.Length < 100);

            return text.Trim();
        }

        public TextStatistics AnalyzeText(string text)
        {
            var statistics = new TextStatistics();
            statistics.AnalysisDate = DateTime.Now;
            statistics.TextPreview = text.Length > 50 ? text.Substring(0, 50) : text;

            var words = SplitTextIntoWords(text);
            statistics.TotalWords = words.Length;
            if (words.Length > 0)
            {
                statistics.ShortestWord = words.OrderBy(word => word.Length).First();
                statistics.LongestWord = words.OrderByDescending(word => word.Length).First();
                CountLetters(text, statistics);
                CalculateLetterFrequency(text, statistics);
            }

            statistics.TotalSentences = CountSentences(text);
            statisticsHistory.Add(statistics);

            return statistics;
        }

        private string[] SplitTextIntoWords(string text)
        {
            char[] separators = { ' ', ',', '.', '!', '?', ';', ':', '\t', '\n', '\r', '(', ')', '[', ']', '{', '}' };
            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                       .Where(word => word.Length > 0 && char.IsLetterOrDigit(word[0]))
                       .ToArray();
        }

        private void CountLetters(string text, TextStatistics statistics)
        {
            statistics.VowelCount = 0;
            statistics.ConsonantCount = 0;

            foreach (char c in text.ToLower())
            {
                if (char.IsLetter(c))
                {
                    if (vowels.Contains(c))
                    {
                        statistics.VowelCount++;
                    }
                    else
                    {
                        statistics.ConsonantCount++;
                    }
                }
            }
        }
        private void CalculateLetterFrequency(string text, TextStatistics statistics)
        {
            foreach (char c in text.ToLower())
            {
                if (char.IsLetter(c))
                {
                    if (statistics.LetterFrequency.ContainsKey(c))
                    {
                        statistics.LetterFrequency[c]++;
                    }
                    else
                    {
                        statistics.LetterFrequency[c] = 1;
                    }
                }
            }
        }

        private int CountSentences(string text)
        {
            return text.Split(sentenceSeparators, StringSplitOptions.RemoveEmptyEntries)
                       .Count(s => s.Trim().Length > 0);
        }

        public List<TextStatistics> GetStatisticsHistory()
        {
            return statisticsHistory;
        }
    }
    