using MyGame;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class OrderAdapter : BaseAdapter<Order, OrderAdapter.OrderView>
{
    public class OrderView : View
    {
        [SerializeField] public Text name;
        [SerializeField] public Image designing;
        [SerializeField] public Image art;
        [SerializeField] public Image programming;
        [SerializeField] public Image testing;
        
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
        Timer timer = null;
        timer = new Timer();
        timer.Interval = Config.BASE_ADAPTER_ANIMATION_TICK_TIME;
        timer.Elapsed += delegate
        {
            synchronizationContext.Post(delegate
            {
                if (currentTicks > 0)
                {
                    view.designing.fillAmount = Mathf.Lerp(view.designing.fillAmount, item.Designing.Percent, Time.deltaTime);
                    view.art.fillAmount = Mathf.Lerp(view.art.fillAmount, item.Art.Percent, Time.deltaTime);
                    view.programming.fillAmount = Mathf.Lerp(view.programming.fillAmount, item.Programming.Percent, Time.deltaTime);
                    view.testing.fillAmount = Mathf.Lerp(view.testing.fillAmount, item.Testing.Percent, Time.deltaTime);
                }
                else
                {
                    timer.Stop();
                    timer = null;
                }
                currentTicks--;
            }, null);
        };
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
