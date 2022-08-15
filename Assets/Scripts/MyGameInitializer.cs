using System.Timers;
using UnityEngine;
using MyGame;
using UnityEngine.UI;

public class MyGameInitializer : MonoBehaviour
{
    private Timer timer;
    private GameModel gameModel;

    public Text moneyText;
    public Text premiumText;

    /// <summary>
    /// Контекст синхронизации, используетс¤ дл¤ выполнени¤ кода в главном потоке.
    /// </summary>
    private static System.Threading.SynchronizationContext synchronizationContext;

    private void Start()
    {
        synchronizationContext = System.Threading.SynchronizationContext.Current;

        timer = new Timer();
        gameModel = GameModel.Get();
        timer.Elapsed += delegate
        {
            gameModel.MakeWork();
        };
        timer.Interval = 1000f / Config.Base.GAME_SPEED;
        timer.Start();

        moneyText.text = gameModel.Money.ToString();
        premiumText.text = gameModel.Premium.ToString();

        gameModel.OnMoneyPut += delegate
        {
            synchronizationContext.Post(delegate
            {
                moneyText.text = gameModel.Money.ToString();
            }, null);
        };

        gameModel.OnMoneyTake += delegate
        {
            synchronizationContext.Post(delegate
            {
                moneyText.text = gameModel.Money.ToString();
            }, null);
        };

        gameModel.OnMoneyPut += delegate
        {
            synchronizationContext.Post(delegate
            {
                premiumText.text = gameModel.Premium.ToString();
            }, null);
        };

        gameModel.OnMoneyTake += delegate
        {
            synchronizationContext.Post(delegate
            {
                premiumText.text = gameModel.Premium.ToString();
            }, null);
        };
    }

    private void OnApplicationQuit()
    {
        // todo сохранение
        timer.Close();
    }
}
