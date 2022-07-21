using System;
using System.Collections.Generic;

namespace MyGame
{
    public class Office : Upgradeable
    {
        private Unit[] units;
        public Unit[] Units { get => units; }
        private List<Order> orders;
        public List<Order> Orders { get => orders; }
        private Kitchen kitchen;
        public Kitchen Kitchen { get => kitchen; }
        private OfficePart[] parts;
        public OfficePart[] Parts { get => parts; }
        public OfficePart PartInternet { get => parts[(byte)OfficePart.OfficePartType.INTERNET]; }
        public OfficePart PartClimate { get => parts[(byte)OfficePart.OfficePartType.CLIMATE]; }
        public OfficePart PartMusic { get => parts[(byte)OfficePart.OfficePartType.MUSIC]; }

        public Office() : this(new Unit[Config.OFFICE_UNITS_START_COUNT], new List<Order>(), new Kitchen(), new OfficePart[OfficePart.officePartTypeLength], Config.OFFICE_ORDERS_START_COUNT)
        {
            for (int i = 0; i < units.Length; i++)
            {
                units[i] = new Unit();
            }
            for (byte i = 0; i < OfficePart.officePartTypeLength; i++)
            {
                parts[i] = new OfficePart(i);
            }
            AddNewOrders();
        }

        public Office(Unit[] units, List<Order> orders, Kitchen kitchen, OfficePart[] parts, byte level) : base(level)
        {
            this.units = units;
            this.orders = orders;
            this.kitchen = kitchen;
            this.parts = parts;
        }

        public override ulong GetUpgradeCost()
        {
            return Convert.ToUInt64(Math.Pow(level, Config.OFFICE_UPGRADE_EXP) * Config.OFFICE_UPGRADE_COST);
        }

        public void MakeWork()
        {
            Works works = new Works();
            foreach (Unit unit in units)
            {
                works += unit.MakeWork(kitchen, GetModifiers());
            }
            foreach (Order order in orders)
            {
                order.TransferWork(ref works);
            }
            DeleteCompletedOrders();
            AddNewOrders();
        }

#nullable enable
        public event EventWith2Object<Office, Order, int>? OrderAdded;
        public event EventWith2Object<Office, Order, int>? OrderDeleted;
#nullable disable

        private void DeleteCompletedOrders()
        {
            for (int i = 0; i < orders.Count;)
            {
                if (orders[i].Completed)
                {
                    Order buff = orders[i];
                    orders.RemoveAt(i);
                    OrderDeleted?.Invoke(this, buff, i);
                }
                else
                {
                    i++;
                }
            }
        }

        private void AddNewOrders()
        {
            OrderFactory factory = OrderFactory.Get();
            while (orders.Count < level)
            {
                Order buff = factory.Create();
                int i = orders.Count;
                orders.Add(buff);
                OrderAdded?.Invoke(this, buff, i);
            }
        }

        public float GetModifiers()
        {
            float value = Config.Base.MODIFIER_BASE;
            for (byte i = 0; i < OfficePart.officePartTypeLength; i++)
            {
                value *= parts[i].GetModifier();
            }
            return value;
        }
    }
}
