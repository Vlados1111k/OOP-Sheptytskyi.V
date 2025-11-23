using System;
using System.Collections.Generic;
using System.Linq;

namespace lab6v17
{
    // Оголошення власного делегата для арифметичних операцій
    public delegate double MathOperation(double a, double b);

    // Клас для зберігання записів про температуру
    public class TemperatureRecord
    {
        public string Day { get; set; }
        public double Degrees { get; set; }

        public TemperatureRecord(string day, double degrees)
        {
            Day = day;
            Degrees = degrees;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Lab 6: Делегати, Лямбда-вирази та LINQ ===\n");

            // --- ЧАСТИНА 1: Власний делегат ---
            Console.WriteLine("--- 1. Власний делегат ---");
            
            // Ініціалізація делегата лямбда-виразами
            MathOperation add = (x, y) => x + y;
            MathOperation multiply = (x, y) => x * y;

            Console.WriteLine($"Додавання (5 + 3): {add(5, 3)}");
            Console.WriteLine($"Множення (5 * 3): {multiply(5, 3)}");


            // --- ЧАСТИНА 2: Анонімні методи vs Лямбда-вирази ---
            Console.WriteLine("\n--- 2. Анонімні методи vs Лямбда-вирази ---");

            // Використання анонімного методу
            Predicate<int> isEvenAnonymous = delegate (int x)
            {
                return x % 2 == 0;
            };

            // Використання лямбда-виразу
            Predicate<int> isEvenLambda = x => x % 2 == 0;

            Console.WriteLine($"Чи парне 4 (Анонімний метод)? {isEvenAnonymous(4)}");
            Console.WriteLine($"Чи парне 7 (Лямбда)? {isEvenLambda(7)}");


            // --- ЧАСТИНА 3: Вбудовані делегати (Action, Func) ---
            Console.WriteLine("\n--- 3. Температурний режим (Action & Func) ---");

            List<TemperatureRecord> weekWeather = new List<TemperatureRecord>
            {
                new TemperatureRecord("Понеділок", 22.5),
                new TemperatureRecord("Вівторок", 26.0),
                new TemperatureRecord("Середа", 24.0),
                new TemperatureRecord("Четвер", 28.5),
                new TemperatureRecord("П'ятниця", 19.0)
            };

            // Func<double, bool>: перевіряє умову (температура > 25)
            Func<double, bool> isHot = t => t > 25.0;

            // Action<double>: виводить повідомлення в консоль
            Action<double> printHot = t => Console.WriteLine($"Спекотно: {t}°C");

            Console.WriteLine("Дні з температурою вище 25°C:");
            foreach (var day in weekWeather)
            {
                if (isHot(day.Degrees))
                {
                    Console.Write($"{day.Day} - ");
                    printHot(day.Degrees); 
                }
            }


            // --- ЧАСТИНА 4: LINQ ---
            Console.WriteLine("\n--- 4. LINQ (Where, Select, OrderBy, Aggregate) ---");

            var numbers = new List<int> { 5, 1, 9, 12, 4, 3, 8 };
            Console.WriteLine($"Початковий список: {string.Join(", ", numbers)}");

            // Where: фільтрація елементів
            var bigNumbers = numbers.Where(n => n > 5);
            Console.WriteLine($"Where (>5): {string.Join(", ", bigNumbers)}");

            // OrderBy: сортування за зростанням
            var sortedNumbers = numbers.OrderBy(n => n);
            Console.WriteLine($"OrderBy: {string.Join(", ", sortedNumbers)}");

            // Select: проєкція (піднесення до квадрату)
            var squares = numbers.Select(n => n * n);
            Console.WriteLine($"Select (квадрати): {string.Join(", ", squares)}");

            // Aggregate: обчислення суми елементів
            int sum = numbers.Aggregate((total, next) => total + next);
            Console.WriteLine($"Aggregate (Сума): {sum}");

            // Використання ланцюжка методів
            var result = numbers
                .Where(n => n % 2 == 0)       // Відбір парних чисел
                .OrderByDescending(n => n)    // Сортування за спаданням
                .Select(n => $"[{n}]");       // Форматування рядка

            Console.WriteLine($"Результат обробки: {string.Join(" -> ", result)}");

            Console.WriteLine("\nНатисніть Enter для завершення...");
            Console.ReadLine();
        }
    }
}