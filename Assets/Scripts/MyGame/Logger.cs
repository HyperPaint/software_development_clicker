using System.Threading;
using UnityEngine;

namespace MyGame
{
    class Logger
    {
        private static object mutex = new();

        /// <summary>
        /// Контекст синхронизации, используется для выполнения кода в главном потоке.
        /// </summary>
        private static readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current;

        public Logger(GameModel gameModel)
        {
            gameModel.Upgraded += (sender) =>
            {
                Log("Игровая модель улучшена до " + (sender as GameModel).Level.ToString());
            };

            foreach (var office in gameModel.Offices)
            {
                office.Upgraded += (sender) =>
                {
                    Log("Максимальное количество заказов офиса " + office.ToString() + " повышено до " + office.OrdersMaxCount.ToString());
                };

                foreach (var unit in office.Units)
                {
                    foreach (var part in unit.Parts)
                    {
                        part.Upgraded += (sender) =>
                        {
                            Log("В офисе " + office.ToString() + ", в отделе " + unit.ToString() + " часть " + part.ToString() + " улучшена до " + part.Level.ToString());
                        };
                    }

                    foreach (var workPlace in unit.WorkPlaces)
                    {
                        foreach (var part in workPlace.Parts)
                        {
                            part.Upgraded += (sender) =>
                            {
                                Log("В офисе " + office.ToString() + ", в отделе " + unit.ToString() + ", в рабочем месте " + workPlace.ToString() + " часть " + part.ToString() + " улучшена до " + part.Level.ToString());
                            };
                        }
                    }
                }
            }

            OrderFactory.Get().OrderCreated += (factory, obj) =>
            {
                Order order = (Order)obj;
                Log("Заказ \"" + order.ToString() + "\" создан");
                order.DesigningCompleted += (sender) =>
                {
                    Log("Заказ \"" + order.Name + "\" проектирование завершено");
                };
                order.ArtCompleted += (sender) =>
                {
                    Log("Заказ \"" + order.Name + "\" дизайн завершен");
                };
                order.ProgrammingCompleted += (sender) =>
                {
                    Log("Заказ \"" + order.Name + "\" программирование завершено");
                };
                order.TestingCompleted += (sender) =>
                {
                    Log("Заказ \"" + order.Name + "\" тестирование завершено");
                };
                order.OrderCompleted += (sender) =>
                {
                    Log("Заказ \"" + order.Name + "\" завершен");
                };
            };

            WorkerFactory.Get().WorkerCreated += (factory, obj) =>
            {
                Worker worker = (Worker)obj;
                Log("Работник \"" + worker.ToString() + "\" создан");
                worker.Upgraded += (sender) =>
                {
                    Log("Навык работника " + worker.ToString() + " улучшен до " + worker.Skill.ToString());
                };
            };
        }

        public void Log()
        {
            synchronizationContext.Post(delegate
            {
                const string defaultError = "Произошло неожиданное событие.";
                lock (mutex)
                {
                    Debug.Log(defaultError);
                }
            }, null);
        }

        public void Log(string message)
        {
            synchronizationContext.Post(delegate
            {
                lock (mutex)
                {
                    Debug.Log(message);
                }
            }, null);
        }
    }
}
