using System;

namespace MyGame
{
    public class Kitchen : Upgradeable
    {
        public int water;
        public int Water { get => water; }
        public int coffee;
        public int Coffee { get => coffee; }
        public int sushi;
        public int Sushi { get => sushi; }
        public int pizza;
        public int Pizza { get => pizza; }
        public int cake;
        public int Cake { get => cake; }

        public Kitchen() : this(0, 0, 0, 0, 0) { }

        public Kitchen(int water, int coffee, int sushi, int pizza, int cake)
        {
            this.water = water;
            this.coffee = coffee;
            this.sushi = sushi;
            this.pizza = pizza;
            this.cake = cake;
        }

        public override ulong GetUpgradeCost()
        {
            return Convert.ToUInt64(Math.Pow(Level, Config.KITCHEN_UPGRADE_MONEY_COST_EXP) * Config.KITCHEN_UPGRADE_MONEY_COST);
        }

        public ulong GetWaterCost()
        {
            return Convert.ToUInt64(((Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level) - Convert.ToUInt64(water)) * Config.KITCHEN_FOOD_MONEY_COST_FOR_UNIT);
        }

        public ulong GetCoffeeCost()
        {
            return Convert.ToUInt64(((Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level) - Convert.ToUInt64(coffee)) * Config.KITCHEN_FOOD_MONEY_COST_FOR_UNIT);
        }

        public ulong GetSushiCost()
        {
            return Convert.ToUInt64(((Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level) - Convert.ToUInt64(sushi)) * Config.KITCHEN_FOOD_MONEY_COST_FOR_UNIT);
        }

        public ulong GetPizzaCost()
        {
            return Convert.ToUInt64(((Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level) - Convert.ToUInt64(pizza)) * Config.KITCHEN_FOOD_MONEY_COST_FOR_UNIT);
        }

        public ulong GetCakeCost()
        {
            return Convert.ToUInt64(((Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level) - Convert.ToUInt64(cake)) * Config.KITCHEN_FOOD_MONEY_COST_FOR_UNIT);
        }

#nullable enable
        public event EventWith1Object<Kitchen, int>? OnWaterBought;
        public event EventWith1Object<Kitchen, int>? OnCoffeeBought;
        public event EventWith1Object<Kitchen, int>? OnSushiBought;
        public event EventWith1Object<Kitchen, int>? OnPizzaBought;
        public event EventWith1Object<Kitchen, int>? OnCakeBought;
#nullable disable

        public bool BuyWater()
        {
            if (GameModel.Get().TakeMoney(GetWaterCost()))
            {
                water = Convert.ToInt32(Convert.ToUInt64(Config.KITCHEN_FOOD_COUNT_FOR_LEVEL) * Level);
                OnWaterBought?.Invoke(this, water);
                return true;
            }
            return false;
        }

        public bool BuyCoffee()
        {
            if (GameModel.Get().TakeMoney(GetWaterCost()))
            {
                coffee = Convert.ToInt32(Convert.ToUInt64(Config.KITCHEN_FOOD_COUNT_FOR_LEVEL) * Level);
                OnCoffeeBought?.Invoke(this, coffee);
                return true;
            }
            return false;
        }

        public bool BuySushi()
        {
            if (GameModel.Get().TakeMoney(GetWaterCost()))
            {
                sushi = Convert.ToInt32(Convert.ToUInt64(Config.KITCHEN_FOOD_COUNT_FOR_LEVEL) * Level);
                OnSushiBought?.Invoke(this, sushi);
                return true;
            }
            return false;
        }

        public bool BuyPizza()
        {
            if (GameModel.Get().TakeMoney(GetWaterCost()))
            {
                pizza = Convert.ToInt32(Convert.ToUInt64(Config.KITCHEN_FOOD_COUNT_FOR_LEVEL) * Level);
                OnPizzaBought?.Invoke(this, pizza);
                return true;
            }
            return false;
        }

        public bool BuyCake()
        {
            if (GameModel.Get().TakeMoney(GetWaterCost()))
            {
                cake = Convert.ToInt32(Convert.ToUInt64(Config.KITCHEN_FOOD_COUNT_FOR_LEVEL) * Level);
                OnCakeBought?.Invoke(this, cake);
                return true;
            }
            return false;
        }

        public float GetWaterModifier()
        {
            if (water > 0)
            {
                water--;
                return Config.KITCHEN_ENOUGH_FOOD_MODIFIER;
            }
            return Config.KITCHEN_NOT_ENOUGH_FOOD_MODIFIER;
        }

        public float GetCoffeeModifier()
        {
            if (coffee > 0)
            {
                coffee--;
                return Config.KITCHEN_ENOUGH_FOOD_MODIFIER;
            }
            return Config.KITCHEN_NOT_ENOUGH_FOOD_MODIFIER;
        }

        public float GetSushiModifier()
        {
            if (sushi > 0)
            {
                sushi--;
                return Config.KITCHEN_ENOUGH_FOOD_MODIFIER;
            }
            return Config.KITCHEN_NOT_ENOUGH_FOOD_MODIFIER;
        }

        public float GetPizzaModifier()
        {
            if (pizza > 0)
            {
                pizza--;
                return Config.KITCHEN_ENOUGH_FOOD_MODIFIER;
            }
            return Config.KITCHEN_NOT_ENOUGH_FOOD_MODIFIER;
        }

        public float GetCakeModifier()
        {
            if (cake > 0)
            {
                cake--;
                return Config.KITCHEN_ENOUGH_FOOD_MODIFIER;
            }
            return Config.KITCHEN_NOT_ENOUGH_FOOD_MODIFIER;
        }

        public override string ToString()
        {
            return "Kitchen";
        }
    }
}
