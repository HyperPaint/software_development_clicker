using MyGame;
using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class OrderAdapter : BaseAdapter<Order, OrderAdapter.OrderView>
{
    public class OrderView : View
    {
        public Text title;
        public Image designing;
        public Image texturing;
        public Image programming;
        public Image testing;

        public OrderView(RectTransform prefab, RectTransform content) : base(prefab, content)
        {
            title = gameObject.transform.Find("Title").GetComponent<Text>();
            designing = gameObject.transform.Find("Designing").GetComponent<Image>();
            programming = gameObject.transform.Find("Programming").GetComponent<Image>();
            texturing = gameObject.transform.Find("Texturing").GetComponent<Image>();
            testing = gameObject.transform.Find("Testing").GetComponent<Image>();
        }
    }

    public OrderAdapter() : base() { }

    protected override OrderView OnCreateView(RectTransform prefab, RectTransform content)
    {
        return new OrderView(prefab, content);
    }

    protected override void OnBindView(Order item, OrderView view, int position)
    {
        view.title.text = item.Name;
        byte currentTicks = Config.BASE_ADAPTER_ANIMATION_TICKS;
        float designing = (item.Designing.Percent - view.designing.fillAmount) / currentTicks;
        float art = (item.Texturing.Percent - view.texturing.fillAmount) / currentTicks;
        float programming = (item.Programming.Percent - view.programming.fillAmount) / currentTicks;
        float testing = (item.Testing.Percent - view.testing.fillAmount) / currentTicks;
        Timer timer;
        timer = new Timer();
        timer.Elapsed += delegate
        {
            if (--currentTicks > 0 && view.gameObject != null)
            {
                synchronizationContext.Post(delegate
                {
                    view.designing.fillAmount += designing;
                    view.texturing.fillAmount += art;
                    view.programming.fillAmount += programming;
                    view.testing.fillAmount += testing;
                }, null);
                return;
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
