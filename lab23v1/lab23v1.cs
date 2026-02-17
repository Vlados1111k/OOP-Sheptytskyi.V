using System;

namespace lab23v1
{
    //Порушення ISP та DIP
    
    public interface ISystemTask
    {
        void Log(string message);
        void SendEmail(string admin, string msg);
        void GenerateReport();
    }

    public class OldSystemMonitor
    {
        //Порушення DIP - створення всередині
        private readonly Logger _logger = new Logger();

        public void Run()
        {
            _logger.Log("Система запущена");
        }
    }

    public class Logger { public void Log(string m) => Console.WriteLine($"Log: {m}"); }

    //ISP та DIP дотримано

    //ISP Розділення інтерфейса
    public interface ILogger { void Log(string message); }
    public interface INotifier { void SendEmail(string admin, string message); }
    public interface IReporter { void GenerateReport(); }

    // Реалізації
    public class ConsoleLogger : ILogger 
    { 
        public void Log(string m) => Console.WriteLine($"[LOG]: {m}"); 
    }

    public class EmailNotifier : INotifier 
    { 
        public void SendEmail(string a, string m) => Console.WriteLine($"[EMAIL] to {a}: {m}"); 
    }

    //DIP 
    public class SystemMonitor
    {
        private readonly ILogger _logger;
        private readonly INotifier _notifier;

        // Впровадження через конструктор
        public SystemMonitor(ILogger logger, INotifier notifier)
        {
            _logger = logger;
            _notifier = notifier;
        }

        public void Start()
        {
            _logger.Log("Моніторинг розпочато.");
            _notifier.SendEmail("admin@site.com", "Система в нормі.");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("lab23: ISP & DIP\n");

            // Конфігурація залежностей
            ILogger myLogger = new ConsoleLogger();
            INotifier myNotifier = new EmailNotifier();

            // Передання залежності в об'єкт
            SystemMonitor monitor = new SystemMonitor(myLogger, myNotifier);
            
            monitor.Start();

        }
    }
}