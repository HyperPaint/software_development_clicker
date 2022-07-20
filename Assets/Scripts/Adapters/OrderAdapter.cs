using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame;

public class OrderAdapter : BaseAdapter<Order, OrderAdapter.OrderView>
{
    public class OrderView : View
    {
        public Text name;
        public Text designing;
        public Text art;
        public Text programming;
        public Text testing;
        
        public OrderView(RectTransform prefab, RectTransform content) : base(prefab, content)
        {
            name = gameObject.transform.Find("NameBackground").Find("Name").GetComponent<Text>();
            designing = gameObject.transform.Find("DesigningProgress").Find("Designing").GetComponent<Text>();
            art = gameObject.transform.Find("ArtProgress").Find("Art").GetComponent<Text>();
            programming = gameObject.transform.Find("ProgrammingProgress").Find("Programming").GetComponent<Text>();
            testing = gameObject.transform.Find("TestingProgress").Find("Testing").GetComponent<Text>();
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
        view.designing.text = (Convert.ToSingle(item.Designing.current) / Convert.ToSingle(item.Designing.needed) * 100f).ToString() + "%";
        view.art.text = (Convert.ToSingle(item.Art.current) / Convert.ToSingle(item.Art.needed) * 100f).ToString() + "%";
        view.programming.text = (Convert.ToSingle(item.Programming.current) / Convert.ToSingle(item.Programming.needed) * 100f).ToString() + "%";
        view.testing.text = (Convert.ToSingle(item.Testing.current) / Convert.ToSingle(item.Testing.needed) * 100f).ToString() + "%";
    }


    private void Start()
    {
        Office office = GameModel.Get().Offices[0];
        List<Order> orders = office.Orders;
        office.OrderAdded += (sender, obj) =>
        {
            ViewInserted(Dataset.IndexOf(obj), false);
            obj.OrderUpdated += (sender) =>
            {
                ViewUpdated(Dataset.IndexOf(obj));
            };
        };
        office.OrderDeleted += (sender, obj) =>
        {
            ViewDestroyed(obj);
        };
        foreach (Order obj in orders)
        {
            obj.OrderUpdated += (sender) =>
            {
                ViewUpdated(Dataset.IndexOf(obj));
            };
        }
        Dataset = orders;
    }
}
