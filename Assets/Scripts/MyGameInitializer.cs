using System.Timers;
using UnityEngine;
using MyGame;
using UnityEngine.UI;

public class MyGameInitializer : MonoBehaviour
{
    private static object mutex = new();
    private GameModel gameModel;
    private Timer timer = new Timer();

    public Text moneyText;
    public Text premiumMoneyText;

    public byte level;
    public ulong money;
    public ulong premiumMoney;
    public ulong reputation;

    public int clicks;

    /// <summary>
    /// Контекст синхронизации, используетс¤ дл¤ выполнени¤ кода в главном потоке.
    /// </summary>
    private static readonly System.Threading.SynchronizationContext synchronizationContext = System.Threading.SynchronizationContext.Current;

    private void Start()
    {
        gameModel = GameModel.Get();
        timer.Elapsed += delegate
        {
            lock (mutex)
            {
                gameModel.MakeWork();
                synchronizationContext.Post(delegate
                {
                    moneyText.text = gameModel.Money.ToString();
                    premiumMoneyText.text = gameModel.PremiumMoney.ToString();

                    level = (byte)gameModel.Level;
                    money = gameModel.Money;
                    premiumMoney = gameModel.PremiumMoney;
                    reputation = gameModel.Reputation;
                    clicks = gameModel.Offices[0].Units[0].Workplaces[0].Clicks;
                }, null);
            }
        };
        timer.Interval = 1000f / Config.Base.GAME_SPEED;
        timer.Start();
    }

    private void OnApplicationQuit()
    {
        // todo сохранение
        timer.Stop();
    }
}
