using MyGame;
using System;
using System.Collections.Generic;

namespace MyGame
{
    public class GameModel
    {
        private static object mutex = new();
        private static volatile GameModel model;

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
        private List<Office> offices;
        public List<Office> Offices { get => offices; }
        private ulong money;
        public ulong Money { get => money; }
        private ulong premiumMoney;
        public ulong PremiumMoney { get => premiumMoney; }
        private ulong reputation; // todo репутация не используется
        public ulong Reputation { get => reputation; }

        private GameModel()
        {
            // загрузка игровых данных

            // игровых данных не найдено, начальные игровые данные
            level = GameStage.GARAGE;
            offices = new List<Office>();
            offices.Add(new Office());
            offices[0].Units[0].WorkPlaces[0].Worker = new Worker();
            money = 0;
            premiumMoney = 100;

            Upgraded += (sender) =>
            {
                Logger.Get().Log("Игровая стадия улучшена до " + level.ToString());
            };
        }

        public void MakeWork()
        {
            foreach (Office office in offices)
            {
                office.MakeWork(1f);
            }
        }

        private static readonly float UPGRADE_COST = 5;
        private static readonly float UPGRADE_EXP = 3;

        public ulong GetUpgradeCost()
        {
            return (ulong)(Math.Pow((double)level, UPGRADE_EXP) * UPGRADE_COST);
        }

        private static readonly float UPGRADE_COST_PREMIUM = 5;
        private static readonly float UPGRADE_EXP_PREMIUM = 3;

        public ulong GetUpgradePremiumCost()
        {
            return (ulong)(Math.Pow((double)level, UPGRADE_EXP_PREMIUM) * UPGRADE_COST_PREMIUM);
        }

#nullable enable
        public event Event? Upgraded;
#nullable disable

        public void Upgrade()
        {
            if (level == GameStage.GARAGE)
                throw new MaxLevelException();
            TakeMoney(GetUpgradeCost());
            level++;
            Upgraded?.Invoke(this);
        }

        public void UpgradePremium()
        {
            if (level == GameStage.GARAGE)
                throw new MaxLevelException();
            TakePremiumMoney(GetUpgradePremiumCost());
            level++;
            Upgraded?.Invoke(this);
        }

        /// <summary>
        /// Функция для добавления валюты в банк.
        /// </summary>
        /// <param name="money">Добавляемое количество валюты</param>
        public void PutMoney(ulong money)
        {
            this.money += money;
        }

        /// <summary>
        /// Функция для проверки и получения валюты из банка.
        /// При достаточном балансе функция уничтожит необходимое количество валюты. 
        /// При недостаточном балансе функция выбросит исключение.
        /// </summary>
        /// <param name="money">Необходимое количество валюты</param>
        /// <exception cref="NoMoneyException">Бросается при недостаточном балансе</exception>
        public void TakeMoney(ulong money)
        {
            if (this.money >= money)
            {
                this.money -= money;
                return;
            }
            throw new NoMoneyException();
        }

        /// <summary>
        /// Функция для добавления премиум валюты в банк.
        /// </summary>
        /// <param name="money">Добавляемое количество премиум валюты</param>
        public void PutPremiumMoney(ulong money)
        {
            premiumMoney += money;
        }

        /// <summary>
        /// Функция для проверки и получения премиум валюты из банка.
        /// При достаточном балансе функция вернёт необходимое количество премиум валюты. 
        /// При недостаточном балансе функция выбросит исключение.
        /// </summary>
        /// <param name="money">Необходимое количество премиум валюты</param>
        /// <exception cref="NoMoneyException">Бросается при недостаточном балансе</exception>
        public void TakePremiumMoney(ulong money)
        {
            if (premiumMoney >= money)
            {
                premiumMoney -= money;
                return;
            }
            throw new NoMoneyException();
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


