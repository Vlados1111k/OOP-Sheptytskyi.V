using System;

namespace lab22v1
{

    public class Rectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public int GetArea() => Width * Height;
    }

    public class Square : Rectangle
    {
        public override int Width
        {
            get => base.Width;
            set { base.Width = value; base.Height = value; }
        }

        public override int Height
        {
            get => base.Height;
            set { base.Width = value; base.Height = value; }
        }
    }

    // Рефакторинг 

    public interface IShape
    {
        int GetArea();
    }

    public class ProperRectangle : IShape
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int GetArea() => Width * Height;
    }

    public class ProperSquare : IShape
    {
        public int Side { get; set; }
        public int GetArea() => Side * Side;
    }


    class Program
    {
        // Клієнтський метод, який ламається через порушення LSP
        static void BrokenClientMethod(Rectangle rect)
        {
            rect.Width = 10;
            rect.Height = 5;

            Console.WriteLine($"Очікували площу 50, отримали: {rect.GetArea()}");
            
            if (rect.GetArea() != 50)
            {
                Console.WriteLine("Помилка LSP: Об'єкт поводиться не так, як  чекали від Rectangle.");
            }
        }

        // Клієнтський метод для правильної ієрархії
        static void GoodClientMethod(IShape shape)
        {
            Console.WriteLine($"Площа фігури: {shape.GetArea()}");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Демонстрація LSP\n");

            // Демонструємо проблему 
            Console.WriteLine("Проблема (Square успадковує Rectangle):");
            Rectangle badRect = new Square(); 
            BrokenClientMethod(badRect);


            // Демонструємо рішення 
            Console.WriteLine("Рішення (Використання інтерфейсу IShape):");
            IShape rect = new ProperRectangle { Width = 10, Height = 5 };
            IShape square = new ProperSquare { Side = 5 };

            GoodClientMethod(rect);
            GoodClientMethod(square);

            Console.ReadKey();
        }
    }
}