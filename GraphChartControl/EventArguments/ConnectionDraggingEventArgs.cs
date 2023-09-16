using System.Windows;

namespace Bau.Controls.GraphChartControl.EventArguments;

/// <summary>
///     Argumentos del evento lanzado mientras un usuario arrastra un nodo dentro de la red
/// </summary>
public class ConnectionDraggingEventArgs : ConnectionDragEventArgs
{
    internal ConnectionDraggingEventArgs(RoutedEvent routedEvent, object source, object node, object connection, object connector) 
                : base(routedEvent, source, node, connection, connector)
    {
    }
}
