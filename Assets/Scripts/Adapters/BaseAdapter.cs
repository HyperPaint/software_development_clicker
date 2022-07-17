using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class BaseAdapter<TYPE, VIEW> : MonoBehaviour where VIEW : BaseAdapter<TYPE, VIEW>.View
{
    public RectTransform prefab;
    public RectTransform content;

    public abstract class View
    {
        public GameObject gameObject;

        protected View(RectTransform prefab, RectTransform content)
        {
            gameObject = Instantiate(prefab.gameObject);
            gameObject.transform.SetParent(content);
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            Vector3 buff = gameObject.transform.localPosition;
            buff.z = 0;
            gameObject.transform.localPosition = buff;
        }
    }

    private List<TYPE> dataset;
    public List<TYPE> Dataset
    {
        get => dataset;
        set
        {
            dataset = value;
            DatasetChanged();
        }
    }

    private readonly List<VIEW> views = new();

    private static readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current;

    public void DatasetChanged()
    {
        synchronizationContext.Post(delegate
        {
            // удаление view
            foreach (VIEW item in views)
            {
                Destroy(item.gameObject);
            }
            // чистка
            views.Clear();
            // создание view 
            foreach (TYPE item in dataset)
            {
                views.Add(CreateView(prefab, content));
            }
            // бинд новых данных
            ViewsUpdated();
        }, null);
    }

    public void ViewsUpdated()
    {
        synchronizationContext.Post(delegate
        {
            for (int i = 0; i < dataset.Count; i++)
            {
                if (views[i].gameObject != null)
                    BindView(dataset[i], views[i]);
            }
        }, null);
    }

    public void ViewUpdated(int position)
    {
        synchronizationContext.Post(delegate
        {
            if (views[position].gameObject != null)
                BindView(dataset[position], views[position]);
        }, null);
    }

    public void ViewRangeUpdated(int position, int newPosition)
    {
        synchronizationContext.Post(delegate
        {
            for (int i = position; i <= newPosition; i++)
            {
                if (views[i].gameObject != null)
                    BindView(dataset[i], views[i]);
            }
        }, null);
    }

    public void ViewDestroyed(int position)
    {
        synchronizationContext.Post(delegate
        {
            // todo постараться сделать анимацию
            if (views[position].gameObject != null)
                Destroy(views[position].gameObject);
        }, null);
    }

    public abstract VIEW CreateView(RectTransform prefab, RectTransform content);

    public abstract void BindView(TYPE item, VIEW view);
}
