using System.Timers;
using UnityEngine;
using MyGame;
using UnityEngine.UI;

public class MyGameInitializer : MonoBehaviour
{
    private GameModel gameModel;
    private Timer timer;

    public Text moneyText;
    public Text premiumText;

    /// <summary>
    /// Контекст синхронизации, используетс¤ дл¤ выполнени¤ кода в главном потоке.
    /// </summary>
    private static System.Threading.SynchronizationContext synchronizationContext;

    private void Start()
    {
        synchronizationContext = System.Threading.SynchronizationContext.Current;

        gameModel = GameModel.Get();
        timer = new Timer();
        timer.Elapsed += delegate
        {
            gameModel.MakeWork();
            synchronizationContext.Post(delegate
            {
                moneyText.text = gameModel.Money.ToString();
                premiumText.text = gameModel.Premium.ToString();
            }, null);
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
