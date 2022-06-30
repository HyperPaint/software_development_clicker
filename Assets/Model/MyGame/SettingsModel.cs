using System;
namespace MyGame
{
    public class SettingsModel
    {
        private static object mutex = new();
        private static volatile SettingsModel model;

        /// <summary>
        /// Функция для получения единственного объекта модели.
        /// Можно безопасно использовать с любого потока.
        /// </summary>
        /// <returns>Модель</returns>
        public static SettingsModel Get()
        {
            if (model == null)
            {
                lock (mutex)
                {
                    if (model == null)
                    {
                        model = new SettingsModel();
                    }
                }
            }
            return model;
        }

        private SettingsModel()
        {
            // загрузка настроек
        }
    }
}
