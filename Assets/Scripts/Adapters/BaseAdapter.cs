using System.Collections.Generic;
using System.Threading;
using UnityEngine;

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
    private static readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current;

    /// <summary>
    /// Сообщает адаптеру, что набор данных изменён.
    /// После чего адаптер перестраивает интерфейс согласно набору данных.
    /// Может серьезно тормозить поток.
    /// </summary>
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

    /// <summary>
    /// Сообщает адаптеру, что данные в наборе данных изменены.
    /// После чего адаптер запоняет все компоненты интерфейса согласно набору данных.
    /// Может серьезно тормозить поток.
    /// </summary>
    public void ViewsUpdated()
    {
        synchronizationContext.Post(delegate
        {
            for (int i = 0; i < dataset.Count; i++)
            {
                BindView(dataset[i], views[i], i);
            }
        }, null);
    }

    /// <summary>
    /// Сообщает адаптеру, что данные на указанной позиции в наборе данных изменены.
    /// После чего адаптер заполняет указанный компонент интерфейса согласно набору данных.
    /// </summary>
    /// <param name="position">Позиция компонента в интерфейсе или наборе данных.</param>
    public void ViewUpdated(int position)
    {
        synchronizationContext.Post(delegate
        {
            BindView(dataset[position], views[position], position);
        }, null);
    }

    /// <summary>
    /// Сообщает адаптеру, что данные на указанных позициях в наборе данных изменены.
    /// После чего адаптер заполняет указанные компоненты интерфейса согласно набору данных.
    /// </summary>
    /// <param name="from">Стартовая позиция компонентов в интерфейсе или наборе данных.</param>
    /// <param name="to">Конечная позиция компонентов в интерфейсе или наборе данных.</param>
    public void ViewRangeUpdated(int from, int to)
    {
        synchronizationContext.Post(delegate
        {
            for (int i = from; i <= to; i++)
            {
                BindView(dataset[i], views[i], i);
            }
        }, null);
    }

    /// <summary>
    /// Сообщает адаптеру, что на указанную позицию добавлен компонент в набор данных.
    /// В интерфейсе Unity объект появится в конце списка.
    /// </summary>
    /// <param name="position">Позиция компонента в наборе данных.</param>
    public void ViewInserted(int position)
    {
        synchronizationContext.Post(delegate
        {
            // todo постараться сделать анимацию
            views.Insert(position, CreateView(prefab, content));
            BindView(dataset[position], views[position], position);
        }, null);
    }

    /// <summary>
    /// Сообщает адаптеру, что c указанной позиции удалён компонент из набора данных.
    /// В интерфейсе Unity объект удалится, остальные сдвинутся.
    /// </summary>
    /// <param name="position">Позиция компонента в интерфейсе или наборе данных.</param>
    public void ViewDestroyed(int position)
    {
        synchronizationContext.Post(delegate
        {
            // todo постараться сделать анимацию
            Destroy(views[position].gameObject);
        }, null);
    }

    /// <summary>
    /// Функция для создания экземпляра класса и его дополнительной инициализации.
    /// </summary>
    /// <param name="prefab">Префаб создаваемого объекта. Необходимо добавить через интерфейс Unity.</param>
    /// <param name="content">Контейнер для создаваемых объектов. Необходимо добавить через интерфейс Unity.</param>
    /// <returns>Экземпляр класса <see cref="View"/>.</returns>
    public abstract VIEW CreateView(RectTransform prefab, RectTransform content);

    /// <summary>
    /// Функция для наполнения интерфейса данными. Заполняет один компонент интерфейса одним экземпляром класса.
    /// Здесь же можно привязываться к событиям интерфейса Unity через <see cref="View.gameObject"/>.
    /// </summary>
    /// <param name="item">Экземпляр класса <see cref="TYPE"/> из заранее выданного набора данных.</param>
    /// <param name="view">Заранее созданный экземпляр класса <see cref="VIEW"/> с привязанными компонентами интерфейса.</param>
    /// <param name="position">Позиция компонента в наборе данных.</param>
    public abstract void BindView(TYPE item, VIEW view, int position);
}
