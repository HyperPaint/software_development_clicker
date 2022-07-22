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

#nullable enable
        public event EventWith1Object<WorkerFactory, Worker>? OnWorkerCreated;
#nullable disable

        public Worker Create(bool useReputation = true)
        {
            Random random = GameModel.Random;
            Worker.EmployeeType employeeType = (Worker.EmployeeType)random.Next(0, Worker.employeeTypeLength - 1);
            Worker.EmployeeFood employeeBoost = (Worker.EmployeeFood)random.Next(0, Worker.employeeBoostLength - 1);
            Worker.EmployeeFeature employeeFeature = (Worker.EmployeeFeature)random.Next(0, Worker.employeeFeatureLength - 1);
            string firstName = Config.WORKER_FACTORY_WORKER_FIRST_NAME[random.Next(0, Config.WORKER_FACTORY_WORKER_FIRST_NAME.Length - 1)];
            string lastName = Config.WORKER_FACTORY_WORKER_LAST_NAME[random.Next(0, Config.WORKER_FACTORY_WORKER_LAST_NAME.Length - 1)];
            string nickName = Config.WORKER_FACTORY_WORKER_NICK_NAME[random.Next(0, Config.WORKER_FACTORY_WORKER_NICK_NAME.Length - 1)];
            ulong skillStart = Config.WORKER_FACTORY_WORKER_SKILL_START, skillEnd = Config.WORKER_FACTORY_WORKER_SKILL_END;
            if (useReputation)
            {
                long reputationModifier = Convert.ToInt64(GameModel.Get().Reputation * Config.WORKER_FACTORY_WORKER_SKILL_REPUTATION_MODIFIER);
                if (reputationModifier >= 0)
                {
                    skillStart += Convert.ToUInt64(reputationModifier);
                    skillEnd += Convert.ToUInt64(reputationModifier);
                }
                else
                {
                    skillEnd = Config.WORKER_FACTORY_WORKER_SKILL_END;
                }
            }
            ulong skill = Convert.ToUInt64(random.Next(Convert.ToInt32(skillStart), Convert.ToInt32(skillEnd)));
            string summary = Config.WORKER_FACTORY_WORKER_SUMMARY[random.Next(0, Config.WORKER_FACTORY_WORKER_SUMMARY.Length - 1)];
            ulong experience = 0;

            Worker worker = new Worker(employeeType, employeeBoost, employeeFeature, firstName, lastName, nickName, skill, summary, experience);
            OnWorkerCreated?.Invoke(this, worker);
            return worker;
        }

        public Worker Create(Worker.EmployeeType employeeType, bool useReputation = true)
        {
            Random random = GameModel.Random;
            Worker.EmployeeFood employeeBoost = (Worker.EmployeeFood)random.Next(0, Worker.employeeBoostLength - 1);
            Worker.EmployeeFeature employeeFeature = (Worker.EmployeeFeature)random.Next(0, Worker.employeeFeatureLength - 1);
            string firstName = Config.WORKER_FACTORY_WORKER_FIRST_NAME[random.Next(0, Config.WORKER_FACTORY_WORKER_FIRST_NAME.Length - 1)];
            string lastName = Config.WORKER_FACTORY_WORKER_LAST_NAME[random.Next(0, Config.WORKER_FACTORY_WORKER_LAST_NAME.Length - 1)];
            string nickName = Config.WORKER_FACTORY_WORKER_NICK_NAME[random.Next(0, Config.WORKER_FACTORY_WORKER_NICK_NAME.Length - 1)];
            ulong skillStart = Config.WORKER_FACTORY_WORKER_SKILL_START, skillEnd = Config.WORKER_FACTORY_WORKER_SKILL_END;
            if (useReputation)
            {
                long reputationModifier = Convert.ToInt64(GameModel.Get().Reputation * Config.WORKER_FACTORY_WORKER_SKILL_REPUTATION_MODIFIER);
                if (reputationModifier >= 0)
                {
                    skillStart += Convert.ToUInt64(reputationModifier);
                    skillEnd += Convert.ToUInt64(reputationModifier);
                }
                else
                {
                    skillEnd = Config.WORKER_FACTORY_WORKER_SKILL_END;
                }
            }
            ulong skill = Convert.ToUInt64(random.Next(Convert.ToInt32(skillStart), Convert.ToInt32(skillEnd)));
            string summary = Config.WORKER_FACTORY_WORKER_SUMMARY[random.Next(0, Config.WORKER_FACTORY_WORKER_SUMMARY.Length - 1)];
            ulong experience = 0;

            Worker worker = new Worker(employeeType, employeeBoost, employeeFeature, firstName, lastName, nickName, skill, summary, experience);
            OnWorkerCreated?.Invoke(this, worker);
            return worker;
        }
    }
}
