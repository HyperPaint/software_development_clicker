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
        public event EventWith1Object<OrderFactory, Order>? OnOrderCreated;
#nullable disable

        public Order Create(bool useReputation = true)
        {
            Random random = GameModel.Random;
            int buff = random.Next(0, Config.ORDER_FACTORY_ORDER_NAME.Length - 1);
            string name = Config.ORDER_FACTORY_ORDER_NAME[buff];
            string description = Config.ORDER_FACTORY_ORDER_DESCRIPTION[buff];
            // todo части заказа должны зависеть от репутации
            Order.Part designing = CreateOrderPart(useReputation);
            Order.Part art = CreateOrderPart(useReputation);
            Order.Part programming = CreateOrderPart(useReputation);
            Order.Part testing = CreateOrderPart(useReputation);
            float neededValue = designing.needed + art.needed + programming.needed + testing.needed;
            ulong money = Convert.ToUInt64(MathF.Pow(neededValue, Config.ORDER_FACTORY_ORDER_PART_MONEY_PER_NEEDED_EXP) * Config.ORDER_FACTORY_ORDER_PART_MONEY_PER_NEEDED);
            ulong premiumMoney = random.Next(1, 100) <= Config.ORDER_FACTORY_ORDER_PART_PREMIUM_CHANCE ? Convert.ToUInt64(MathF.Pow(neededValue, Config.ORDER_FACTORY_ORDER_PART_PREMIUM_PER_NEEDED_EXP) * Config.ORDER_FACTORY_ORDER_PART_PREMIUM_PER_NEEDED) : 0;

            Order order = new Order(name, description, designing, art, programming, testing, money, premiumMoney, false);
            OnOrderCreated?.Invoke(this, order);
            return order;
        }

        private Order.Part CreateOrderPart(bool useReputation = true)
        {
            Random random = GameModel.Random;
            ulong start = Config.ORDER_FACTORY_ORDER_PART_START, end = Config.ORDER_FACTORY_ORDER_PART_END;
            if (useReputation)
            {
                long reputationModifier = Convert.ToInt64(GameModel.Get().Reputation * Config.WORKER_FACTORY_WORKER_SKILL_REPUTATION_MODIFIER);
                if (reputationModifier >= 0)
                {
                    start += Convert.ToUInt64(reputationModifier);
                    end += Convert.ToUInt64(reputationModifier);
                }
                else
                {
                    end = Config.WORKER_FACTORY_WORKER_SKILL_END;
                }
            }
            return new Order.Part(0, Convert.ToUInt64(random.Next(Convert.ToInt32(start), Convert.ToInt32(end))), false);
        }
    }
}
