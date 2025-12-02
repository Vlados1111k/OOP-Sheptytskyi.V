using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab6_Compact
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1. Func: це готовий делегат. Приймає два int, повертає int.
            // Лямбда (x, y) => x + y 
            Func<int, int, int> add = (x, y) => x + y;
            Console.WriteLine($"Лямбда сума (10 + 5): {add(10, 5)}");

            // 2. Action: нічого не повертає, просто робить (наприклад, друкує).
            Action<string> print = msg => Console.WriteLine($"[INFO]: {msg}");
            print("Action працює, політ нормальний!");

            // 3. Дані (замість складного класу просто список температур)
            var temps = new List<double> { 22.5, 26.0, 24.0, 28.5, 19.0 };

            // 4. LINQ (Where -> OrderBy -> Select)
            var hotDays = temps
                .Where(t => t > 25)       
                .OrderBy(t => t)            
                .Select(t => $"{t}°C"); 

            print($"Спекотні дні: {string.Join(", ", hotDays)}");

            // 5. Aggregate: збиває все в одну купу (наприклад, суму)
            double totalHeat = temps.Aggregate((sum, next) => sum + next);
            print($"Сума всіх градусів: {totalHeat}");

            // 6. Predicate: просто повертає true/false
            Predicate<int> isEven = x => x % 2 == 0;
            Console.WriteLine($"\nЧи парне число 42? {isEven(42)}");

            Console.ReadLine();
        }
    }
}