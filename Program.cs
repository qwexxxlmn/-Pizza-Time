using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        public int OrderNumber { get; set; }

        public void PlaceOrder(Pizzeria pizzeria, List<Pizza> pizzas)
        {
            Order order = pizzeria.TakeOrder(this, pizzas);
            OrderNumber = order.OrderNumber;
            Console.WriteLine($"Заказ успешно размещен. Номер заказа: {OrderNumber}");
        }

        public void PickUpOrder(Pizzeria pizzeria)
        {
            Order readyOrder = pizzeria.FindOrder(OrderNumber);

            if (readyOrder != null)
            {
                pizzeria.NotifyUser(readyOrder);
                Console.WriteLine($"Заказ №{OrderNumber} забран. Приятного аппетита!");
            }
            else
            {
                Console.WriteLine($"Заказ №{OrderNumber} не найден.");
            }
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
                UserName = $"Гость",
                Pizzas = pizzas,
                IsReady = false
            };

            Orders.Add(order);
            return order;
        }

        public Order FindOrder(int orderNumber)
        {
            return Orders.FirstOrDefault(order => order.OrderNumber == orderNumber);
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
            Pizzeria pizzeria = new Pizzeria();

            while (true)
            {
                User user = new User();

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

                Console.WriteLine("Выберите пиццы из меню (введите номера через запятую): ");
                string pizzaChoices = Console.ReadLine();
                List<int> selectedPizzaIndices = pizzaChoices.Split(',').Select(int.Parse).ToList();

                List<Pizza> selectedPizzas = selectedPizzaIndices
                    .Where(index => index > 0 && index <= menu.Count)
                    .Select(index => menu[index - 1])
                    .ToList();

                if (selectedPizzas.Count > 0)
                {
                    user.PlaceOrder(pizzeria, selectedPizzas);
                    Console.WriteLine($"Заказ успешно создан.");

                    // Задержка на приготовление пиццы в виде таймера
                    Console.WriteLine("Готовим пиццу...");
                    Thread.Sleep(5000); // Задержка в миллисекундах (в данном случае 5 секунд)

                    user.PickUpOrder(pizzeria);
                }
                else
                {
                    Console.WriteLine("Неверный выбор пиццы.");
                }

                Console.WriteLine("Желаете разместить еще один заказ? (Y/N)");
                string continueOrdering = Console.ReadLine();

                if (continueOrdering.ToUpper() != "Y")
                    break;
            }
        }
    }
}