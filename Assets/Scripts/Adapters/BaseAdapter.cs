using MyGame;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Базовый типизированный класс адаптера.
/// </summary>
/// <typeparam name="TYPE">Тип адаптируемого класса.</typeparam>
/// <typeparam name="VIEW">Модель компонента интерфейса. Обязательно наследуется от <see cref="View"/></typeparam>
public abstract class BaseAdapter<TYPE, VIEW> : MonoBehaviour where VIEW : BaseAdapter<TYPE, VIEW>.View
{
    /// <summary>
    /// Префаб создаваемого объекта. Необходимо добавить через интерфейс Unity.
    /// </summary>
    public RectTransform prefab;
    /// <summary>
    /// Контейнер для создаваемых объектов. Необходимо добавить через интерфейс Unity.
    /// </summary>
    public RectTransform content;

    /// <summary>
    /// Базовый класс модели компонента интерфейса.
    /// Класс должен осуществлять привязку к компонентам интерфейса Unity.
    /// Это позволит далее использовать заранее найденные компоненты.
    /// </summary>
    public abstract class View
    {
        /// <summary>
        /// Привязанный объект интерфейса Unity.
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// Конструктор с параметрами. Здесь необходимо инициализировать поля класса.
        /// </summary>
        /// <param name="prefab">Префаб создаваемого объекта. Необходимо добавить через интерфейс Unity.</param>
        /// <param name="content">Контейнер для создаваемых объектов. Необходимо добавить через интерфейс Unity.</param>
        protected View(RectTransform prefab, RectTransform content)
        {
            gameObject = Instantiate(prefab.gameObject);
            gameObject.transform.SetParent(content);
            // установка нормальных значений
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            Vector3 vector3Buff = gameObject.transform.localPosition;
            vector3Buff.z = 0;
            gameObject.transform.localPosition = vector3Buff;
        }
    }

    /// <summary>
    /// Набор данных.
    /// </summary>
    private List<TYPE> dataset;
    /// <summary>
    /// Открытое свойство для задания и получения набора данных.
    /// </summary>
    public List<TYPE> Dataset
    {
        get => dataset;
        set
        {
            dataset = value;
            DatasetChanged();
        }
    }
    /// <summary>
    /// Список моделей компонентов интерфейса.
    /// </summary>
    private readonly List<VIEW> views = new();

    /// <summary>
    /// Контекст синхронизации, используется для выполнения кода в главном потоке.
    /// </summary>
    protected static System.Threading.SynchronizationContext synchronizationContext;

    /// <summary>
    /// Сообщает адаптеру, что набор данных полностью изменён.
    /// Настолько, что невозможно использовать специфичные методы.
    /// После чего адаптер перестраивает интерфейс согласно набору данных.
    /// Может серьезно тормозить поток.
    /// </summary>
    public void DatasetChanged()
    {
        synchronizationContext.Post(delegate
        {
            foreach (VIEW item in views)
            {
                Destroy(item.gameObject);
            }
            views.Clear();
            foreach (TYPE item in dataset)
            {
                views.Add(OnCreateView(prefab, content));
            }
            for (int i = 0; i < dataset.Count; i++)
            {
                OnBindView(dataset[i], views[i], i);
            }
        }, null);
    }

    /// <summary>
    /// Сообщает адаптеру, что данные в наборе данных изменены.
    /// После чего адаптер заполняет все компоненты интерфейса согласно набору данных.
    /// Может серьезно тормозить главный поток.
    /// </summary>
    public void ViewsUpdated()
    {
        synchronizationContext.Post(delegate
        {
            for (int i = 0; i < dataset.Count; i++)
            {
                OnBindView(dataset[i], views[i], i);
            }
        }, null);
    }

    /// <summary>
    /// Сообщает адаптеру, что данные на указанной позиции в наборе данных изменены.
    /// После чего адаптер заполняет указанный компонент интерфейса согласно набору данных.
    /// </summary>
    /// <param name="position">Позиция компонента в наборе данных.</param>
    public void ViewUpdated(int position)
    {
        synchronizationContext.Post(delegate
        {
            if (dataset.Count > position && views.Count > position)
            {
                OnBindView(dataset[position], views[position], position);
            }
        }, null);
    }

    /// <summary>
    /// Сообщает адаптеру, что данные на указанных позициях в наборе данных изменены.
    /// После чего адаптер заполняет указанные компоненты интерфейса согласно набору данных.
    /// </summary>
    /// <param name="from">Стартовая позиция компонентов в наборе данных.</param>
    /// <param name="to">Конечная позиция компонентов в наборе данных.</param>
    public void ViewRangeUpdated(int from, int to)
    {
        synchronizationContext.Post(delegate
        {
            if (from < 0) from = 0;
            if (from >= dataset.Count) from = dataset.Count - 1;
            if (to < 0) to = 0;
            if (to >= dataset.Count) to = dataset.Count - 1;
            for (int i = from; i <= to; i++)
            {
                OnBindView(dataset[i], views[i], i);
            }
        }, null);
    }

