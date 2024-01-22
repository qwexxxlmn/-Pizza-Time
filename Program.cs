using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaTime
{
    // Сущность "Пицца"
    public class Pizza
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    // Сущность "Заказ"
    public class Order
    {
        public int OrderNumber { get; set; }
        public string UserName { get; set; }
        public List<Pizza> Pizzas { get; set; }
        public bool IsReady { get; set; }
    }

    // Сущность "Пользователь"
    public class User
    {
        public string Name { get; set; }

        public void PlaceOrder(Pizzeria pizzeria, List<Pizza> pizzas)
        {
            Order order = pizzeria.TakeOrder(this, pizzas);
            Console.WriteLine($"Заказ успешно размещен. Номер заказа: {order.OrderNumber}");
        }

        public void PickUpOrder(Pizzeria pizzeria, Order order)
        {
            pizzeria.NotifyUser(order);
            Console.WriteLine($"{Name} забрал заказ. Приятного аппетита!");
        }
    }

    // Сущность "Пиццерия"
    public class Pizzeria
    {
        private int orderCounter = 1;

        public List<Order> Orders { get; } = new List<Order>();

        public Order TakeOrder(User user, List<Pizza> pizzas)
        {
            Order order = new Order
            {
                OrderNumber = orderCounter++,
                UserName = user.Name,
                Pizzas = pizzas,
                IsReady = false
            };

            Orders.Add(order);
            return order;
        }

        public void NotifyUser(Order order)
        {
            Console.WriteLine($"Заказ {order.OrderNumber} готов. Пожалуйста, заберите его, {order.UserName}!");
            order.IsReady = true;
        }
    }

    class Program
    {
        static void Main()
        {
            // Пример использования
            Pizzeria pizzeria = new Pizzeria();
            User user = new User { Name = "Иван" };
            List<Pizza> pizzas = new List<Pizza> { new Pizza { Name = "Пепперони", Price = 10.99m } };

            user.PlaceOrder(pizzeria, pizzas);
            // Здесь могло бы быть ожидание оповещения о готовности заказа

            Order readyOrder = pizzeria.Orders[0];
            user.PickUpOrder(pizzeria, readyOrder);
            Console.ReadLine();
        }
    }
}