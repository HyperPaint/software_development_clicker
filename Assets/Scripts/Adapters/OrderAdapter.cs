using MyGame;
using System;
using System.Collections.Generic;
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
        view.designing.fillAmount = Convert.ToSingle(item.Designing.current) / Convert.ToSingle(item.Designing.needed);
        view.art.fillAmount = Convert.ToSingle(item.Art.current) / Convert.ToSingle(item.Art.needed);
        view.programming.fillAmount = Convert.ToSingle(item.Programming.current) / Convert.ToSingle(item.Programming.needed);
        view.testing.fillAmount = Convert.ToSingle(item.Testing.current) / Convert.ToSingle(item.Testing.needed);
    }

    private void Start()
    {
        Office office = GameModel.Get().Offices[0];
        List<Order> orders = office.Orders;
        office.OrderAdded += OrderAdded;
        office.OrderDeleted += OrderDeleted;
        foreach (Order obj in orders)
        {
            obj.OrderUpdated += OrderUpdated;
        }
        Dataset = orders;
    }

    private void OrderAdded(Office sender, Order obj1, int obj2)
    {
        ViewInserted(obj2);
        obj1.OrderUpdated += OrderUpdated;
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
