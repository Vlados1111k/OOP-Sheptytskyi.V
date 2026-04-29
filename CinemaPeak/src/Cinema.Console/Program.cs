using Cinema.Domain;

Console.WriteLine("=== CinemaPeak System v0.1 ===");

var movie = new Movie { Title = "Початок (Inception)", Genre = "Sci-Fi", DurationMinutes = 148 };

Console.WriteLine($"Система готова! Перший фільм у базі: {movie.Title} ({movie.Genre})");
Console.ReadLine();