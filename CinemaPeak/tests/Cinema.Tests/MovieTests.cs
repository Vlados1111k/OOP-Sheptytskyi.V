using Cinema.Domain;
using Xunit;

namespace Cinema.Tests;

public class MovieTests
{
    [Fact]
    public void Movie_ShouldHaveCorrectTitle()
    {
        var movie = new Movie { Title = "Бійцівський клуб", Genre = "Драма" };

        Assert.Equal("Бійцівський клуб", movie.Title);
    }

    [Fact]
    public void Movie_ToString_ReturnsExpectedFormat()
    {
        var movie = new Movie { Id = 1, Title = "Inception", Genre = "Sci-Fi", DurationMinutes = 148 };

        var result = movie.ToString();

        Assert.Contains("Inception", result);
    }
}