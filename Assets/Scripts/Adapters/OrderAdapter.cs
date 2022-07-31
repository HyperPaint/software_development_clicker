using MyGame;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class OrderAdapter : BaseAdapter<Order, OrderAdapter.OrderView>
{
    public class OrderView : View
    {
        public Text name;
        public Image designing;
        public Image art;
        public Image programming;
        public Image testing;
        
        public OrderView(RectTransform prefab, RectTransform content) : base(prefab, content)
        {
            name = gameObject.transform.Find("NameBackground").Find("Name").GetComponent<Text>();
            designing = gameObject.transform.Find("BackGround (1)").Find("Designing_progress").GetComponent<Image>();
            art = gameObject.transform.Find("BackGround (3)").Find("Art_progress").GetComponent<Image>();
            programming = gameObject.transform.Find("BackGround").Find("Programming_progress").GetComponent<Image>();
            testing = gameObject.transform.Find("BackGround (2)").Find("Testing_progress").GetComponent<Image>();
        }
    }

    public OrderAdapter() : base() { }

    protected override OrderView OnCreateView(RectTransform prefab, RectTransform content)
    {
        return new OrderView(prefab, content);
    }

    protected override void OnBindView(Order item, OrderView view, int position)
    {
        view.name.text = item.Name;
        byte currentTicks = Config.BASE_ADAPTER_ANIMATION_TICKS;
        Timer timer;
        timer = new Timer();
        timer.Elapsed += delegate
        {
            if (currentTicks-- > 0)
            {
                if (view.gameObject != null)
                {
                    synchronizationContext.Post(delegate
                    {
                        view.designing.fillAmount = Mathf.Lerp(view.designing.fillAmount, item.Designing.Percent, Config.BASE_ADAPTER_ANIMATION_TICK_VALUE);
                        view.art.fillAmount = Mathf.Lerp(view.art.fillAmount, item.Art.Percent, Config.BASE_ADAPTER_ANIMATION_TICK_VALUE);
                        view.programming.fillAmount = Mathf.Lerp(view.programming.fillAmount, item.Programming.Percent, Config.BASE_ADAPTER_ANIMATION_TICK_VALUE);
                        view.testing.fillAmount = Mathf.Lerp(view.testing.fillAmount, item.Testing.Percent, Config.BASE_ADAPTER_ANIMATION_TICK_VALUE);
                    }, null);
                    return;
                }
            }
            timer.Stop();
            timer = null;
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
