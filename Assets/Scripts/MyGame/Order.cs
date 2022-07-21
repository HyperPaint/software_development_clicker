namespace MyGame
{
    public class Order
    {
        public struct Part
        {
            public ulong current;
            public ulong needed;
            public bool completed;

            public Part(ulong current, ulong needed, bool completed)
            {
                this.current = current;
                this.needed = needed;
                this.completed = completed;
            }
        }

        private string name;
        public string Name { get => name; }
        private string description;
        public string Description { get => description; }
        private uint icon;
        public uint Icon { get => icon; }
        private Part designing;
        public Part Designing { get => designing; }
        private Part art;
        public Part Art { get => art; }
        private Part programming;
        public Part Programming { get => programming; }
        private Part testing;
        public Part Testing { get => testing; }
        private ulong money;
        public ulong Money { get => money; }
        private ulong premiumMoney;
        public ulong PremiumMoney { get => premiumMoney; }
        private bool completed;
        public bool Completed { get => completed; }

        public Order() : this ("", "", 0, new Part(), new Part(), new Part(), new Part(), 1, 0, false) { }

        public Order(string name, string description, uint icon, Part designing, Part art, Part programming, Part testing, ulong money, ulong premiumMoney, bool completed)
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
        public event Event<Order>? DesigningCompleted;
        public event Event<Order>? ArtCompleted;
        public event Event<Order>? ProgrammingCompleted;
        public event Event<Order>? TestingCompleted;
        public event Event<Order>? OrderUpdated;
        public event Event<Order>? OrderCompleted;
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
            }
            // сдача заказа требует одного тика
            else
            {
                completed = true;
                GameModel.Get().PutMoney(money);
                GameModel.Get().PutPremium(premiumMoney);
                OrderCompleted?.Invoke(this);
            }
        }

        private void TransferWork(ref ulong work, ref Part orderPart, ref Event<Order> @event) {
            // перевожу работу в часть
            orderPart.current += work;
            // если часть выполнена
            if (orderPart.current >= orderPart.needed)
            {
                // возращаю остаток работы
                work = orderPart.current - orderPart.needed;
                orderPart.current = orderPart.needed;
                orderPart.completed = true;
                @event?.Invoke(this);
            }
            // часть не выполнена
            else
            {
                // забираю работу
                work = 0;
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
