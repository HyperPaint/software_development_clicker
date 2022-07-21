using System;
using System.Collections.Generic;

namespace MyGame
{
    public class GameModel
    {
        private static object mutex = new();
        private static volatile GameModel model;
        private static volatile Logger logger;

        /// <summary>
        /// Функция для получения единственного объекта модели.
        /// Можно безопасно использовать с любого потока.
        /// </summary>
        /// <returns>Модель</returns>
        public static GameModel Get()
        {
            if (model == null)
            {
                lock (mutex)
                {
                    if (model == null)
                    {
                        model = new GameModel();
                    }
                }
            }
            return model;
        }

        private static Random random = new Random();
        public static Random Random { get => random; }

        public enum GameStage : byte
        {
            GARAGE = 1,
            OFFICE = 2,
            BUILDING = 3,
        }

        private GameStage level;
        public GameStage Level { get => level; }
        private List<Office> offices = new List<Office>();
        public List<Office> Offices { get => offices; }
        private ulong money;
        public ulong Money { get => money; }
        private ulong premiumMoney;
        public ulong PremiumMoney { get => premiumMoney; }
        private ulong reputation; // todo репутация не используется
        public ulong Reputation { get => reputation; }

        private GameModel()
        {
            logger = new Logger(this);

            // загрузка игровых данных

            // игровых данных не найдено, начальные игровые данные
            level = GameStage.GARAGE;
            offices.Add(new Office());
            offices[0].Units[0].Workplaces[0].Worker = WorkerFactory.Get().Create(Worker.EmployeeType.FULLSTACK); ;
            money = 0;
            premiumMoney = 100;
        }

#nullable enable
        public event Event<GameModel>? Upgraded;
#nullable disable

        public void BuyUpgrade()
        {
            TakeMoney(GetUpgradeCost());
            if (level < GameStage.BUILDING)
            {
                level++;
            }
            else
            {
                throw new MaxLevelException();
            }
            Upgraded?.Invoke(this);
        }

        public ulong GetUpgradeCost()
        {
            return Convert.ToUInt64(Math.Pow((double)level, Config.GAME_MODEL_UPGRADE_EXP) * Config.GAME_MODEL_UPGRADE_COST);
        }

        public void MakeWork()
        {
            foreach (Office office in offices)
            {
                office.MakeWork();
            }
        }

        /// <summary>
        /// Функция для добавления валюты в банк.
        /// </summary>
        /// <param name="money">Добавляемое количество валюты</param>
        public void PutMoney(ulong money)
        {
            this.money += money;
        }

#nullable enable
        public event EventWith1Object<GameModel, ulong>? NotEnoughMoney;
#nullable disable

        /// <summary>
        /// Функция для проверки и получения валюты из банка.
        /// При достаточном балансе функция уничтожит необходимое количество валюты. 
        /// При недостаточном балансе функция выбросит исключение.
        /// </summary>
        /// <param name="money">Необходимое количество валюты</param>
        /// <exception cref="NotEnoughCurrencyException">Бросается при недостаточном балансе</exception>
        public void TakeMoney(ulong money)
        {
            if (this.money >= money)
            {
                this.money -= money;
                return;
            }
            NotEnoughMoney?.Invoke(this, money - this.money);
        }

        /// <summary>
        /// Функция для добавления премиум валюты в банк.
        /// </summary>
        /// <param name="money">Добавляемое количество премиум валюты</param>
        public void PutPremium(ulong money)
        {
            premiumMoney += money;
        }

#nullable enable
        public event EventWith1Object<GameModel, ulong>? NotEnoughPremium;
#nullable disable

        /// <summary>
        /// Функция для проверки и получения премиум валюты из банка.
        /// При достаточном балансе функция вернёт необходимое количество премиум валюты. 
        /// При недостаточном балансе функция выбросит исключение.
        /// </summary>
        /// <param name="money">Необходимое количество премиум валюты</param>
        /// <exception cref="NotEnoughCurrencyException">Бросается при недостаточном балансе</exception>
        public void TakePremium(ulong money)
        {
            if (premiumMoney >= money)
            {
                premiumMoney -= money;
                return;
            }
            NotEnoughPremium?.Invoke(this, money - this.money);
        }

        public void IncreaseReputation()
        {
            if (reputation < ulong.MaxValue)
            {
                reputation++;
            }
        }

        public void DecreaseReputation()
        {
            if (reputation > 0)
            {
                reputation--;
            }
        }
    }
}


