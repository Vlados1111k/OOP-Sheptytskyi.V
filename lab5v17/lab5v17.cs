using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lab5Cinema
{
    // --- Власні винятки ---
    
    // Виняток, що виникає при спробі забронювати вже зайняте місце
    public class SeatAlreadyBookedException : Exception
    {
        public SeatAlreadyBookedException(int row, int seat)
            : base($"Помилка бронювання: Місце [Ряд: {row + 1}, Місце: {seat + 1}] вже зайняте.") { }
    }

    // --- Сутності ---

    // Клас місця. Містить дані про ціну, статус та координати.
    public class Seat
    {
        public int Row { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
        public bool IsBooked { get; set; } = false;

        public override string ToString()
        {
            return IsBooked ? "[ X ]" : $"[{Price}]";
        }
    }

    // --- Узагальнений компонент (Generics) ---

    // Узагальнена матриця для зберігання схеми залу.
    // Реалізує IEnumerable<T>, щоб дозволити використання LINQ безпосередньо над об'єктом матриці.
    public class Matrix<T> : IEnumerable<T>
    {
        private readonly T[,] _data;
        public int Rows { get; }
        public int Cols { get; }

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            _data = new T[rows, cols];
        }

        // Індексатор для зручного доступу matrix[i, j]
        public T this[int row, int col]
        {
            get
            {
                ValidateIndex(row, col);
                return _data[row, col];
            }
            set
            {
                ValidateIndex(row, col);
                _data[row, col] = value;
            }
        }

        // Валідація вхідних координат
        private void ValidateIndex(int row, int col)
        {
            if (row < 0 || row >= Rows || col < 0 || col >= Cols)
                throw new IndexOutOfRangeException($"Координати [{row},{col}] виходять за межі матриці.");
        }

        // Реалізація перебору елементів для LINQ
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    yield return _data[i, j];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    // --- Головний клас (Composition) ---

    public class CinemaHall
    {
        public string Name { get; set; }
        
        // Композиція: Зал містить матрицю місць як невід'ємну частину
        private Matrix<Seat> _seats; 

        public CinemaHall(string name, int rows, int cols, decimal basePrice)
        {
            Name = name;
            _seats = new Matrix<Seat>(rows, cols);
            InitializeSeats(basePrice);
        }

        // Ініціалізація місць із динамічним ціноутворенням
        private void InitializeSeats(decimal basePrice)
        {
            for (int i = 0; i < _seats.Rows; i++)
            {
                for (int j = 0; j < _seats.Cols; j++)
                {
                    // Наприклад, кожен наступний ряд дорожчий на 10 од.
                    decimal currentPrice = basePrice + (i * 10);
                    _seats[i, j] = new Seat { Row = i, Number = j, Price = currentPrice };
                }
            }
        }

        // Метод бронювання з перевіркою бізнес-логіки
        public void BookSeat(int row, int col)
        {
            // Може викинути IndexOutOfRangeException (перевірка всередині Matrix)
            var seat = _seats[row, col]; 

            if (seat.IsBooked)
            {
                // Викидаємо власний виняток
                throw new SeatAlreadyBookedException(row, col);
            }

            seat.IsBooked = true;
            Console.WriteLine($"Успішно заброньовано: Ряд {row + 1}, Місце {col + 1}. Ціна: {seat.Price}");
        }

        // --- Методи обчислення (LINQ) ---

        public decimal CalculateTotalRevenue()
        {
            // Сума цін тільки заброньованих місць
            return _seats.Where(s => s.IsBooked).Sum(s => s.Price);
        }

        public double CalculateOccupancyPercentage()
        {
            int totalSeats = _seats.Rows * _seats.Cols;
            if (totalSeats == 0) return 0;
            
            int bookedCount = _seats.Count(s => s.IsBooked);
            return (double)bookedCount / totalSeats * 100;
        }

        public void PrintLayout()
        {
            Console.WriteLine($"\n--- Схема залу: {Name} ---");
            for (int i = 0; i < _seats.Rows; i++)
            {
                for (int j = 0; j < _seats.Cols; j++)
                {
                    Console.Write(_seats[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("--------------------------\n");
        }
    }
}