using System.Threading;
using UnityEngine;
using MyGame;
using UnityEngine.UI;

public class ModelInitializer : MonoBehaviour
{
    private static object mutex = new();

    SettingsModel settingsModel;
    GameModel gameModel;

    Timer timer;

    public Text moneyText;
    public Text premiumMoneyText;

    public byte level;
    public ulong money;
    public ulong premiumMoney;
    public ulong reputation;

    public byte clicks;

    private void Start()
    {
        settingsModel = SettingsModel.Get();
        gameModel = GameModel.Get();

        timer = new Timer(delegate
        {
            lock (mutex)
            {
                gameModel.MakeWork();
            }
        }, null, 0, 1000);
    }

    private void Update()
    {
        // todo нужно всё это каким-то чудом вызывать после работы, передавая код из другого потока в главный, т.к. UI подчиняется только главному потоку
        moneyText.text = gameModel.Money.ToString();
        premiumMoneyText.text = gameModel.PremiumMoney.ToString();

        // дебаг
        level = (byte)gameModel.Level;
        money = gameModel.Money;
        premiumMoney = gameModel.PremiumMoney;
        reputation = gameModel.Reputation;
        clicks = gameModel.Offices[0].Units[0].WorkPlaces[0].Clicks;
    }

    private void OnDestroy()
    {
        settingsModel = null;
        gameModel = null;
        timer = null;
    }
}
