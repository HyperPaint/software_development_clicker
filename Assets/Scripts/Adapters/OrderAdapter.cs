using MyGame;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class OrderAdapter : BaseAdapter<Order, OrderAdapter.OrderView>
{
    public class OrderView : View
    {
        public Image designing;
        public Image texturing;
        public Image programming;
        public Image testing;

        public OrderView(RectTransform prefab, RectTransform content, Order item) : base(prefab, content)
        {
            gameObject.transform.Find("Title").GetComponent<Text>().text = item.Name;

            designing = gameObject.transform.Find("Designing").GetComponent<Image>();
            designing.fillAmount = 0f;

            programming = gameObject.transform.Find("Programming").GetComponent<Image>();
            programming.fillAmount = 0f;

            texturing = gameObject.transform.Find("Texturing").GetComponent<Image>();
            texturing.fillAmount = 0f;

            testing = gameObject.transform.Find("Testing").GetComponent<Image>();
            testing.fillAmount = 0f;

            if (item.Money != 0)
            {
                gameObject.transform.Find("Reward").Find("Text").GetComponent<Text>().text = item.Money.ToString();
                gameObject.transform.Find("Reward").Find("Money").gameObject.SetActive(true);
                gameObject.transform.Find("Reward").Find("Premium").gameObject.SetActive(false);
            }
            else if (item.Premium != 0)
            {
                gameObject.transform.Find("Reward").Find("Text").GetComponent<Text>().text = item.Premium.ToString();
                gameObject.transform.Find("Reward").Find("Money").gameObject.SetActive(false);
                gameObject.transform.Find("Reward").Find("Premium").gameObject.SetActive(true);
            }
        }
    }

    public OrderAdapter() : base() { }

    protected override OrderView OnCreateView(RectTransform prefab, RectTransform content, Order item)
    {
        return new OrderView(prefab, content, item);
    }

    private List<Timer> timers = new List<Timer>();

    protected override void OnBindView(Order item, OrderView view, int position)
    {
        byte currentTicks = Config.BASE_ADAPTER_ANIMATION_TICKS;
        Timer timer = null;
        timer = new Timer();
        lock (timers)
        {
            timers.Add(timer);
        }
        timer.Elapsed += delegate
        {
            if (--currentTicks > 0 && view.gameObject != null)
            {
                synchronizationContext.Post(delegate
                {
                    if (view.gameObject != null)
                    {
                        view.designing.fillAmount = Mathf.Lerp(view.designing.fillAmount, item.Designing.Percent, Config.BASE_ADAPTER_ANIMATION_TICK_VALUE);
                        view.programming.fillAmount = Mathf.Lerp(view.programming.fillAmount, item.Programming.Percent, Config.BASE_ADAPTER_ANIMATION_TICK_VALUE);
                        view.texturing.fillAmount = Mathf.Lerp(view.texturing.fillAmount, item.Texturing.Percent, Config.BASE_ADAPTER_ANIMATION_TICK_VALUE);
                        view.testing.fillAmount = Mathf.Lerp(view.testing.fillAmount, item.Testing.Percent, Config.BASE_ADAPTER_ANIMATION_TICK_VALUE);
                    }
                }, null);
            }
            else
            {
                timer.Close();
                lock (timers)
                {
                    timers.Remove(timer);
                }
            }
        };
        timer.Interval = Config.BASE_ADAPTER_ANIMATION_TICK_TIME;
        timer.Start();
    }

    protected override void Start()
    {
        base.Start();
        Office office = GameModel.Get().Offices[0];
        List<Order> orders = office.Orders;
        office.OnOrderAdded += OrderAdded;
        office.OnOrderDeleted += OrderDeleted;
        foreach (Order obj in orders)
        {
            obj.OnOrderUpdated += OrderUpdated;
        }
        Dataset = orders;
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        foreach (Timer timer in timers)
        {
            timer.Close();
        }
    }

    private void OrderAdded(Office sender, Order obj1, int obj2)
    {
        ViewInserted(obj2);
        obj1.OnOrderUpdated += OrderUpdated;
    }

    private void OrderUpdated(Order sender)
    {
        ViewUpdated(Dataset.IndexOf(sender));
    }

    private void OrderDeleted(Office sender, Order obj1, int obj2)
    {
        ViewDestroyed(obj2);
    }
}