    /// <summary>
    /// Сообщает адаптеру, что на указанную позицию (начало или конец списка) добавлен компонент в набор данных.
    /// </summary>
    /// <param name="position">Позиция компонента в наборе данных.</param>
    /// <param name="setFirstSibling">Установить компонент интерфейса в начало списка?</param>
    public async void ViewInserted(int position, bool setFirstSibling = false)
    {
        VIEW view = null;
        synchronizationContext.Post(delegate
        {
            view = OnCreateView(prefab, content);
            views.Insert(position, view);
            if (setFirstSibling)
                view.gameObject.transform.SetAsFirstSibling();
            else
                view.gameObject.transform.SetAsLastSibling();
            Vector3 scale = view.gameObject.transform.localScale;
            scale.x = 0f;
            scale.y = 0f;
            view.gameObject.transform.localScale = scale;
            OnBindView(dataset[position], view, position);
        }, null);
        await Task.Delay(Config.BASE_ADAPTER_ANIMATION_WAIT_TIME);
        // анимация
        synchronizationContext.Post(delegate
        {
            byte currentTicks = Config.BASE_ADAPTER_ANIMATION_TICKS;
            Timer timer = null;
            timer = new Timer();
            timer.Interval = Config.BASE_ADAPTER_ANIMATION_TICK_TIME;
            timer.Elapsed += delegate
            {
                synchronizationContext.Post(delegate
                {
                    if (--currentTicks > 0 && view.gameObject != null)
                    {
                        Vector3 scale = view.gameObject.transform.localScale;
                        scale.x += Config.BASE_ADAPTER_ANIMATION_TICK_VALUE;
                        scale.y += Config.BASE_ADAPTER_ANIMATION_TICK_VALUE;
                        view.gameObject.transform.localScale = scale;
                        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
                        return;
                    }
                    timer.Stop();
                    timer = null;
                }, null);
            };
            timer.Start();
        }, null);
    }

    /// <summary>
    /// Сообщает адаптеру, что c указанной позиции удалён компонент из набора данных.
    /// В интерфейсе Unity объект удалится, остальные сдвинутся.
    /// </summary>
    /// <param name="position">Позиция компонента в наборе данных.</param>
    public async void ViewDestroyed(int position)
    {
        VIEW view = views[position];
        views.RemoveAt(position);
        await Task.Delay(Config.BASE_ADAPTER_ANIMATION_WAIT_TIME);
        synchronizationContext.Post(delegate
        {
            // анимация
            byte currentTicks = Config.BASE_ADAPTER_ANIMATION_TICKS;
            Timer timer = null;
            timer = new Timer();
            timer.Interval = Config.BASE_ADAPTER_ANIMATION_TICK_TIME;
            timer.Elapsed += delegate
            {
                synchronizationContext.Post(delegate
                {
                    if (--currentTicks > 0 && view.gameObject != null)
                    {
                        Vector3 scale = view.gameObject.transform.localScale;
                        scale.x -= Config.BASE_ADAPTER_ANIMATION_TICK_VALUE;
                        scale.y -= Config.BASE_ADAPTER_ANIMATION_TICK_VALUE;
                        view.gameObject.transform.localScale = scale;
                        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
                        return;
                    }
                    Destroy(view.gameObject);
                    timer.Stop();
                    timer = null;
                }, null);
            };
            timer.Start();
        }, null);
    }

    /// <summary>
    /// Функция для создания экземпляра класса и его дополнительной инициализации.
    /// </summary>
    /// <param name="prefab">Префаб создаваемого объекта. Необходимо добавить через интерфейс Unity.</param>
    /// <param name="content">Контейнер для создаваемых объектов. Необходимо добавить через интерфейс Unity.</param>
    /// <returns>Экземпляр класса <see cref="View"/>.</returns>
    protected abstract VIEW OnCreateView(RectTransform prefab, RectTransform content);

    /// <summary>
    /// Функция для наполнения интерфейса данными. Заполняет один компонент интерфейса одним экземпляром класса.
    /// Здесь же можно привязываться к событиям интерфейса Unity через <see cref="View.gameObject"/>.
    /// </summary>
    /// <param name="item">Экземпляр класса <see cref="TYPE"/> из заранее выданного набора данных.</param>
    /// <param name="view">Заранее созданный экземпляр класса <see cref="VIEW"/> с привязанными компонентами интерфейса.</param>
    /// <param name="position">Позиция компонента в наборе данных.</param>
    protected abstract void OnBindView(TYPE item, VIEW view, int position);

    protected virtual void Start()
    {
        synchronizationContext = System.Threading.SynchronizationContext.Current;
    }

    protected virtual void OnApplicationQuit()
    {
        
    }
}
