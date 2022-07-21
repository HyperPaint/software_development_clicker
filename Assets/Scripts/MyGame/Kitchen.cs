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
            return Convert.ToUInt64(Math.Pow(Level, Config.KITCHEN_UPGRADE_EXP) * Config.KITCHEN_UPGRADE_COST);
        }

        public ulong GetWaterCost()
        {
            return Convert.ToUInt64(((Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level) - water) * Config.KITCHEN_FOOD_COST_FOR_UNIT);
        }

        public ulong GetCoffeeCost()
        {
            return Convert.ToUInt64(((Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level) - coffee) * Config.KITCHEN_FOOD_COST_FOR_UNIT);
        }

        public ulong GetSushiCost()
        {
            return Convert.ToUInt64(((Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level) - sushi) * Config.KITCHEN_FOOD_COST_FOR_UNIT);
        }

        public ulong GetPizzaCost()
        {
            return Convert.ToUInt64(((Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level) - pizza) * Config.KITCHEN_FOOD_COST_FOR_UNIT);
        }

        public ulong GetCakeCost()
        {
            return Convert.ToUInt64(((Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level) - cake) * Config.KITCHEN_FOOD_COST_FOR_UNIT);
        }

#nullable enable
        public event Event<Kitchen>? WaterBought;
        public event Event<Kitchen>? CoffeeBought;
        public event Event<Kitchen>? SushiBought;
        public event Event<Kitchen>? PizzaBought;
        public event Event<Kitchen>? CakeBought;
#nullable disable

        public void BuyWater()
        {
            GameModel.Get().TakeMoney(GetWaterCost());
            water = Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level;
            WaterBought?.Invoke(this);
        }

        public void BuyCoffee()
        {
            GameModel.Get().TakeMoney(GetWaterCost());
            coffee = Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level;
            CoffeeBought?.Invoke(this);
        }

        public void BuySushi()
        {
            GameModel.Get().TakeMoney(GetWaterCost());
            sushi = Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level;
            SushiBought?.Invoke(this);
        }

        public void BuyPizza()
        {
            GameModel.Get().TakeMoney(GetWaterCost());
            pizza = Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level;
            PizzaBought?.Invoke(this);
        }

        public void BuyCake()
        {
            GameModel.Get().TakeMoney(GetWaterCost());
            cake = Config.KITCHEN_FOOD_COUNT_FOR_LEVEL * Level;
            CakeBought?.Invoke(this);
        }

        public float GetWaterModifier()
        {
            if (water > 0)
            {
                water--;
                return Config.KITCHEN_ENOUGH_FOOD_MODIFIER;
            }
            return Config.KITCHEN_NOT_ENOUGH__FOODMODIFIER;
        }

        public float GetCoffeeModifier()
        {
            if (coffee > 0)
            {
                coffee--;
                return Config.KITCHEN_ENOUGH_FOOD_MODIFIER;
            }
            return Config.KITCHEN_NOT_ENOUGH__FOODMODIFIER;
        }

        public float GetSushiModifier()
        {
            if (sushi > 0)
            {
                sushi--;
                return Config.KITCHEN_ENOUGH_FOOD_MODIFIER;
            }
            return Config.KITCHEN_NOT_ENOUGH__FOODMODIFIER;
        }

        public float GetPizzaModifier()
        {
            if (pizza > 0)
            {
                pizza--;
                return Config.KITCHEN_ENOUGH_FOOD_MODIFIER;
            }
            return Config.KITCHEN_NOT_ENOUGH__FOODMODIFIER;
        }

        public float GetCakeModifier()
        {
            if (cake > 0)
            {
                cake--;
                return Config.KITCHEN_ENOUGH_FOOD_MODIFIER;
            }
            return Config.KITCHEN_NOT_ENOUGH__FOODMODIFIER;
        }
    }
}
