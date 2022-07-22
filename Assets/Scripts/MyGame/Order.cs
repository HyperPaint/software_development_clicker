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
        private ulong premium;
        public ulong PremiumMoney { get => premium; }
        private bool completed;
        public bool Completed { get => completed; }

        public Order() : this ("", "", new Part(), new Part(), new Part(), new Part(), 1, 0, false) { }

        public Order(string name, string description, Part designing, Part art, Part programming, Part testing, ulong money, ulong premiumMoney, bool completed)
        {
            this.name = name;
            this.description = description;
            this.designing = designing;
            this.art = art;
            this.programming = programming;
            this.testing = testing;
            this.money = money;
            this.premium = premiumMoney;
            this.completed = completed;
        }

#nullable enable
        public event Event<Order>? OnDesigningCompleted;
        public event Event<Order>? OnArtCompleted;
        public event Event<Order>? OnProgrammingCompleted;
        public event Event<Order>? OnTestingCompleted;
        public event Event<Order>? OnOrderUpdated;
        public event Event<Order>? OnOrderCompleted;
#nullable disable

        public void TransferWork(ref Works works)
        {
            if (!designing.completed)
            {
                TransferWork(ref works.designing, ref designing, ref OnDesigningCompleted);
                TransferWork(ref works.fullstack, ref designing, ref OnDesigningCompleted);
                OnOrderUpdated?.Invoke(this);
                if (designing.completed)
                    TransferWork(ref works);
            }
            else if (!art.completed || !programming.completed)
            {
                if (!art.completed)
                {
                    TransferWork(ref works.art, ref art, ref OnArtCompleted);
                    TransferWork(ref works.fullstack, ref art, ref OnArtCompleted);
                    OnOrderUpdated?.Invoke(this);
                }
                if (!programming.completed)
                {
                    TransferWork(ref works.programming, ref programming, ref OnProgrammingCompleted);
                    TransferWork(ref works.fullstack, ref programming, ref OnProgrammingCompleted);
                    OnOrderUpdated?.Invoke(this);
                }
                if (art.completed && programming.completed)
                    TransferWork(ref works);
            }
            else if (!testing.completed)
            {
                TransferWork(ref works.testing, ref testing, ref OnTestingCompleted);
                TransferWork(ref works.fullstack, ref testing, ref OnTestingCompleted);
                OnOrderUpdated?.Invoke(this);
            }
            // сдача заказа требует одного тика
            else
            {
                completed = true;
                if (money > 0)
                {
                    GameModel.Get().PutMoney(money);
                }
                if (premium > 0)
                {
                    GameModel.Get().PutPremium(premium);
                }
                OnOrderCompleted?.Invoke(this);

                GameModel.Get().IncreaseReputation(Config.ORDER_REPUTATION_CHANGE_ON_COMPLETE);
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
