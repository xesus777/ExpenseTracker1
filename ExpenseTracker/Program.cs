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

    