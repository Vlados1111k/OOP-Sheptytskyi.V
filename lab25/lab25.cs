using System;
using System.Collections.Generic;
using System.IO;

namespace lab25
{
    //Factory methon && loger
    public interface ILogger { void Log(string message); }

    public class ConsoleLogger : ILogger 
    { 
        public void Log(string message) => Console.WriteLine($"[ConsoleLog]: {message}"); 
    }

    public class FileLogger : ILogger 
    { 
        public void Log(string message) => File.AppendAllText("log.txt", $"[FileLog]: {message}\n"); 
    }

    public abstract class LoggerFactory { public abstract ILogger CreateLogger(); }
    public class ConsoleLoggerFactory : LoggerFactory { public override ILogger CreateLogger() => new ConsoleLogger(); }
    public class FileLoggerFactory : LoggerFactory { public override ILogger CreateLogger() => new FileLogger(); }

    //Singleton: LoggerManager
    public class LoggerManager
    {
        private static LoggerManager _instance;
        private ILogger _logger;
        private LoggerFactory _factory;

        private LoggerManager() { }

        public static LoggerManager Instance => _instance ??= new LoggerManager();

        public void SetFactory(LoggerFactory factory)
        {
            _factory = factory;
            _logger = _factory.CreateLogger();
        }

        public void Log(string message) => _logger?.Log(message);
    }

    //Stragety: DataProcessor
    public interface IDataProcessorStrategy { string Process(string data); }

    public class EncryptDataStrategy : IDataProcessorStrategy 
    { 
        public string Process(string data) => $"<Encrypted>{data}</Encrypted>"; 
    }

    public class CompressDataStrategy : IDataProcessorStrategy 
    { 
        public string Process(string data) => $"Zip({data})"; 
    }

    public class DataContext
    {
        private IDataProcessorStrategy _strategy;
        public DataContext(IDataProcessorStrategy strategy) => _strategy = strategy;
        public void SetStrategy(IDataProcessorStrategy strategy) => _strategy = strategy;
        public string ExecuteStrategy(string data) => _strategy.Process(data);
    }

    //Observer: DataPublisher
    public class DataPublisher
    {
        public event Action<string> DataProcessed;
        public void PublishDataProcessed(string result) => DataProcessed?.Invoke(result);
    }

    public class ProcessingLoggerObserver
    {
        public void OnDataProcessed(string result)
        {
            LoggerManager.Instance.Log($"Observer отримав дані: {result}");
        }
    }

    //Main program
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            var publisher = new DataPublisher();
            var observer = new ProcessingLoggerObserver();
            publisher.DataProcessed += observer.OnDataProcessed;

            //Сценарій 1
            PrintHeader("Сценарій 1: Повна інтеграція (Console + Encrypt)");
            LoggerManager.Instance.SetFactory(new ConsoleLoggerFactory());
            var context = new DataContext(new EncryptDataStrategy());
            
            string res1 = context.ExecuteStrategy("SecretData");
            publisher.PublishDataProcessed(res1);

            //Сценарій 2
            PrintHeader("Сценарій 2: Динамічна зміна логера (на File)");
            LoggerManager.Instance.SetFactory(new FileLoggerFactory());
            
            string res2 = context.ExecuteStrategy("ImportantData");
            publisher.PublishDataProcessed(res2);
            Console.WriteLine("(Перевірте файл log.txt у папці з програмою)");

            //Сценарій 3
            PrintHeader("Сценарій 3: Динамічна зміна стратегії (на Compress)");
            LoggerManager.Instance.SetFactory(new ConsoleLoggerFactory()); 
            context.SetStrategy(new CompressDataStrategy());
            
            string res3 = context.ExecuteStrategy("BigDataChunk");
            publisher.PublishDataProcessed(res3);

            Console.WriteLine("\nУспішно!");
            Console.ReadKey();
        }

        static void PrintHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n--- {title} ---");
            Console.ResetColor();
        }
    }
}