using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Пример для демострации работы.
/// Адаптируемый класс.
/// Содержит единственное поле типа byte.
/// </summary>
public class Example
{
    /// <summary>
    /// Единственное поле класса типа byte.
    /// </summary>
    public byte example;

    /// <summary>
    /// Конструктор без параметров.
    /// </summary>
    public Example() : this(0) { }

    /// <summary>
    /// Конструктор с параметрами.
    /// </summary>
    /// <param name="example">Единственное поле класса типа byte.</param>
    public Example(byte example)
    {
        this.example = example;
    }
}

/// <summary>
/// Пример для демострации работы.
/// </summary>
public class ExampleAdapter : BaseAdapter<Example, ExampleAdapter.ExampleView>
{
    /// <summary>
    /// Модель компонента интерфейса.
    /// Класс должен осуществлять привязку к компонентам интерфейса Unity.
    /// Это позволит далее использовать заранее найденные компоненты.
    /// Класс должен наследоваться от <see cref="BaseAdapter{TYPE, VIEW}.View"/>
    /// </summary>
    public class ExampleView : View
    {
        /// <summary>
        /// Ссылка на компонент интерфейса, для наполнения данными в <see cref="BindView(Example, ExampleView)"/>
        /// </summary>
        public Text exampleText;

        /// <summary>
        /// Конструктор с параметрами. Здесь необходимо инициализировать поля класса.
        /// </summary>
        /// <param name="prefab">Префаб создаваемого объекта, используется в базовом классе. Необходимо добавить через интерфейс Unity.</param>
        /// <param name="content">Контейнер для создаваемых объектов, используется в базовом классе. Необходимо добавить через интерфейс Unity.</param>
        public ExampleView(RectTransform prefab, RectTransform content) : base(prefab, content)
        {
            // инициализация поля класса
            exampleText = gameObject.transform.Find("Byte").GetComponent<Text>();
        }
    }

    /// <summary>
    /// Конструктор без параметров.
    /// </summary>
    public ExampleAdapter() : base() { }

    /// <summary>
    /// Функция для создания экземпляра класса и его дополнительной инициализации.
    /// </summary>
    /// <param name="prefab">Префаб создаваемого объекта, используется в базовом классе. Необходимо добавить через интерфейс Unity.</param>
    /// <param name="content">Контейнер для создаваемых объектов, используется в базовом классе. Необходимо добавить через интерфейс Unity.</param>
    /// <returns>Экземпляр класса <see cref="ExampleView"/>.</returns>
    protected override ExampleView OnCreateView(RectTransform prefab, RectTransform content)
    {
        return new ExampleView(prefab, content);
    }

    /// <summary>
    /// Функция для наполнения интерфейса данными. Заполняет один компонент интерфейса одним экземпляром класса.
    /// </summary>
    /// <param name="item">Экземпляр класса <see cref="Example"/> из заранее выданного набора данных.</param>
    /// <param name="view">Заранее созданный экземпляр класса <see cref="ExampleView"/> с привязанными компонентами интерфейса.</param>
    /// <param name="position">Позиция компонента в наборе данных.</param>
    protected override void OnBindView(Example item, ExampleView view, int position)
    {
        // наполнение интерфейса данными
        view.exampleText.text = item.example.ToString();
    }

    /// <summary>
    /// Опциональная функция для инициализации <see cref="ExampleAdapter"/>. 
    /// </summary>
    protected override void Start()
    {
        List<Example> list = new List<Example>();
        list.Add(new Example(1));
        list.Add(new Example(2));
        list.Add(new Example(3));
        Dataset = list;
    }
}
