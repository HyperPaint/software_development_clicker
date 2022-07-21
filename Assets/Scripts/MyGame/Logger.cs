using UnityEngine;

namespace MyGame
{
    class Logger
    {
        private static object mutex = new();

        public Logger(GameModel gameModel)
        {
            gameModel.Upgraded += (sender) =>
            {
                Log("Игровая модель улучшена до " + sender.Level.ToString());
            };

            foreach (var office in gameModel.Offices)
            {
                office.Upgraded += (sender) =>
                {
                    Log("Максимальное количество заказов офиса " + sender.ToString() + " повышено до " + sender.Level.ToString());
                };

                foreach (var part in office.Parts)
                {
                    part.Upgraded += (sender) =>
                    {
                        Log("В офисе " + office.ToString() + " часть " + sender.ToString() + " улучшена до " + sender.Level.ToString());
                    };
                }
                foreach (var unit in office.Units)
                {
                    foreach (var workPlace in unit.Workplaces)
                    {
                        foreach (var part in workPlace.Parts)
                        {
                            part.Upgraded += (sender) =>
                            {
                                Log("В офисе " + office.ToString() + ", в отделе " + unit.ToString() + ", в рабочем месте " + workPlace.ToString() + " часть " + sender.ToString() + " улучшена до " + sender.Level.ToString());
                            };
                        }
                    }
                }
            }

            OrderFactory.Get().OrderCreated += (factory, obj) =>
            {
                Log("Заказ \"" + obj.ToString() + "\" создан");
                obj.DesigningCompleted += (sender) =>
                {
                    Log("Заказ \"" + sender.Name + "\" проектирование завершено");
                };
                obj.ArtCompleted += (sender) =>
                {
                    Log("Заказ \"" + sender.Name + "\" дизайн завершен");
                };
                obj.ProgrammingCompleted += (sender) =>
                {
                    Log("Заказ \"" + sender.Name + "\" программирование завершено");
                };
                obj.TestingCompleted += (sender) =>
                {
                    Log("Заказ \"" + sender.Name + "\" тестирование завершено");
                };
                obj.OrderCompleted += (sender) =>
                {
                    Log("Заказ \"" + sender.Name + "\" завершен");
                };
            };

            WorkerFactory.Get().WorkerCreated += (factory, obj) =>
            {
                Log("Работник \"" + obj.ToString() + "\" создан");
                obj.Upgraded += (sender) =>
                {
                    Log("Навык работника " + sender.ToString() + " улучшен до " + sender.Skill.ToString());
                };
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
