using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Order
    {
        private string name;
        public string Name { get => name; }
        private string description;
        public string Description { get => description; }
        private uint icon;
        public uint Icon { get => icon; }
        private OrderPart designing;
        public OrderPart Designing { get => designing; }
        private OrderPart art;
        public OrderPart Art { get => art; }
        private OrderPart programming;
        public OrderPart Programming { get => programming; }
        private OrderPart testing;
        public OrderPart Testing { get => testing; }
        private ulong money;
        public ulong Money { get => money; }
        private ulong premiumMoney;
        public ulong PremiumMoney { get => premiumMoney; }
        private bool completed;
        public bool Completed { get => completed; }

        public Order() : this ("", "", 0, new OrderPart(), new OrderPart(), new OrderPart(), new OrderPart(), 1, 0, false) { }

        public Order(string name, string description, uint icon, OrderPart designing, OrderPart art, OrderPart programming, OrderPart testing, ulong money, ulong premiumMoney, bool completed)
        {
            this.name = name;
            this.description = description;
            this.icon = icon;
            this.designing = designing;
            this.art = art;
            this.programming = programming;
            this.testing = testing;
            this.money = money;
            this.premiumMoney = premiumMoney;
            this.completed = completed;
        }

#nullable enable
        public event Event? OrderUpdated;
        public event Event? DesigningCompleted;
        public event Event? ArtCompleted;
        public event Event? ProgrammingCompleted;
        public event Event? TestingCompleted;
        public event Event? OrderCompleted;
#nullable disable

        public void TransferWork(ref Works works)
        {
            if (!designing.completed)
            {
                TransferWork(ref works.designing, ref designing, ref DesigningCompleted);
                TransferWork(ref works.fullstack, ref designing, ref DesigningCompleted);
                OrderUpdated?.Invoke(this);
                if (designing.completed)
                    TransferWork(ref works);
            }
            else if (!art.completed || !programming.completed)
            {
                if (!art.completed)
                {
                    TransferWork(ref works.art, ref art, ref ArtCompleted);
                    TransferWork(ref works.fullstack, ref art, ref ArtCompleted);
                    OrderUpdated?.Invoke(this);
                }
                if (!programming.completed)
                {
                    TransferWork(ref works.programming, ref programming, ref ProgrammingCompleted);
                    TransferWork(ref works.fullstack, ref programming, ref ProgrammingCompleted);
                    OrderUpdated?.Invoke(this);
                }
                if (art.completed && programming.completed)
                    TransferWork(ref works);
            }
            else if (!testing.completed)
            {
                TransferWork(ref works.testing, ref testing, ref TestingCompleted);
                TransferWork(ref works.fullstack, ref testing, ref TestingCompleted);
                OrderUpdated?.Invoke(this);
                if (testing.completed)
                    TransferWork(ref works);
            }
            else
            {
                completed = true;
                GameModel.Get().PutMoney(money);
                GameModel.Get().PutPremiumMoney(premiumMoney);
                OrderCompleted?.Invoke(this);
            }
        }

        private void TransferWork(ref ulong work, ref OrderPart orderPart, ref Event @event) {
            // перевожу работу в часть заказа
            orderPart.current += work;
            // если часть заказа выполнена
            if (orderPart.current >= orderPart.needed)
            {
                // возращаю остаток работы
                work = orderPart.current - orderPart.needed;
                orderPart.current = orderPart.needed;
                orderPart.completed = true;
                @event?.Invoke(this);
            }
            // не выполнена
            else
            {
                // забираю всю работу
                work = 0;
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
