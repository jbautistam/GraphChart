using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Specialized;

namespace Bau.Libraries.GraphChart.ViewModels.Base;

/// <summary>
///     Implementación de una colección observable que contiene una lista interna duplicada
/// retenida en memoria hasta que se limpia la lista
///     Así los observadores pueden por ejemplo deshacer eventos una vez que la lista se
/// ha limpiado y se ha lanzado un evento CollectionChanged con una acción Reset
/// </summary>
public class ImpObservableCollection<TypeData> : ObservableCollection<TypeData>, ICloneable
{
    // Eventos públicos
    public event EventHandler<CollectionItemsChangedEventArgs>? ItemsAdded;
    public event EventHandler<CollectionItemsChangedEventArgs>? ItemsRemoved;
    // Variables privadas
    private bool _inCollectionChangedEvent;

    public ImpObservableCollection() {}

    public ImpObservableCollection(IEnumerable<TypeData> range) : base(range) {}

    public ImpObservableCollection(IList<TypeData> list) : base(list)
    {
        InnerList.AddRange(list);
    }

    /// <summary>
    ///     Añade una serie de elementos
    /// </summary>
    public void AddRange(TypeData[] range)
    {
        foreach (TypeData item in range)
            Add(item);
    }

    /// <summary>
    ///     Añade una serie de elementos
    /// </summary>
    public void AddRange(IEnumerable range)
    {
        foreach (TypeData item in range)
            Add(item);
    }

    /// <summary>
    ///     Añade una serie de elementos
    /// </summary>
    public void AddRange(ICollection<TypeData> range)
    {
        foreach (TypeData item in range)
            Add(item);
    }

    /// <summary>
    ///     Elimina una serie de elementos
    /// </summary>
    public void RemoveRange(TypeData[] range)
    {
        foreach (TypeData item in range)
            Remove(item);
    }

    /// <summary>
    ///     Elimina una serie de elementos
    /// </summary>
    public void RemoveRange(IEnumerable range)
    {
        foreach (TypeData item in range)
            Remove(item);
    }

    /// <summary>
    ///     Elimina una serie de elementos
    /// </summary>
    public void RemoveRange(ImpObservableCollection<TypeData> range)
    {
        foreach (TypeData item in range)
            Remove(item);
    }

    /// <summary>
    ///     Elimina una serie de elementos
    /// </summary>
    public void RemoveRangeAt(int index, int count)
    {
        for (int item = 0; item < count; item++)
            RemoveAt(item + index);
    }

    /// <summary>
    ///     Elimina una serie de elementos
    /// </summary>
    public void RemoveRange(ICollection<TypeData> range)
    {
        foreach (TypeData item in range)
            Remove(item);
    }

    /// <summary>
    ///     Elimina una serie de elementos
    /// </summary>
    public void RemoveRange(ICollection range)
    {
        foreach (TypeData item in range)
            Remove(item);
    }

    /// <summary>
    ///     Trata el evento de modificación de la selección
    /// </summary>
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        // Trata el evento en la base
        base.OnCollectionChanged(e);
        // Indica que se está tratando el elemento
        _inCollectionChangedEvent = true;
        // Trata el evento
        try
        {
            // Reinicia la lista interna
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {  
                // Llama al evento que indica que se están borrando datos
                if (InnerList.Count > 0)
                    OnItemsRemoved(InnerList);
                // Limpia la lista
                InnerList.Clear();
            }
            // Elimina los elementos antiguos de la lista de respaldo
            if (e.OldItems != null)
            {
                // Elimina la lista
                foreach (TypeData item in e.OldItems)
                    InnerList.Remove(item);
                // Indica que se han eliminado
                OnItemsRemoved(e.OldItems);
            }
            // Añade nuevos elementos a la lista de respaldo
            if (e.NewItems != null)
            {  
                // Añade los elementos
                foreach (TypeData item in e.NewItems)
                    InnerList.Add(item);
                // Inica que se han añadido
                OnItemsAdded(e.NewItems);
            }
        }
        catch (Exception exception)
        {
            System.Diagnostics.Trace.WriteLine($"Error when treat OnCollectionChange. {exception.Message}");
        }
        // Indica que se ha terminado de tratar el evento
        _inCollectionChangedEvent = false;
    }

    /// <summary>
    ///     Lanza el evento ItemsChanged
    /// </summary>
    protected virtual void OnItemsAdded(ICollection items)
    {
		ItemsAdded?.Invoke(this, new CollectionItemsChangedEventArgs(items));
	}

    /// <summary>
    ///     Lanza el evento <see cref="ItemsRemoved"/>
    /// </summary>
    protected virtual void OnItemsRemoved(ICollection items)
    {
		ItemsRemoved?.Invoke(this, new CollectionItemsChangedEventArgs(items));
	}

    /// <summary>
    ///     Convierte la lista en un array
    /// </summary>
    public TypeData[] ToArray() => InnerList.ToArray();

    /// <summary>
    ///     Convierte los datos en otro tipo
    /// </summary>
    public IEnumerable<TypeOutput> Convert<TypeOutput>() where TypeOutput : class
    {
        foreach (TypeData item in this)
            if (item is TypeOutput output)
                yield return output;
    }

    /// <summary>
    ///     Clona la lista
    /// </summary>
    public object Clone()
    {
        ImpObservableCollection<object> clone = new ImpObservableCollection<object>();

            // Rellena la lista de salida
            foreach (TypeData item in this)
                if (item is ICloneable output)
                    clone.Add((TypeData) output.Clone());
            // Devuelve los elementos clonados
            return clone;
    }

    /// <summary>
    ///     Lista interna
    /// </summary>
    private List<TypeData> InnerList = new();
}
