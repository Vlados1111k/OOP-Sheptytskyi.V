namespace Cinema.Domain;

public class CinemaHall {
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int TotalSeats => Rows * Columns;
    public int Rows { get; set; }
    public int Columns { get; set; }
}

public class MovieSession {
    public int Id { get; set; }
    public Movie Movie { get; set; } = null!;
    public CinemaHall Hall { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public decimal BasePrice { get; set; }
}