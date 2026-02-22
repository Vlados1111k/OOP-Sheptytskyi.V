using System;
using System.Collections.Generic;

namespace lab24
{
    //Stategy

    // Інтерфейс
    public interface INumericOperationStrategy
    {
        double Execute(double value);
    }

    // Квадрат
    public class SquareOperationStrategy : INumericOperationStrategy
    {
        public double Execute(double value) => value * value;
    }

    // Куб
    public class CubeOperationStrategy : INumericOperationStrategy
    {
        public double Execute(double value) => value * value * value;
    }

    // Корінь
    public class SquareRootOperationStrategy : INumericOperationStrategy
    {
        public double Execute(double value) => Math.Sqrt(value);
    }

    // Процесор
    public class NumericProcessor
    {
        private INumericOperationStrategy _strategy;

        public NumericProcessor(INumericOperationStrategy strategy) => _strategy = strategy;

        public void SetStrategy(INumericOperationStrategy strategy) => _strategy = strategy;

        public double Process(double input) => _strategy.Execute(input);
    }

    //Observer

    public class ResultPublisher
    {
        public event Action<double, string> ResultCalculated;

        public void PublishResult(double result, string operationName)
        {
            ResultCalculated?.Invoke(result, operationName);
        }
    }

    // Спостерігач 1
    public class ConsoleLoggerObserver
    {
        public void OnResultCalculated(double result, string op) 
            => Console.WriteLine($"[LOG]: Операція '{op}' видала результат: {result}");
    }

    // Спостерігач 2
    public class HistoryLoggerObserver
    {
        public List<string> History { get; } = new List<string>();
        public void OnResultCalculated(double result, string op) 
            => History.Add($"{op}: {result}");
    }

    // Спостерігач 3
    public class ThresholdNotifierObserver
    {
        private double _threshold;
        public ThresholdNotifierObserver(double threshold) => _threshold = threshold;

        public void OnResultCalculated(double result, string op)
        {
            if (result > _threshold)
                Console.WriteLine($"[ALERT]: Результат {result} перевищив поріг {_threshold}!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var processor = new NumericProcessor(new SquareOperationStrategy());
            var publisher = new ResultPublisher();

            var logger = new ConsoleLoggerObserver();
            var history = new HistoryLoggerObserver();
            var alarm = new ThresholdNotifierObserver(100); // Поріг 100

            publisher.ResultCalculated += logger.OnResultCalculated;
            publisher.ResultCalculated += history.OnResultCalculated;
            publisher.ResultCalculated += alarm.OnResultCalculated;

            double input = 12;

            double res1 = processor.Process(input);
            publisher.PublishResult(res1, "Квадрат");

            processor.SetStrategy(new CubeOperationStrategy());
            double res2 = processor.Process(5); 
            publisher.PublishResult(res2, "Куб");

            processor.SetStrategy(new SquareRootOperationStrategy());
            double res3 = processor.Process(81);
            publisher.PublishResult(res3, "Корінь");

            Console.WriteLine("\nВміст історії:");
            history.History.ForEach(h => Console.WriteLine(h));

            Console.ReadKey();
        }
    }
}