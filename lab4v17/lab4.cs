using System;
using System.Linq;

public interface ISortStrategy
{
    void Sort(int[] array);
}

public class AscendingSortStrategy : ISortStrategy
{
    public void Sort(int[] array)
    {
        Array.Sort(array);
    }
}

public class DescendingSortStrategy : ISortStrategy
{
    public void Sort(int[] array)
    {
        Array.Sort(array);
        Array.Reverse(array);
    }
}

public class ArrayCalculator
{
    private int[] _sortedArray;

    public ArrayCalculator(int[] sortedArray)
    {
        if (sortedArray == null || sortedArray.Length == 0)
        {
            throw new ArgumentException("Масив не може бути порожнім, друже!");
        }
        _sortedArray = sortedArray;
    }

    public double GetAverage()
    {
        return _sortedArray.Average();
    }

    public int GetMin()
    {
        return _sortedArray.Min();
    }

    public int GetMax()
    {
        return _sortedArray.Max();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        int[] myMessyArray = { 5, 1, 100, 3, 42, 22 };
        
        Console.WriteLine("--- Старт! ---");
        Console.WriteLine($"Оригінальний масив: [{string.Join(", ", myMessyArray)}]");
        Console.WriteLine("---------------------------------");

        Console.WriteLine("Спроба 1: Сортування ПО ЗРОСТАННЮ");
        ISortStrategy strategyUp = new AscendingSortStrategy(); 
        int[] sortedUp = (int[])myMessyArray.Clone(); 
        strategyUp.Sort(sortedUp); 
        Console.WriteLine($"Результат:           [{string.Join(", ", sortedUp)}]");

        ArrayCalculator calcUp = new ArrayCalculator(sortedUp);
        Console.WriteLine($"Мін: {calcUp.GetMin()}");
        Console.WriteLine($"Макс: {calcUp.GetMax()}");
        Console.WriteLine($"Середнє: {calcUp.GetAverage():F2}");

        Console.WriteLine("---------------------------------");

        Console.WriteLine("Спроба 2: Сортування ПО СПАДАННЮ");
        ISortStrategy strategyDown = new DescendingSortStrategy(); 
        int[] sortedDown = (int[])myMessyArray.Clone(); 
        strategyDown.Sort(sortedDown); 
        Console.WriteLine($"Результат:           [{string.Join(", ", sortedDown)}]");

        ArrayCalculator calcDown = new ArrayCalculator(sortedDown);
        Console.WriteLine($"Мін: {calcDown.GetMin()}");
        Console.WriteLine($"Макс: {calcDown.GetMax()}");
        Console.WriteLine($"Середнє: {calcDown.GetAverage():F2}");

        Console.WriteLine("---------------------------------");
        Console.WriteLine("Готово! Можна йти звідси.");
    }
}