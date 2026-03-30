using System;
using System.Collections.Generic;

namespace lab20
{
    public enum OrderStatus { New, PendingValidation, Processed, Shipped, Delivered, Cancelled }

    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Order(int id, string customerName, decimal totalAmount)
        {
            Id = id;
            CustomerName = customerName;
            TotalAmount = totalAmount;
            Status = OrderStatus.New;
        }
    }


    public interface IOrderValidator
    {
        bool IsValid(Order order);
    }

    public interface IOrderRepository
    {
        void Save(Order order);
        Order GetById(int id);
    }

    public interface IEmailService
    {
        void SendOrderConfirmation(Order order);
    }


    public class OrderValidator : IOrderValidator
    {
        public bool IsValid(Order order) => order.TotalAmount > 0;
    }

    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly Dictionary<int, Order> _database = new();

        public void Save(Order order)
        {
            _database[order.Id] = order;
            Console.WriteLine($"[DB] Замовлення #{order.Id} збережено в базу даних.");
        }

        public Order GetById(int id) => _database.ContainsKey(id) ? _database[id] : null;
    }

    public class ConsoleEmailService : IEmailService
    {
        public void SendOrderConfirmation(Order order)
        {
            Console.WriteLine($"[Email] Лист відправлено клієнту {order.CustomerName}: Замовлення #{order.Id} прийнято");
        }
    }


    public class OrderService
    {
        private readonly IOrderValidator _validator;
        private readonly IOrderRepository _repository;
        private readonly IEmailService _emailService;

        public OrderService(IOrderValidator validator, IOrderRepository repository, IEmailService emailService)
        {
            _validator = validator;
            _repository = repository;
            _emailService = emailService;
        }

        public void ProcessOrder(Order order)
        {
            Console.WriteLine($"Обробка замовлення #{order.Id}");

            if (!_validator.IsValid(order))
            {
                order.Status = OrderStatus.Cancelled;
                Console.WriteLine($"[Помилка] Замовлення #{order.Id} невалідне (Сума: {order.TotalAmount}).");
                return;
            }

            order.Status = OrderStatus.Processed;
            _repository.Save(order);
            _emailService.SendOrderConfirmation(order);

            Console.WriteLine($"[Успіх] Статус замовлення #{order.Id}: {order.Status}\n");
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var validator = new OrderValidator();
            var repository = new InMemoryOrderRepository();
            var emailService = new ConsoleEmailService();

            var orderService = new OrderService(validator, repository, emailService);

            var validOrder = new Order(1, "Владислав", 1500.50m);
            orderService.ProcessOrder(validOrder);

            // 4. Тест: Невалідне замовлення
            var invalidOrder = new Order(2, "Тестер", -50m);
            orderService.ProcessOrder(invalidOrder);

            Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}