using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Office
    {
        private Unit[] units;
        public Unit[] Units { get => units; }
        private List<Order> orders;
        public List<Order> Orders { get => orders; }
        // todo сейчас невозможно купить еду в кухню
        private OfficeKitchen kitchen;
        public OfficeKitchen Kitchen { get => kitchen; }
        private byte ordersMaxCount;
        public byte OrdersMaxCount { get => ordersMaxCount; }

        private static readonly byte ORDERS_MAX_COUNT_START = 2;

        public Office() : this(new Unit[1], new List<Order>(), new OfficeKitchen(), ORDERS_MAX_COUNT_START)
        {
            for (int i = 0; i < units.Length; i++)
            {
                units[i] = new Unit();
            }
        }

        public Office(Unit[] units, List<Order> orders, OfficeKitchen kitchen, byte ordersMaxCount)
        {
            this.units = units;
            this.orders = orders;
            this.kitchen = kitchen;
            this.ordersMaxCount = ordersMaxCount;
            GetNewOrders();
        }

#nullable enable
        public event EventObject<Office, Order>? OrderAdded;
        public event EventObject<Office, int>? OrderDeleted;
#nullable disable

        public void MakeWork(float modifiers)
        {
            Works works = new Works();
            foreach (Unit unit in units)
            {
                works += unit.MakeWork(ref kitchen, modifiers);
            }
            foreach (Order order in orders)
            {
                order.TransferWork(ref works);
            }
            for (int i = 0; i < orders.Count;)
            {
                if (orders[i].Completed)
                {
                    orders.RemoveAt(i);
                    OrderDeleted?.Invoke(this, i);
                }
                else
                {
                    i++;
                }
                    
            }
            GetNewOrders();
        }

        private void GetNewOrders()
        {
            OrderFactory factory = OrderFactory.Get();
            while (orders.Count < ordersMaxCount)
            {
                Order buff = factory.Create();
                orders.Add(buff);
                OrderAdded?.Invoke(this, buff);
            }
        }

        public void GetBuyFoodForKitchenCost()
        {

        }

        public void GetBuyFoodForKitchenPremiumCost()
        {

        }

        public void BuyFoodForKitchen()
        {

        }

        public void BuyFoodForKitchenPremium()
        {

        }

        private static readonly float UPGRADE_COST = 5;
        private static readonly float UPGRADE_EXP = 3;

        public ulong GetUpgradeOrdersMaxCountCost()
        {
            return (ulong)(Math.Pow(ordersMaxCount, UPGRADE_EXP) * UPGRADE_COST);
        }

        private static readonly float UPGRADE_COST_PREMIUM = 5;
        private static readonly float UPGRADE_EXP_PREMIUM = 3;

        public ulong GetUpgradeOrdersMaxCountPremiumCost()
        {
            return (ulong)(Math.Pow(ordersMaxCount, UPGRADE_EXP_PREMIUM) * UPGRADE_COST_PREMIUM);
        }

#nullable enable
        public event Event<Office>? Upgraded;
#nullable disable

        public void UpgradeOrdersMaxCount()
        {
            if (ordersMaxCount == byte.MaxValue)
                throw new MaxLevelException();
            GameModel.Get().TakeMoney(GetUpgradeOrdersMaxCountCost());
            ordersMaxCount++;
            Upgraded?.Invoke(this);
            GetNewOrders();
        }

        public void UpgradeOrdersMaxCountPremium()
        {
            if (ordersMaxCount == byte.MaxValue)
                throw new MaxLevelException();
            GameModel.Get().TakePremiumMoney(GetUpgradeOrdersMaxCountPremiumCost());
            ordersMaxCount++;
            Upgraded?.Invoke(this);
            GetNewOrders();
        }
    }
}
