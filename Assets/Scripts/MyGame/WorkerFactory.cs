using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class WorkerFactory
    {
        private static object mutex = new();
        private static volatile WorkerFactory factory;

        /// <summary>
        /// Функция для получения единственного объекта фабрики.
        /// Можно безопасно использовать с любого потока.
        /// </summary>
        /// <returns>Фабрика</returns>
        public static WorkerFactory Get()
        {
            if (factory == null)
            {
                lock (mutex)
                {
                    if (factory == null)
                    {
                        factory = new WorkerFactory();
                    }
                }
            }
            return factory;
        }

        private WorkerFactory() { }

        private readonly string[] firstNames = {
            "Шарж",
            "Абрикос",
            "Шарик",
            "Банзай",
            "Шашлык",
        };

        private readonly string[] lastNames = {
            "Адонис",
            "Витязь",
            "Шарль",
            "Эрос",
            "Арамис",
        };

        private readonly string[] nickNames =
        {
            "Vanya567",
            "Ripering",
            "Evil_Biscuit",
            "Gendalf[BY]",
            "Xincerrie",
            "Slediks",
            "Taske",
            "Di4_Feyter",
            "Fearo",
            "Bicker",
            "Cyrus",
        };

#nullable enable
        public event EventObject<WorkerFactory, Worker>? WorkerCreated;
#nullable disable

        public Worker Create()
        {
            Random random = GameModel.Random;
            Worker.EmployeeType employeeType = (Worker.EmployeeType)random.Next(0, Worker.employeeTypeLength - 1);
            Worker.EmployeeBoost employeeBoost = (Worker.EmployeeBoost)random.Next(0, Worker.employeeBoostLength - 1);
            Worker.EmployeeFeature employeeFeature = (Worker.EmployeeFeature)random.Next(0, Worker.employeeFeatureLength - 1);
            string firstName = firstNames[random.Next(0, firstNames.Length - 1)];
            string lastName = lastNames[random.Next(0, lastNames.Length - 1)];
            string nickName = nickNames[random.Next(0, nickNames.Length - 1)];
            // todo должно зависеть от репутации
            byte skill = (byte)random.Next();
            string summary = "Здесь должна быть краткая забавная история работника";

            Worker worker = new Worker(employeeType, employeeBoost, employeeFeature, firstName, lastName, nickName, skill, summary);
            WorkerCreated?.Invoke(this, worker);
            return worker;
        }

        public Worker Create(Worker.EmployeeType employeeType)
        {
            Random random = GameModel.Random;
            Worker.EmployeeBoost employeeBoost = (Worker.EmployeeBoost)random.Next(0, Worker.employeeBoostLength - 1);
            Worker.EmployeeFeature employeeFeature = (Worker.EmployeeFeature)random.Next(0, Worker.employeeFeatureLength - 1);
            string firstName = firstNames[random.Next(0, firstNames.Length - 1)];
            string lastName = lastNames[random.Next(0, lastNames.Length - 1)];
            string nickName = nickNames[random.Next(0, nickNames.Length - 1)];
            // todo должно зависеть от репутации
            // todo необходимо уменьшить как было
            byte skill = 150;
            string summary = "Здесь должна быть краткая забавная история работника";

            Worker worker = new Worker(employeeType, employeeBoost, employeeFeature, firstName, lastName, nickName, skill, summary);
            WorkerCreated?.Invoke(this, worker);
            return worker;
        }

        public Worker Create(Worker.EmployeeType employeeType, Worker.EmployeeFeature employeeFeature)
        {
            Random random = GameModel.Random;
            Worker.EmployeeBoost employeeBoost = (Worker.EmployeeBoost)random.Next(0, Worker.employeeBoostLength - 1);
            string firstName = firstNames[random.Next(0, firstNames.Length - 1)];
            string lastName = lastNames[random.Next(0, lastNames.Length - 1)];
            string nickName = nickNames[random.Next(0, nickNames.Length - 1)];
            // todo должно зависеть от репутации
            byte skill = 5;
            string summary = "Здесь должна быть краткая забавная история работника";

            Worker worker = new Worker(employeeType, employeeBoost, employeeFeature, firstName, lastName, nickName, skill, summary);
            WorkerCreated?.Invoke(this, worker);
            return worker;
        }
    }
}
