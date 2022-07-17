﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class OrderFactory
    {
        private static object mutex = new();
        private static volatile OrderFactory factory;

        /// <summary>
        /// Функция для получения единственного объекта фабрики.
        /// Можно безопасно использовать с любого потока.
        /// </summary>
        /// <returns>Фабрика</returns>
        public static OrderFactory Get()
        {
            if (factory == null)
            {
                lock (mutex)
                {
                    if (factory == null)
                    {
                        factory = new OrderFactory();
                    }
                }
            }
            return factory;
        }

        private OrderFactory()
        {
            OrderCreated += (sender, @object) =>
            {
                Order order = (Order)@object;
                Logger.Get().Log("Заказ \"" + order.ToString() + "\" создан");
            };
        }

        private readonly string[] names = {
            "Создание игор",
            "Симулятор жизни абрикоса",
            "Шариковая ручка",
            "Банзай - ну што пацаны погнали далеко?",
            "Банзай 2 - некрасивый рот!",
            "Шашлык - кулинарный сборник",
        };

#nullable enable
        public event EventFactory? OrderCreated;
#nullable disable

        private static readonly float MONEY_PER_NEEDED = 1f;
        private static readonly float MONEY_PER_NEEDED_EXP = 0.5f;

        private static readonly byte MONEY_PREMIUM_CHANCE = 1;
        private static readonly float MONEY_PREMIUM_PER_NEEDED = 0.5f;
        private static readonly float MONEY_PREMIUM_PER_NEEDED_EXP = 0.25f;

        public Order Create()
        {
            Random random = GameModel.Random;
            string name = names[random.Next(0, names.Length - 1)];
            string description = "";
            uint icon = 0;
            // todo части заказа должны точно от чего-то зависеть
            OrderPart designing = new OrderPart(0, (ulong)(random.Next(1, 5) * 100), false);
            OrderPart art = new OrderPart(0, (ulong)(random.Next(1, 5) * 100), false);
            OrderPart programming = new OrderPart(0, (ulong)(random.Next(1, 5) * 100), false);
            OrderPart testing = new OrderPart(0, (ulong)(random.Next(1, 5) * 100), false);
            float neededValue = designing.needed + art.needed + programming.needed + testing.needed;
            ulong money = (ulong)(MathF.Pow(neededValue, MONEY_PER_NEEDED_EXP) * MONEY_PER_NEEDED);
            ulong premiumMoney = random.Next(1, 100) <= MONEY_PREMIUM_CHANCE ? (ulong)(MathF.Pow(neededValue, MONEY_PREMIUM_PER_NEEDED_EXP) * MONEY_PREMIUM_PER_NEEDED) : 0;

            Order order = new Order(name, description, icon, designing, art, programming, testing, money, premiumMoney, false);
            OrderCreated?.Invoke(this, order);
            return order;
        }
    }
}