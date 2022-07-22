using System;
using System.Collections.Generic;

namespace MyGame
{
    public class GameModel
    {
        private static object mutex = new();
        private static volatile GameModel model;
        private readonly Logger logger;

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
        private ulong premium;
        public ulong Premium { get => premium; }
        private long reputation; // todo репутация не используется
        public long Reputation { get => reputation; }

        private GameModel()
        {
            if (Config.LOGGER_ENABLED)
            {
                logger = new Logger(this);
            }

            // загрузка игровых данных

            // игровых данных не найдено, начальные игровые данные
            level = GameStage.GARAGE;
            BuyOffice(true);
            offices[0].BuyUnit(true);
            offices[0].Units[0].Workplaces[0].BuyWorker(WorkerFactory.Get().Create(Worker.EmployeeType.FULLSTACK, false), true);
            money = 0;
            premium = 100;
            reputation = 0;
        }

        public ulong GetUpgradeCost()
        {
            return Convert.ToUInt64(Math.Pow((double)level, Config.GAME_MODEL_UPGRADE_MONEY_COST_EXP) * Config.GAME_MODEL_UPGRADE_MONEY_COST);
        }

#nullable enable
        public event Event<GameModel>? OnGameStageUpgraded;
#nullable disable

        public void BuyUpgrade()
        {
            if (level < GameStage.BUILDING)
            {
                TakeMoney(GetUpgradeCost());
                level++;
                OnGameStageUpgraded?.Invoke(this);
                return;
            }
            throw new MaxLevelException();
        }

#nullable enable
        public event EventWith1Object<GameModel, Office>? OnOfficeBought;
#nullable disable

        public void BuyOffice(bool gameStart = false)
        {
            if (gameStart)
            {
                Office buff = new Office();
                offices.Add(buff);
                OnOfficeBought?.Invoke(this, buff);
            }
            else if (TakeMoney(Office.GetCost()))
            {
                Office buff = new Office();
                offices.Add(buff);
                OnOfficeBought?.Invoke(this, buff);
            }
        }

        public void MakeWork()
        {
            foreach (Office office in offices)
            {
                office.MakeWork();
            }
        }

#nullable enable
        public event EventWith2Object<GameModel, ulong, ulong>? OnMoneyPut;
#nullable disable

        /// <summary>
        /// Функция для добавления валюты в банк.
        /// </summary>
        /// <param name="money">Добавляемое количество валюты</param>
        public void PutMoney(ulong money)
        {
            this.money += money;
            OnMoneyPut?.Invoke(this, money, this.money);
        }

#nullable enable
        public event EventWith2Object<GameModel, ulong, ulong>? OnMoneyTake;
        public event EventWith2Object<GameModel, ulong, ulong>? OnNotEnoughMoney;
#nullable disable

        /// <summary>
        /// Функция для проверки и получения валюты из банка.
        /// При достаточном балансе функция уничтожит необходимое количество валюты. 
        /// При недостаточном балансе функция выбросит исключение.
        /// </summary>
        /// <param name="money">Необходимое количество валюты</param>
        /// <returns>Взята валюта?</returns>
        public bool TakeMoney(ulong money)
        {
            if (this.money >= money)
            {
                this.money -= money;
                OnMoneyTake?.Invoke(this, money, this.money);
                return true;
            }
            OnNotEnoughMoney?.Invoke(this, money - this.money, this.money);
            return false;
        }

#nullable enable
        public event EventWith2Object<GameModel, ulong, ulong>? OnPremiumPut;
#nullable disable

        /// <summary>
        /// Функция для добавления премиум валюты в банк.
        /// </summary>
        /// <param name="premium">Добавляемое количество премиум валюты</param>
        public void PutPremium(ulong premium)
        {
            this.premium += premium;
            OnPremiumPut?.Invoke(this, premium, this.premium);
        }

#nullable enable
        public event EventWith2Object<GameModel, ulong, ulong>? OnPremiumTake;
        public event EventWith2Object<GameModel, ulong, ulong>? OnNotEnoughPremium;
#nullable disable

        /// <summary>
        /// Функция для проверки и получения премиум валюты из банка.
        /// При достаточном балансе функция вернёт необходимое количество премиум валюты. 
        /// При недостаточном балансе функция выбросит исключение.
        /// </summary>
        /// <param name="premium">Необходимое количество премиум валюты</param>
        /// <returns>Взята валюта?</returns>
        public bool TakePremium(ulong premium)
        {
            if (this.premium >= premium)
            {
                this.premium -= premium;
                OnPremiumTake?.Invoke(this, premium, this.premium);
                return true;
            }
            OnNotEnoughPremium?.Invoke(this, premium - this.premium, this.premium);
            return false;
        }

#nullable enable
        public event EventWith1Object<GameModel, long>? OnReputationIncreased;
        public event EventWith1Object<GameModel, long>? OnReputationDecreased;
#nullable disable

        public void IncreaseReputation(long reputation)
        {
            if (this.reputation + reputation < long.MaxValue)
            {
                this.reputation += reputation;
                OnReputationIncreased?.Invoke(this, this.reputation);
            }
            else
            {
                this.reputation = long.MaxValue;
                OnReputationIncreased?.Invoke(this, this.reputation);
            }
        }

        public void DecreaseReputation(long reputation)
        {
            if (this.reputation - reputation > long.MinValue)
            {
                this.reputation -= reputation;
                OnReputationDecreased?.Invoke(this, this.reputation);
            }
            else
            {
                this.reputation = long.MinValue;
                OnReputationDecreased?.Invoke(this, this.reputation);
            }
        }
    }
}


