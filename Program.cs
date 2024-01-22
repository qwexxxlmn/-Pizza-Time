using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pizza_time
{
    // Сущность "Пицца"
    public class Pizza
    {
        public string Name { get; set; }
        public double Price { get; set; }
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
            Console.WriteLine($"{Name}забрал заказ. Приятного аппетита!");
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
            Console.WriteLine($"Заказ {order.OrderNumber} готов. Пожалуйста, заберите его!");
            order.IsReady = true;

            // Задержка перед тем, как пользователь заберет заказ
            Console.WriteLine("Ожидаем, пока гость заберет заказ...");
            Thread.Sleep(3000); // Задержка в миллисекундах (в данном случае 3 секунды)
        }
    }

    class Program
    {
        static void Main()
        {
            // Пример использования
            Pizzeria pizzeria = new Pizzeria();
            User user = new User { Name = "Гость " };

            // Добавим несколько видов пицц для выбора пользователем
            List<Pizza> menu = new List<Pizza>
            {
                new Pizza { Name = "1. Пепперони: ", Price = 549.00},
                new Pizza { Name = "2. Маргарита: ", Price = 349.49},
                new Pizza { Name = "3. Гавайская: ", Price = 799.99}
            };

            Console.WriteLine("Меню: ");
            foreach (var pizza in menu)
            {
                Console.WriteLine($"{pizza.Name} - {pizza.Price:F2}");
            }

            Console.WriteLine("Выберите пиццу из меню (введите номер): ");
            int pizzaChoice = int.Parse(Console.ReadLine());

            if (pizzaChoice < 1 || pizzaChoice > menu.Count)
            {
                Console.WriteLine("Неверный выбор пиццы.");
                Console.ReadLine();
                return;
            }

            Pizza selectedPizza = menu[pizzaChoice - 1];

            List<Pizza> pizzas = new List<Pizza> { selectedPizza };

            user.PlaceOrder(pizzeria, pizzas);
            Console.WriteLine($"Заказ на {selectedPizza.Name} успешно размещен. Номер заказа: {pizzeria.Orders.Last().OrderNumber}");

            // Задержка на приготовление пиццы в виде таймера
            Console.WriteLine("Готовим пиццу...");
            Thread.Sleep(5000); // Задержка в миллисекундах (в данном случае 5 секунд)

            Order readyOrder = pizzeria.Orders.Last();
            user.PickUpOrder(pizzeria, readyOrder);
            Console.ReadLine();
        }
    }
}
