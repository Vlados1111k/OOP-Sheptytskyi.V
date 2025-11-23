using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace lab7v17
{
    // --- КЛАСИ-АКТОРИ (Ті, що імітують бурхливу діяльність) ---

    public class FileProcessor
    {
        private int _attempts = 0;

        // Цей метод намагається завантажити замовлення з файлу
        public List<string> LoadOrderIds(string path)
        {
            _attempts++;
            Console.WriteLine($"[FileProcessor] Спроба #{_attempts} прочитати файл '{path}'...");

            // Імітуємо проблему перші 2 рази 
            if (_attempts <= 2)
            {
                Console.WriteLine($"[FileProcessor] Ой! Файл заблоковано або щось таке. (IOException)");
                throw new IOException("Файл тимчасово недоступний.");
            }

            Console.WriteLine("[FileProcessor] Успіх! Файл прочитано.");
            return new List<string> { "Order-101", "Order-102", "Order-103" };
        }
    }

    public class NetworkClient
    {
        private int _attempts = 0;

        // Цей метод стукає на сервер
        public List<string> GetOrdersFromApi(string url)
        {
            _attempts++;
            Console.WriteLine($"[NetworkClient] Спроба #{_attempts} підключитися до '{url}'...");

            // Імітуємо проблему перші 3 рази (сервер "впав", буває)
            if (_attempts <= 3)
            {
                Console.WriteLine($"[NetworkClient] Сервер не відповідає! (HttpRequestException)");
                throw new HttpRequestException("Помилка мережі 503.");
            }

            Console.WriteLine("[NetworkClient] Успіх! Дані отримано. Інтернет літає!");
            return new List<string> { "Order-201", "Order-202", "Order-203" };
        }
    }

    // --- НАШ ГЕРОЙ (RetryHelper) ---

    public static class RetryHelper
    {
        // Оце наш узагальнений метод (Generic). Він як швейцарський ніж - працює з будь-чим.
        public static T ExecuteWithRetry<T>(
            Func<T> operation, 
            int retryCount = 3, 
            TimeSpan initialDelay = default, 
            Func<Exception, bool> shouldRetry = null)
        {
            int attempt = 0;

            while (true)
            {
                try
                {
                    // Пробуємо виконати передану нам роботу
                    return operation();
                }
                catch (Exception ex)
                {
                    attempt++;

                    // Перевіряємо, чи треба пробувати ще. 
                    // Якщо спроби скінчились АБО фільтр помилок каже "досить" - викидаємо помилку далі.
                    bool isRetryAllowed = shouldRetry == null || shouldRetry(ex);

                    if (attempt > retryCount || !isRetryAllowed)
                    {
                        Console.WriteLine($"[RetryHelper] Здаємося після {attempt} спроб(и). Помилка: {ex.GetType().Name}");
                        throw; // Все, "бобік здох", кидаємо помилку нагору
                    }

                    // Рахуємо затримку: InitialDelay * 2^(номер спроби - 1)
                    double delayMultiplier = Math.Pow(2, attempt - 1);
                    var delay = initialDelay.TotalMilliseconds * delayMultiplier;
                    
                    // Якщо затримка 0, поставимо хоч трішки, щоб комп не згорів
                    if (delay <= 0) delay = 100; 

                    Console.WriteLine($"[RetryHelper] Невдача (спроба {attempt}). Причина: {ex.Message}");
                    Console.WriteLine($"[RetryHelper] Чекаємо {delay} мс перед наступною спробою...");

                    // Спимо (блокуємо потік)
                    Thread.Sleep((int)delay);
                }
            }
        }
    }

    // --- ГОЛОВНИЙ ВХІД ---

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Щоб українська мова була красивою в консолі
            Console.WriteLine("=== Лабораторна 7:  ===\n");

            var fileProc = new FileProcessor();
            var netClient = new NetworkClient();

            // Налаштування для RetryHelper
            var delay = TimeSpan.FromSeconds(1); // Починаємо з 1 секунди

            // --- СЦЕНАРІЙ 1: Робота з файлом ---
            Console.WriteLine("--- Сценарій 1: Читаємо замовлення з файлу ---");
            try
            {
                // Викликаємо метод через наш Хелпер
                var orders = RetryHelper.ExecuteWithRetry(
                    operation: () => fileProc.LoadOrderIds("data.txt"),
                    retryCount: 3,
                    initialDelay: delay,
                    shouldRetry: (ex) => ex is IOException // Повторюємо тільки якщо це IOException
                );

                Console.WriteLine("Результат: Отримано замовлень: " + string.Join(", ", orders));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Фатальна помилка: {ex.Message}");
            }

            Console.WriteLine("\n--------------------------------------------------\n");

            // --- СЦЕНАРІЙ 2: Робота з мережею ---
            Console.WriteLine("--- Сценарій 2: Тягнемо дані з API ---");
            try
            {
                var apiOrders = RetryHelper.ExecuteWithRetry(
                    operation: () => netClient.GetOrdersFromApi("https://api.kot-barsik.com/orders"),
                    retryCount: 4, // Даємо трохи більше спроб, бо інтернет - штука нестабільна
                    initialDelay: delay,
                    shouldRetry: (ex) => ex is HttpRequestException // Реагуємо на помилки мережі
                );

                Console.WriteLine("Результат: Отримано з API: " + string.Join(", ", apiOrders));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Фатальна помилка: {ex.Message}");
            }

            Console.WriteLine("\n=== Кінець роботи. ===");
        }
    }
}