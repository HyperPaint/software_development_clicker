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

    public override OrderView CreateView(RectTransform prefab, RectTransform content)
    {
        return new OrderView(prefab, content);
    }

    public override void BindView(Order item, OrderView view)
    {
        view.name.text = item.Name;
        view.designing.text = (Convert.ToSingle(item.Designing.current) / Convert.ToSingle(item.Designing.needed) * 100f).ToString() + "%";
        view.art.text = (Convert.ToSingle(item.Art.current) / Convert.ToSingle(item.Art.needed) * 100f).ToString() + "%";
        view.programming.text = (Convert.ToSingle(item.Programming.current) / Convert.ToSingle(item.Programming.needed) * 100f).ToString() + "%";
        view.testing.text = (Convert.ToSingle(item.Testing.current) / Convert.ToSingle(item.Testing.needed) * 100f).ToString() + "%";
    }

    private void Start()
    {
        // todo сделать класс для хранения игровых данных по типу такой то офис видно на экране и т.д. на него ссылаться тут при отображении пула заказов
        List<Order> orders = GameModel.Get().Offices[0].Orders;
        OrderFactory.Get().OrderCreated += (sender, created) =>
        {
            DatasetChanged();
            (created as Order).OrderUpdated += (sender) =>
            {
                ViewsUpdated();
            };
        };
        foreach (var item in orders)
        {
            item.OrderUpdated += (sender) =>
            {
                ViewsUpdated();
            };
        }
        Dataset = orders;
    }
}
