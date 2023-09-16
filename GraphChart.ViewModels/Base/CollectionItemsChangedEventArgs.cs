using System.Collections;

namespace Bau.Libraries.GraphChart.ViewModels.Base;

/// <summary>
///     Argumentos para los eventos ItemsAdded e ItemsRemoved
/// </summary>
public class CollectionItemsChangedEventArgs : EventArgs
{
    public CollectionItemsChangedEventArgs(ICollection items)
    {
        Items = items;
    }

    /// <summary>
    ///     La colección de elementos modificados
    /// </summary>
    public ICollection Items { get; }
}
