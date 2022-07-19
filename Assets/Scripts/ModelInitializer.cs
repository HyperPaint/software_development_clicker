using System.Threading;
using UnityEngine;
using MyGame;
using UnityEngine.UI;
using System;

public class ModelInitializer : MonoBehaviour
{
    private static object mutex = new();
    private GameModel gameModel;
    private Timer timer;

    public Text moneyText;
    public Text premiumMoneyText;

    public byte level;
    public ulong money;
    public ulong premiumMoney;
    public ulong reputation;

    public byte clicks;

    /// <summary>
    ///  онтекст синхронизации, используетс¤ дл¤ выполнени¤ кода в главном потоке.
    /// </summary>
    private static readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current;

    private void Start()
    {
        gameModel = GameModel.Get();
        timer = new Timer(delegate
        {
            lock (mutex)
            {
                gameModel.MakeWork();
                synchronizationContext.Post(delegate
                {
                    moneyText.text = gameModel.Money.ToString();
                    premiumMoneyText.text = gameModel.PremiumMoney.ToString();
                }, null);
            }
        }, null, 0, 500);
    }

    private void Update()
    {
        // дебаг
        level = (byte)gameModel.Level;
        money = gameModel.Money;
        premiumMoney = gameModel.PremiumMoney;
        reputation = gameModel.Reputation;
        clicks = gameModel.Offices[0].Units[0].WorkPlaces[0].Clicks;
    }

    private void OnDestroy()
    {
        gameModel = null;
        timer = null;
    }
}
