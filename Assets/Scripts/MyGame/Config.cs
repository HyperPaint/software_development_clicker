namespace MyGame
{
    public static class Config
    {
        public static class Base
        {
            public const float GAME_SPEED = 30f;
            public const float MODIFIER_BASE = 1f;
        }

        // BaseAdapter
        public const int BASE_ADAPTER_ANIMATION_TICKS = 60;
        public const int BASE_ADAPTER_ANIMATION_WAIT_TIME = 500;
        public const int BASE_ADAPTER_ANIMATION_TIME = 1000;
        public const float BASE_ADAPTER_ANIMATION_TICK_VALUE = 1f / BASE_ADAPTER_ANIMATION_TICKS;
        public const int BASE_ADAPTER_ANIMATION_TICK_TIME = BASE_ADAPTER_ANIMATION_TIME / BASE_ADAPTER_ANIMATION_TICKS;

        // Clickable
        public const int CLICKABLE_CLICKS_MAX = 50;

        public const byte CLICKABLE_POWER_NONE = 0;
        public const byte CLICKABLE_POWER_EASY = 1;
        public const byte CLICKABLE_POWER_NORMAL = 10;
        public const byte CLICKABLE_POWER_MEDIUM = 20;
        public const byte CLICKABLE_POWER_HARD = 40;

        private const float CLICKABLE_POWER_MODIFIER = 1f;
        public const float CLICKABLE_POWER_NONE_MODIFIER = (Base.MODIFIER_BASE + 0f) * CLICKABLE_POWER_MODIFIER;
        public const float CLICKABLE_POWER_EASY_MODIFIER = (Base.MODIFIER_BASE + 0.5f) * CLICKABLE_POWER_MODIFIER;
        public const float CLICKABLE_POWER_NORMAL_MODIFIER = (Base.MODIFIER_BASE + 1f) * CLICKABLE_POWER_MODIFIER;
        public const float CLICKABLE_POWER_MEDIUM_MODIFIER = (Base.MODIFIER_BASE + 1.5f) * CLICKABLE_POWER_MODIFIER;
        public const float CLICKABLE_POWER_HARD_MODIFIER = (Base.MODIFIER_BASE + 2f) * CLICKABLE_POWER_MODIFIER;

        private const float CLICKABLE_CLICK_CONSUMING_MODIFIER = 0.25f;
        public const float CLICKABLE_POWER_NONE_CLICK_CONSUMING_MODIFIER = (Base.MODIFIER_BASE + 0f) * CLICKABLE_CLICK_CONSUMING_MODIFIER;
        public const float CLICKABLE_POWER_EASY_CLICK_CONSUMING_MODIFIER = (Base.MODIFIER_BASE + 0.5f) * CLICKABLE_CLICK_CONSUMING_MODIFIER;
        public const float CLICKABLE_POWER_NORMAL_CLICK_CONSUMING_MODIFIER = (Base.MODIFIER_BASE + 1f) * CLICKABLE_CLICK_CONSUMING_MODIFIER;
        public const float CLICKABLE_POWER_MEDIUM_CLICK_CONSUMING_MODIFIER = (Base.MODIFIER_BASE + 1.5f) * CLICKABLE_CLICK_CONSUMING_MODIFIER;
        public const float CLICKABLE_POWER_HARD_CLICK_CONSUMING_MODIFIER = (Base.MODIFIER_BASE + 2f) * CLICKABLE_CLICK_CONSUMING_MODIFIER;

        // GameModel
        public const float GAME_MODEL_UPGRADE_MONEY_COST = 5;
        public const float GAME_MODEL_UPGRADE_MONEY_COST_EXP = 3;

        // Kitchen
        public const int KITCHEN_FOOD_COUNT_FOR_LEVEL = 1000;
        public const float KITCHEN_FOOD_MONEY_COST_FOR_UNIT = 0.1f;

        public const float KITCHEN_ENOUGH_FOOD_MODIFIER = 1.2f;
        public const float KITCHEN_NOT_ENOUGH_FOOD_MODIFIER = 0.8f;

        public const ulong KITCHEN_UPGRADE_MONEY_COST = 5;
        public const ulong KITCHEN_UPGRADE_MONEY_COST_EXP = 3;

        // Logger
        public const bool LOGGER_ENABLED = true;

        // Office
        public const int OFFICE_UNITS_MAX_COUNT = 7;
        public const int OFFICE_ORDERS_MAX_COUNT = 7;
        public const int OFFICE_ORDERS_START_COUNT = 2;

        public const float OFFICE_MONEY_COST = 5;
        public const float OFFICE_MONEY_COST_EXP = 3;

        public const ulong OFFICE_UPGRADE_MONEY_COST = 5;
        public const ulong OFFICE_UPGRADE_MONEY_COST_EXP = 3;

        // OfficePart
        public const float OFFICE_PART_INTERNET_MODIFIER_PER_LEVEL = 0.05f;
        public const float OFFICE_PART_CLIMATE_MODIFIER_PER_LEVEL = 0.05f;
        public const float OFFICE_PART_MUSIC_MODIFIER_PER_LEVEL = 0.05f;

        public const float OFFICE_PART_UPGRADE_MONEY_COST = 5;
        public const float OFFICE_PART_UPGRADE_MONEY_COST_EXP = 3;

        // Order
        public const long ORDER_REPUTATION_CHANGE_ON_COMPLETE = 1;

        // OrderFactory
        public static readonly float ORDER_FACTORY_ORDER_PART_MONEY_PER_NEEDED = 1f;
        public static readonly float ORDER_FACTORY_ORDER_PART_MONEY_PER_NEEDED_EXP = 0.5f;

        public static readonly byte ORDER_FACTORY_ORDER_PART_PREMIUM_CHANCE = 1;
        public static readonly float ORDER_FACTORY_ORDER_PART_PREMIUM_PER_NEEDED = 0.5f;
        public static readonly float ORDER_FACTORY_ORDER_PART_PREMIUM_PER_NEEDED_EXP = 0.25f;

        public const ulong ORDER_FACTORY_ORDER_PART_START = 100;
        public const ulong ORDER_FACTORY_ORDER_PART_END = 300;
        public const float ORDER_FACTORY_ORDER_PART_REPUTATION_MODIFIER = 2f;
        public const float ORDER_FACTORY_ORDER_MONEY_REPUTATION_MODIFIER = 2f;

        public static readonly string[] ORDER_FACTORY_ORDER_NAME = {
            "Камень онлайн",
            "Симулятор жизни абрикоса",
            "Уничтожение ручки",
            "Банзай",
            "Прыгай 2",
            "Шашлык - кулинарный сборник",
        };
        public static readonly string[] ORDER_FACTORY_ORDER_DESCRIPTION =
        {
            "Здесь должно быть описание заказа",
            "Здесь должно быть описание заказа",
            "Здесь должно быть описание заказа",
            "Здесь должно быть описание заказа",
            "Здесь должно быть описание заказа",
            "Здесь должно быть описание заказа",
        };

        // Unit
        public const int UNIT_WORKPLACES_COUNT = 6;

        public const float UNIT_MONEY_COST = 5;
        public const float UNIT_MONEY_COST_EXP = 3;

        // Upgradeable --

        // UpgradeablePart --

        // Worker
        public const float WORKER_WORK_PER_SKILL = 0.25f;

        public const float WORKER_FULLSTACK_WORK_MODIFIER = 0.5f;
        public const float WORKER_DESIGNER_WORK_MODIFIER = 1f;
        public const float WORKER_ARTIST_WORK_MODIFIER = 1f;
        public const float WORKER_PROGRAMMER_WORK_MODIFIER = 1f;
        public const float WORKER_TESTER_WORK_MODIFIER = 1f;

        public const float WORKER_STUDENT_WORK_MODIFIER = 0.5f;
        public const float WORKER_SPECIALIST_WORK_MODIFIER = 2f;
        public const byte WORKER_LAZY_WORK_CHANCE = 25;
        public const float WORKER_LAZY_WORK_MODIFIER = 0.5f;
        public const byte WORKER_CREATIVE_WORK_CHANCE = 25;
        public const float WORKER_CREATIVE_WORK_MODIFIER = 2f;

        public const float WORKER_STUDENT_COST_MODIFIER = 0.5f;
        public const float WORKER_SPECIALIST_COST_MODIFIER = 2f;
        public const float WORKER_LAZY_COST_MODIFIER = 0.66f;
        public const float WORKER_CREATIVE_COST_MODIFIER = 1.33f;

        public const float WORKER_SKILL_MONEY_COST = 5;
        public const float WORKER_SKILL_MONEY_COST_EXP = 3;

        public const float WORKER_EXPERIENCE_PER_WORK = 0.1f;
        public const float WORKER_UPGRADE_SKILL_EXPERIENCE_COST = 5;
        public const float WORKER_UPGRADE_SKILL_EXPERIENCE_COST_EXP = 3;

        public const long WORKER_REPUTATION_CHANGE_ON_DISMISS = -10;

        // WorkerFactory
        public const ulong WORKER_FACTORY_WORKER_SKILL_START = 5;
        public const ulong WORKER_FACTORY_WORKER_SKILL_END = 10;
        public const float WORKER_FACTORY_WORKER_SKILL_REPUTATION_MODIFIER = 1.1f;

        public static readonly string[] WORKER_FACTORY_WORKER_FIRST_NAME = {
            "Шарж",
            "Абрикос",
            "Шарик",
            "Банзай",
            "Шашлык",
        };

        public static readonly string[] WORKER_FACTORY_WORKER_LAST_NAME = {
            "Адонис",
            "Витязь",
            "Шарль",
            "Эрос",
            "Арамис",
        };

        public static readonly string[] WORKER_FACTORY_WORKER_NICK_NAME =
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

        public static readonly string[] WORKER_FACTORY_WORKER_SUMMARY =
        {
            "Здесь должна быть краткая забавная история работника",
        };

        // Workplace --

        // WorkplacePart
        public const float WORKPLACE_PART_TABLE_MODIFIER_PER_LEVEL = 0.05f;
        public const float WORKPLACE_PART_CHAIR_MODIFIER_PER_LEVEL = 0.10f;
        public const float WORKPLACE_PART_COMPUTER_MODIFIER_PER_LEVEL = 0.15f;

        public const float WORKPLACE_PART_UPGRADE_MONEY_COST = 5;
        public const float WORKPLACE_PART_UPGRADE_MONEY_COST_EXP = 3;
    }
}
