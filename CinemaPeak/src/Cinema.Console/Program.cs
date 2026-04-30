using System;
using System.Linq;
using Cinema.Domain;
using Cinema.Infrastructure;

var repo = new FileMovieRepository();

await repo.LoadAsync();

while (true)
{
    Console.Clear();
    Console.WriteLine("CinemaPeak Management System (Ітерація 2)");
    Console.WriteLine("1. Показати всі фільми");
    Console.WriteLine("2. Додати фільм");
    Console.WriteLine("3. Пошук (LINQ): Тільки Sci-Fi");
    Console.WriteLine("4. Зберегти та вийти");
    Console.Write("\nОберіть опцію: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.WriteLine("\nСписок фільмів");
            foreach (var m in repo.GetAll()) Console.WriteLine(m);
            break;

        case "2":
            Console.Write("Назва: "); var title = Console.ReadLine();
            Console.Write("Жанр: "); var genre = Console.ReadLine();
            Console.Write("Тривалість (хв): "); int.TryParse(Console.ReadLine(), out int dur);
            
            repo.Add(new Movie { 
                Id = repo.GetAll().Count + 1, 
                Title = title, 
                Genre = genre, 
                DurationMinutes = dur 
            });
            break;

        case "3":
            var sciFiMovies = repo.GetAll()
                .Where(m => m.Genre.Equals("Sci-Fi", StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            Console.WriteLine("\n--- Знайдені Sci-Fi фільми ---");
            sciFiMovies.ForEach(Console.WriteLine);
            break;

        case "4":
            await repo.SaveAsync();
            Console.WriteLine("Дані збережено у movies.json. До зустрічі!");
            return;
    }
    Console.WriteLine("\nНатисніть Enter для продовження");
    Console.ReadLine();
}