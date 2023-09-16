using System.Collections;

namespace BauMvvm.Views.Wpf;

/// <summary>
///     Argumentos para los eventos ItemsAdded e ItemsRemoved
/// </summary>
public class CollectionItemsChangedEventArgs : EventArgs
{
    // Variables privadas
    private ICollection? _items = null;

    public CollectionItemsChangedEventArgs(ICollection items)
    {
        _items = items;
    }

    /// <summary>
    ///     La colección de elementos modificados
    /// </summary>
    public ICollection? Items => _items;
}
