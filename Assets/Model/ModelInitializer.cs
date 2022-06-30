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
    public Text orderText1;
    public Text orderText2;

    [SerializeField] string hint = "Ќиже пол€ только дл€ ознакомлени€";

    [SerializeField] byte level;
    [SerializeField] ulong money;
    [SerializeField] ulong premiumMoney;
    [SerializeField] ulong reputation;

    [SerializeField] byte clicks;

    void Start()
    {
        settingsModel = SettingsModel.Get();
        gameModel = GameModel.Get();

        MakeWork makeWork = () => 
        {
            lock (mutex)
            {
                gameModel.MakeWork();
            }
        };

        timer = new Timer((o) => makeWork(), null, 0, 1000);
    }

    delegate void MakeWork();

    void Update()
    {
        // todo нужно всЄ это каким-то чудом вызывать после работы, передава€ код из другого потока в главный, т.к. UI подчин€етс€ только главному потоку
        moneyText.text = gameModel.Money.ToString();
        premiumMoneyText.text = gameModel.PremiumMoney.ToString();
        Order buffOrder;
        string buffString;
        buffOrder = gameModel.Offices[0].Orders[0];
        buffString = buffOrder.ToString() + "\n";
        buffString += ((float)buffOrder.Designing.current / buffOrder.Designing.needed * 100f).ToString() + "%\n";
        buffString += ((float)buffOrder.Art.current / buffOrder.Art.needed * 100f).ToString() + "%\n";
        buffString += ((float)buffOrder.Programming.current / buffOrder.Programming.needed * 100f).ToString() + "%\n";
        buffString += ((float)buffOrder.Testing.current / buffOrder.Testing.needed * 100f).ToString() + "%\n";
        orderText1.text = buffString;
        buffOrder = gameModel.Offices[0].Orders[1];
        buffString = buffOrder.ToString() + "\n";
        buffString += ((float)buffOrder.Designing.current / buffOrder.Designing.needed * 100f).ToString() + "%\n";
        buffString += ((float)buffOrder.Art.current / buffOrder.Art.needed * 100f).ToString() + "%\n";
        buffString += ((float)buffOrder.Programming.current / buffOrder.Programming.needed * 100f).ToString() + "%\n";
        buffString += ((float)buffOrder.Testing.current / buffOrder.Testing.needed * 100f).ToString() + "%\n";
        orderText2.text = buffString;

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
