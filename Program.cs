using System;
using System.Collections.Generic;
using System.Text;

namespace Lab16v19
{

    // Порушення SRP
    public class BadUserActivityLogger
    {
        public void LogEverything(string activity)
        {
            if (string.IsNullOrEmpty(activity)) return; 

            Console.WriteLine($"[BAD] Запис у файл: {activity}"); 
            Console.WriteLine($"[BAD] Відправка на сервер: {activity}"); 
        }
    }

    // рефакторинг 
    public interface IActivityFilter { bool IsValid(string activity); }
    public interface IFileLogger { void Save(string activity); }
    public interface IServerLogger { void Send(string activity); }

    public class ActivityFilter : IActivityFilter
    {
        public bool IsValid(string activity) => !string.IsNullOrWhiteSpace(activity);
    }

    public class FileLogger : IFileLogger
    {
        public void Save(string activity) => 
            Console.WriteLine($"[FILE]: Рядок '{activity}' успішно записано в log.txt");
    }

    public class ServerLogger : IServerLogger
    {
        public void Send(string activity) => 
            Console.WriteLine($"[SERVER]: Пакет '{activity}' відправлено на віддалений сервер.");
    }

    // UserActivityService
    public class UserActivityService
    {
        private readonly IActivityFilter _filter;
        private readonly IFileLogger _fileLogger;
        private readonly IServerLogger _serverLogger;

        public UserActivityService(IActivityFilter filter, IFileLogger fileLogger, IServerLogger serverLogger)
        {
            _filter = filter;
            _fileLogger = fileLogger;
            _serverLogger = serverLogger;
        }

        public void Execute(string activity)
        {
            Console.WriteLine($"Обробка події: \"{activity}\"");
            
            if (_filter.IsValid(activity))
            {
                _fileLogger.Save(activity);
                _serverLogger.Send(activity);
                Console.WriteLine("Результат: Операція успішна.\n");
            }
            else
            {
                Console.WriteLine("Результат: Подія порожня, ігноруємо.\n");
            }
        }
    }


    // Main

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Самостійна робота №16 (Варіант 19)\n");

            Console.WriteLine(" Демонстрація порушення SRP:");
            var badLogger = new BadUserActivityLogger();
            badLogger.LogEverything("Спроба входу");
            Console.WriteLine(new string('-', 40));

            Console.WriteLine("\n Демонстрація після рефакторингу:");
            
            var filter = new ActivityFilter();
            var fileLog = new FileLogger();
            var serverLog = new ServerLogger();

            var service = new UserActivityService(filter, fileLog, serverLog);

            service.Execute("Користувач натиснув 'Вихід'");
            service.Execute(""); 
            service.Execute("Зміна паролю");

            Console.WriteLine("Роботу виконав студент автоматичноюю системою перевірки знань.");
            Console.ReadKey();
        }
    }
}