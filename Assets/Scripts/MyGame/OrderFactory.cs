using System;
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

        private OrderFactory() { }

#nullable enable
        public event EventWith1Object<OrderFactory, Order>? OrderCreated;
#nullable disable

        public Order Create()
        {
            Random random = GameModel.Random;
            int buff = random.Next(0, Config.ORDER_FACTORY_ORDER_NAME.Length - 1);
            string name = Config.ORDER_FACTORY_ORDER_NAME[buff];
            string description = Config.ORDER_FACTORY_ORDER_DESCRIPTION[buff];
            // todo иконки заказа
            uint icon = 0;
            // todo части заказа должны зависеть от репутации
            Order.Part designing = new(0, (ulong)(random.Next(1, 5) * 100), false);
            Order.Part art = new(0, (ulong)(random.Next(1, 5) * 100), false);
            Order.Part programming = new(0, (ulong)(random.Next(1, 5) * 100), false);
            Order.Part testing = new(0, (ulong)(random.Next(1, 5) * 100), false);
            float neededValue = designing.needed + art.needed + programming.needed + testing.needed;
            ulong money = Convert.ToUInt64(MathF.Pow(neededValue, Config.ORDER_FACTORY_ORDER_PART_MONEY_PER_NEEDED_EXP) * Config.ORDER_FACTORY_ORDER_PART_MONEY_PER_NEEDED);
            ulong premiumMoney = random.Next(1, 100) <= Config.ORDER_FACTORY_ORDER_PART_PREMIUM_CHANCE ? Convert.ToUInt64(MathF.Pow(neededValue, Config.ORDER_FACTORY_ORDER_PART_PREMIUM_PER_NEEDED_EXP) * Config.ORDER_FACTORY_ORDER_PART_PREMIUM_PER_NEEDED) : 0;

            Order order = new Order(name, description, icon, designing, art, programming, testing, money, premiumMoney, false);
            OrderCreated?.Invoke(this, order);
            return order;
        }
    }
}
