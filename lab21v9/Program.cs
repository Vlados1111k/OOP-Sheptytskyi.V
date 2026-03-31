using System;
using System.Collections.Generic;

namespace lab21v9
{
    public interface IParkingStrategy
    {
        decimal CalculatePrice(int duration);
    }

    public class HourlyParkingStrategy : IParkingStrategy
    {
        public decimal CalculatePrice(int hours)
        {
            decimal rate = 30m;
            if (hours > 5) rate = 25m; 
            return hours * rate;
        }
    }

    public class DailyParkingStrategy : IParkingStrategy
    {
        public decimal CalculatePrice(int days)
        {
            decimal dayRate = 200m;
            if (days > 7) dayRate = 180m;
            return days * dayRate;
        }
    }

    public class MonthlySubscriptionStrategy : IParkingStrategy
    {
        public decimal CalculatePrice(int months)
        {
            decimal monthRate = 3000m;
            if (months >= 6) monthRate = 2500m;
            return months * monthRate;
        }
    }

    public class VIPParkingStrategy : IParkingStrategy
    {
        public decimal CalculatePrice(int hours)
        {
            return (hours * 50m) + 100m;
        }
    }

    public static class ParkingStrategyFactory
    {
        private static readonly Dictionary<string, IParkingStrategy> _strategies = 
            new Dictionary<string, IParkingStrategy>(StringComparer.OrdinalIgnoreCase)
        {
            { "Hourly", new HourlyParkingStrategy() },
            { "Daily", new DailyParkingStrategy() },
            { "Monthly", new MonthlySubscriptionStrategy() },
            { "VIP", new VIPParkingStrategy() }
        };

        public static IParkingStrategy CreateStrategy(string type)
        {
            if (_strategies.TryGetValue(type, out var strategy))
            {
                return strategy;
            }
            throw new ArgumentException("Такого тарифу не існує");
        }
    }

    public class ParkingService
    {
        public decimal GetTotalCost(int duration, IParkingStrategy strategy)
        {
            return strategy.CalculatePrice(duration);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ParkingService service = new ParkingService();

            Console.WriteLine("Система управління парковкою 'SmartPark'");
            
            Console.Write("Оберіть тариф (Hourly, Daily, Monthly, VIP): ");
            string type = Console.ReadLine();

            Console.Write("Введіть тривалість (год/дні/міс): ");
            if (!int.TryParse(Console.ReadLine(), out int duration)) duration = 0;

            try
            {
                IParkingStrategy strategy = ParkingStrategyFactory.CreateStrategy(type);
                decimal total = service.GetTotalCost(duration, strategy);
                
                Console.WriteLine($"\nВартість паркування: {total} грн.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}