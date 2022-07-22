using System;
using System.Collections.Generic;

namespace MyGame
{
    public class Office : Upgradeable
    {
        private List<Unit> units;
        public List<Unit> Units { get => units; }
        private List<Order> orders;
        public List<Order> Orders { get => orders; }
        private Kitchen kitchen;
        public Kitchen Kitchen { get => kitchen; }
        private OfficePart[] parts;
        public OfficePart[] Parts { get => parts; }
        public OfficePart PartInternet { get => parts[(byte)OfficePart.OfficePartType.INTERNET]; }
        public OfficePart PartClimate { get => parts[(byte)OfficePart.OfficePartType.CLIMATE]; }
        public OfficePart PartMusic { get => parts[(byte)OfficePart.OfficePartType.MUSIC]; }

        public Office() : this(new List<Unit>(), new List<Order>(), new Kitchen(), new OfficePart[OfficePart.officePartTypeLength], Config.OFFICE_ORDERS_START_COUNT)
        {
            for (byte i = 0; i < OfficePart.officePartTypeLength; i++)
            {
                parts[i] = new OfficePart(i);
            }
        }

        public Office(List<Unit> units, List<Order> orders, Kitchen kitchen, OfficePart[] parts, ulong level) : base(level)
        {
            this.units = units;
            this.orders = orders;
            this.kitchen = kitchen;
            this.parts = parts;
        }

        public override ulong GetUpgradeCost()
        {
            return Convert.ToUInt64(Math.Pow(level, Config.OFFICE_UPGRADE_MONEY_COST_EXP) * Config.OFFICE_UPGRADE_MONEY_COST);
        }

        public ulong GetUnitCost()
        {
            return Convert.ToUInt64(Math.Pow(units.Count, Config.UNIT_MONEY_COST_EXP) * Config.UNIT_MONEY_COST);
        }

#nullable enable
        public event EventWith1Object<Office, Unit>? OnUnitBought;
#nullable disable

        public void BuyUnit(bool gameStart = false)
        {
            if (gameStart)
            {
                Unit buff = new Unit();
                units.Add(buff);
                OnUnitBought?.Invoke(this, buff);
            }
            else if (GameModel.Get().TakeMoney(GetUnitCost()))
            {
                Unit buff = new Unit();
                units.Add(buff);
                OnUnitBought?.Invoke(this, buff);
            }
        }

        public void MakeWork()
        {
            AddNewOrders();
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
        }

#nullable enable
        public event EventWith2Object<Office, Order, int>? OnOrderAdded;
        public event EventWith2Object<Office, Order, int>? OnOrderDeleted;
#nullable disable

        private void AddNewOrders()
        {
            OrderFactory factory = OrderFactory.Get();
            while (orders.Count < Convert.ToInt32(level))
            {
                Order buff = factory.Create();
                int i = orders.Count;
                orders.Add(buff);
                OnOrderAdded?.Invoke(this, buff, i);
            }
        }

        private void DeleteCompletedOrders()
        {
            for (int i = 0; i < orders.Count;)
            {
                if (orders[i].Completed)
                {
                    Order buff = orders[i];
                    orders.RemoveAt(i);
                    OnOrderDeleted?.Invoke(this, buff, i);
                }
                else
                {
                    i++;
                }
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

        public static ulong GetCost()
        {
            return Convert.ToUInt64(Math.Pow(GameModel.Get().Offices.Count, Config.OFFICE_MONEY_COST_EXP) * Config.OFFICE_MONEY_COST);
        }

        public override string ToString()
        {
            return "Office";
        }
    }
}
