using System;
using System.Text;
using Lab5Cinema; // Підключення простору імен з попереднього файлу

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Система бронювання кінотеатру (Lab 5 Variant 17)");

        // Створення екземпляра залу (4 ряди, 5 місць)
        CinemaHall hall = new CinemaHall("Blue Hall", 4, 5, 100);

        try
        {
            // Початковий стан
            hall.PrintLayout();

            // 1. Коректні операції
            Console.WriteLine("Виконуємо бронювання...");
            hall.BookSeat(0, 0); // Ряд 1, Місце 1
            hall.BookSeat(2, 2); // Ряд 3, Місце 3
            hall.BookSeat(3, 4); // Останнє місце

            // 2. Демонстрація перехоплення власного винятку (SeatAlreadyBookedException)
            // Розкоментуйте рядок нижче для тестування:
            // hall.BookSeat(0, 0); 

            // 3. Демонстрація перехоплення системного винятку (IndexOutOfRangeException)
            // Розкоментуйте рядок нижче для тестування:
            // hall.BookSeat(10, 0);

            hall.PrintLayout();
        }
        catch (SeatAlreadyBookedException ex)
        {
            // Обробка специфічної помилки логіки
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Logic Error] {ex.Message}");
            Console.ResetColor();
        }
        catch (IndexOutOfRangeException ex)
        {
            // Обробка помилки виходу за межі масиву
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Critical Error] {ex.Message}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            // Обробка всіх інших непередбачених помилок
            Console.WriteLine($"[General Error] {ex.Message}");
        }
        finally
        {
            // Блок, що виконується завжди (наприклад, для закриття ресурсів або фіналізації)
            Console.WriteLine("Операцію бронювання завершено (блок finally).");
        }

        // --- Виведення статистики ---
        Console.WriteLine("\n=== Статистика сеансу ===");
        Console.WriteLine($"Загальний дохід: {hall.CalculateTotalRevenue()} грн");
        Console.WriteLine($"Заповненість залу: {hall.CalculateOccupancyPercentage():F2}%");
        
        Console.ReadKey();
    }
}