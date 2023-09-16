using System.Windows;

namespace Bau.Controls.GraphChartControl.EventArguments;

/// <summary>
///     Argumentos del evento lanzado mientras un usuario arrastra un nodo en la red
/// </summary>
internal class ConnectorItemDraggingEventArgs : RoutedEventArgs
{
    internal ConnectorItemDraggingEventArgs(RoutedEvent routedEvent, object source, double horizontalChange, double verticalChange) : base(routedEvent, source)
    {
        HorizontalChange = horizontalChange;
        VerticalChange = verticalChange;
    }

    /// <summary>
    ///     Cantidad que se ha arrastrado el nodo horizontalmente
    /// </summary>
    internal double HorizontalChange { get; }

    /// <summary>
    ///     Cantidad que se ha arrastrado el nodo verticalmente
    /// </summary>
    internal double VerticalChange { get; }
}
