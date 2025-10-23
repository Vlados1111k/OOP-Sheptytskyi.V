using System;
using System.Collections.Generic;

namespace FoodExample
{
    abstract class FoodItem 
    {
        public string Name { get; set; }
        public int Calories { get; set; }

        public FoodItem(string name, int calories)
        {
            Name = name;
            Calories = calories;
        }

        public virtual string GetCategory()
        {
            return "Загальна їжа";
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"{Name} - {Calories} калорій ({GetCategory()})");
        }
        
        public abstract void ShowStorageInfo();

        ~FoodItem()
        {
            Console.WriteLine($"[ДЕСТРУКТОР]: Продукт {Name} було утилізовано.");
        }
    }

    class Fruit : FoodItem
    {
        public string Color { get; set; }

        public Fruit(string name, int calories, string color)
            : base(name, calories) 
        {
            Color = color;
        }

        public override string GetCategory()
        {
            return "Фрукт";
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"{Name} ({Color}) - {Calories} калорій [{GetCategory()}]");
        }

        public override void ShowStorageInfo()
        {
            Console.WriteLine($"    -> {Name} слід зберігати в холодильнику.");
        }
    }

    class Meat : FoodItem
    {
        public string Type { get; set; }

        public Meat(string name, int calories, string type)
            : base(name, calories)
        {
            Type = type;
        }

        public override string GetCategory()
        {
            return "М'ясо";
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"{Name} ({Type}) - {Calories} калорій [{GetCategory()}]");
        }

        public override void ShowStorageInfo()
        {
            Console.WriteLine($"    -> {Name} слід зберігати в морозилці!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<FoodItem> menu = new List<FoodItem>
            {
                new Fruit("Яблуко", 52, "червоне"),
                new Fruit("Банан", 89, "жовтий"),
                new Meat("Курятина", 239, "пташине"),
                new Meat("Свинина", 301, "червоне м'ясо")
            };

            int totalCalories = 0;

            Console.WriteLine("Меню:");
            Console.WriteLine("------");
            foreach (var item in menu)
            {
                item.ShowInfo(); 
                item.ShowStorageInfo(); 
                totalCalories += item.Calories;
            }

            Console.WriteLine("\nЗагальна калорійність страви: " + totalCalories);

            Console.WriteLine("\nПорівняння категорій:");
            Console.WriteLine(menu[0].GetCategory() == menu[2].GetCategory()
                ? "Одна категорія"
                : "Різні категорії");
        }
    }
}
