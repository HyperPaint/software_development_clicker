using UnityEngine;

namespace MyGame
{
    class Logger
    {
        private static object mutex = new();
        private static volatile Logger logger;

        /// <summary>
        /// Функция для получения единственного объекта логгер.
        /// Можно безопасно использовать с любого потока.
        /// </summary>
        /// <returns>Логгер</returns>
        public static Logger Get()
        {
            if (logger == null)
            {
                lock (mutex)
                {
                    if (logger == null)
                    {
                        logger = new Logger();
                    }
                }
            }
            return logger;
        }

        private Logger()
        {
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
