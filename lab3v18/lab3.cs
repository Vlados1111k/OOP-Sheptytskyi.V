using System;
using System.Collections.Generic;

namespace FoodExample
{
    //Базовий клас
    class FoodItem
    {
        public string Name { get; set; }
        public int Calories { get; set; }

        public FoodItem(string name, int calories)
        {
            Name = name;
            Calories = calories;
        }

        // Віртуальний метод
        public virtual string GetCategory()
        {
            return "Загальна їжа";
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"{Name} - {Calories} калорій ({GetCategory()})");
        }
    }

    // Похідний клас Fruit
    class Fruit : FoodItem
    {
        public string Color { get; set; }

        public Fruit(string name, int calories, string color)
            : base(name, calories) // Виклик конструктора базового класу
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
    }

    // Похідний клас Meat
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
    }

    // Основна програма
    class Program
    {
        static void Main(string[] args)
        {
            // Поліморфізм.Колекція базового типу, що містить різні об'єкти
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
                item.ShowInfo(); // поліморфізм
                totalCalories += item.Calories;
            }

            Console.WriteLine("\nЗагальна калорійність страви: " + totalCalories);

            // порівняння за категоріями
            Console.WriteLine("\nПорівняння категорій:");
            Console.WriteLine(menu[0].GetCategory() == menu[2].GetCategory()
                ? "Одна категорія"
                : "Різні категорії");
        }
    }
}
