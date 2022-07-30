using System.Timers;
using UnityEngine;
using MyGame;
using UnityEngine.UI;

public class MyGameInitializer : MonoBehaviour
{
    private GameModel gameModel;
    private Timer timer;

    [SerializeField] public Text moneyText;
    [SerializeField] public Text premiumText;

    /// <summary>
    /// Контекст синхронизации, используетс¤ дл¤ выполнени¤ кода в главном потоке.
    /// </summary>
    private System.Threading.SynchronizationContext synchronizationContext;
    private object mutex;

    private void Start()
    {
        synchronizationContext = System.Threading.SynchronizationContext.Current;
        mutex = new();

        gameModel = GameModel.Get();
        timer = new Timer();
        timer.Elapsed += delegate
        {
            lock (mutex)
            {
                gameModel.MakeWork();
                synchronizationContext.Post(delegate
                {
                    moneyText.text = gameModel.Money.ToString();
                    premiumText.text = gameModel.Premium.ToString();
                }, null);
            }
        };
        timer.Interval = Config.Base.GAME_SPEED_MS;
        timer.Start();
    }

    private void Update()
    {
        //moneyText.text = gameModel.Money.ToString();
        //premiumText.text = gameModel.Premium.ToString();
    }

    private void OnApplicationQuit()
    {
        // todo сохранение
        timer.Stop();
    }
}
