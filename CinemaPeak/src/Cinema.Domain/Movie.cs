namespace Cinema.Domain;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }

    public override string ToString()
    {
        return $"[{Id}] {Title} | Жанр: {Genre} | Тривалість: {DurationMinutes} хв";
    }
}