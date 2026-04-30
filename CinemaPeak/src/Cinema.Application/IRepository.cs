namespace Cinema.Application.Interfaces;

public interface IRepository<T>
{
    IReadOnlyCollection<T> GetAll();
    void Add(T entity);
    Task SaveAsync();
    Task LoadAsync();
}