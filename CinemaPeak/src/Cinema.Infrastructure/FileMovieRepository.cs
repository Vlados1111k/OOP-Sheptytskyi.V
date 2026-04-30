using System.Text.Json;
using Cinema.Domain;
using Cinema.Application.Interfaces;

namespace Cinema.Infrastructure;

public class FileMovieRepository : IRepository<Movie>
{
    private List<Movie> _movies = new();
    private readonly string _filePath = "movies.json";

    public IReadOnlyCollection<Movie> GetAll() => _movies.AsReadOnly();

    public void Add(Movie entity) => _movies.Add(entity);

    public async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(_movies);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task LoadAsync()
    {
        if (!File.Exists(_filePath)) return;
        var json = await File.ReadAllTextAsync(_filePath);
        _movies = JsonSerializer.Deserialize<List<Movie>>(json) ?? new List<Movie>();
    }
}