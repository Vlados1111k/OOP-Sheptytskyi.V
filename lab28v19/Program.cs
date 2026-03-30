using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var repo = new VehicleRepository();
        string fileName = "vehicles.json";

        repo.Add(new Vehicle { Id = 1, Model = "Tesla Model 3", LicensePlate = "BК1234AI" });
        repo.Add(new Vehicle { Id = 2, Model = "BMW X5", LicensePlate = "ВК7777OO" });

        Console.WriteLine("Збереження даних у JSON:");
        await repo.SaveToFileAsync(fileName);

        Console.WriteLine("Завантаження даних з файлу:");
        await repo.LoadFromFileAsync(fileName);

        foreach (var v in repo.GetAll())
        {
            Console.WriteLine($"ID: {v.Id}, Модель: {v.Model}, Номер: {v.LicensePlate}");
        }
    }
}


public class Vehicle
{
    public int Id { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }
}

public class Route
{
    public int Id { get; set; }
    public string StartPoint { get; set; }
    public string EndPoint { get; set; }
}


public class VehicleRepository
{
    private List<Vehicle> _vehicles = new List<Vehicle>();

    public void Add(Vehicle vehicle) => _vehicles.Add(vehicle);
    public List<Vehicle> GetAll() => _vehicles;
    public Vehicle GetById(int id) => _vehicles.FirstOrDefault(v => v.Id == id);

    public async Task SaveToFileAsync(string filename)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(_vehicles, options);
        await File.WriteAllTextAsync(filename, json);
    }

    public async Task LoadFromFileAsync(string filename)
    {
        if (File.Exists(filename))
        {
            string json = await File.ReadAllTextAsync(filename);
            _vehicles = JsonSerializer.Deserialize<List<Vehicle>>(json) ?? new List<Vehicle>();
        }
    }
}