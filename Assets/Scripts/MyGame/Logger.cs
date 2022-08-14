using System;
using UnityEngine;

namespace MyGame
{
    class Logger
    {
        private static object mutex = new();

        public Logger(GameModel gameModel)
        {
            gameModel.OnGameStageUpgraded += (sender) =>
            {
                Log("Игровая модель улучшена до " + sender.Level.ToString());
            };

            gameModel.OnMoneyPut += (sender, money, current) =>
            {
                Log("Валюта увеличена на " + money.ToString() + " (" + current.ToString() + ")");
            };

            gameModel.OnMoneyTake += (sender, money, current) =>
            {
                Log("Валюта уменьшена на " + money.ToString() + " (" + current.ToString() + ")");
            };

            gameModel.OnNotEnoughMoney += (sender, money, current) =>
            {
                Log("Валюты недостаточно " + money.ToString() + " (" + current.ToString() + ")");
            };

            gameModel.OnPremiumPut += (sender, premium, current) =>
            {
                Log("Премиум валюта увеличена на " + premium.ToString() + " (" + current.ToString() + ")");
            };

            gameModel.OnPremiumTake += (sender, premium, current) =>
            {
                Log("Премиум валюта уменьшена на " + premium.ToString() + " (" + current.ToString() + ")");
            };

            gameModel.OnNotEnoughPremium += (sender, premium, current) =>
            {
                Log("Премиум валюты недостаточно " + premium.ToString() + " (" + current.ToString() + ")");
            };

            gameModel.OnReputationIncreased += (sender, reputation) =>
            {
                Log("Репутация увеличена (" + reputation + ")");
            };

            gameModel.OnReputationDecreased += (sender, reputation) =>
            {
                Log("Репутация уменьшена (" + reputation + ")");
            };

            foreach (var office in gameModel.Offices)
            {
                GameModelOfficeBind(gameModel, office);

                foreach (var order in office.Orders)
                {
                    OrderBind(order);
                }

                foreach (var unit in office.Units)
                {
                    foreach (var workplace in unit.Workplaces)
                    {
                        if (workplace.Worker != null)
                        {
                            WorkerBind(workplace.Worker);
                        }
                    }
                }
            }

            gameModel.OnOfficeBought += (sender, office) =>
            {
                Log("Куплен офис " + office.ToString());
                GameModelOfficeBind(sender, office);
            };

            OrderFactory.Get().OnOrderCreated += (factory, order) =>
            {
                Log("Заказ \"" + order.ToString() + "\" создан");
                OrderBind(order);
            };

            WorkerFactory.Get().OnWorkerCreated += (factory, worker) =>
            {
                Log("Работник \"" + worker.ToString() + "\" создан");
                WorkerBind(worker);
            };
        }

        private void GameModelOfficeBind(GameModel gameModel, Office office)
        {
            office.OnUpgraded += (sender, level) =>
            {
                Log("Максимальное количество заказов офиса " + sender.ToString() + " повышено до " + level.ToString());
            };

            office.OnOrderAdded += (sender, order, position) =>
            {
                Log("В офисе " + sender.ToString() + " добавлен заказ " + order.ToString());
            };

            office.OnOrderDeleted += (sender, order, position) =>
            {
                Log("В офисе " + sender.ToString() + " удалён заказ " + order.ToString());
            };

            office.Kitchen.OnUpgraded += (sender, level) =>
            {
                Log("В офисе " + sender.ToString() + " улучшена кухня " + office.Kitchen.ToString());
            };

            office.Kitchen.OnWaterBought += (sender, count) =>
            {
                Log("В офисе " + office.ToString() + " на кухню " + sender.ToString() + " закуплена вода (" + count.ToString() + ")");
            };

            office.Kitchen.OnCoffeeBought += (sender, count) =>
            {
                Log("В офисе " + office.ToString() + " на кухню " + sender.ToString() + " закуплен кофе (" + count.ToString() + ")");
            };

            office.Kitchen.OnSushiBought += (sender, count) =>
            {
                Log("В офисе " + office.ToString() + " на кухню " + sender.ToString() + " закуплены суши (" + count.ToString() + ")");
            };

            office.Kitchen.OnPizzaBought += (sender, count) =>
            {
                Log("В офисе " + office.ToString() + " на кухню " + sender.ToString() + " закуплены пиццы (" + count.ToString() + ")");
            };

            office.Kitchen.OnCakeBought += (sender, count) =>
            {
                Log("В офисе " + office.ToString() + " на кухню " + sender.ToString() + " закуплены торты (" + count.ToString() + ")");
            };

            foreach (var part in office.Parts)
            {
                part.OnUpgraded += (sender, level) =>
                {
                    Log("В офисе " + office.ToString() + " часть " + sender.ToString() + " улучшена до " + level);
                };
            }

            foreach (var unit in office.Units)
            {
                OfficeUnitBind(office, unit);
            }

            office.OnUnitBought += (sender, unit) =>
            {
                Log("В офисе " + sender.ToString() + " куплена комната " + unit.ToString());
                OfficeUnitBind(sender, unit);
            };
        }

        private void OfficeUnitBind(Office office, Unit unit)
        {
            foreach (var workplace in unit.Workplaces)
            {
                workplace.OnClick += (sender, clicks) =>
                {
                    Log("В офисе " + office.ToString() + " в комнате " + unit.ToString() + " рабочее место " + sender.ToString() + " получило клик (" + clicks.ToString() + ")");
                };

                workplace.OnWorkerBought += (sender, worker) =>
                {
                    Log("В офисе " + office.ToString() + " на рабочее место " + sender.ToString() + " нанят работник " + worker.ToString());
                };

                foreach (var part in workplace.Parts)
                {
                    part.OnUpgraded += (sender, level) =>
                    {
                        Log("В офисе " + office.ToString() + ", в отделе " + unit.ToString() + ", в рабочем месте " + workplace.ToString() + " часть " + sender.ToString() + " улучшена до " + level.ToString());
                    };
                }
            }
        }

        private void OrderBind(Order order)
        {
            order.OnDesigningCompleted += (sender) =>
            {
                Log("Заказ \"" + sender.Name + "\" проектирование завершено");
            };

            order.OnProgrammingCompleted += (sender) =>
            {
                Log("Заказ \"" + sender.Name + "\" программирование завершено");
            };

            order.OnTexturingCompleted += (sender) =>
            {
                Log("Заказ \"" + sender.Name + "\" текстурирование завершено");
            };

            order.OnTestingCompleted += (sender) =>
            {
                Log("Заказ \"" + sender.Name + "\" тестирование завершено");
            };

            order.OnOrderCompleted += (sender) =>
            {
                Log("Заказ \"" + sender.Name + "\" завершен");
            };
        }

        private void WorkerBind(Worker worker)
        {
            worker.OnSkillUpgraded += (sender, skill) =>
            {
                Log("Работник " + worker.ToString() + " улучшил навык до " + skill.ToString());
            };
        }

        public void Log()
        {
            const string defaultError = "Произошло неожиданное событие.";
            lock (mutex)
            {
                Debug.Log(defaultError);
            }
        }

        public void Log(string message)
        {
            lock (mutex)
            {
                Debug.Log(message);
            }
        }
    }
}
