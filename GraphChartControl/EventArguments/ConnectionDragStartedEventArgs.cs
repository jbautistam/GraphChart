using System.Windows;

namespace Bau.Controls.GraphChartControl.EventArguments;

/// <summary>
///     Argumentos del evento lanzado cuando el usuario comienza a arrastrar una conexión fuera de un nodo
/// </summary>
public class ConnectionDragStartedEventArgs : ConnectionDragEventArgs
{
    internal ConnectionDragStartedEventArgs(RoutedEvent routedEvent, object source, object node, object connector) 
        : base(routedEvent, source, node, null, connector) 
    {
    }
}
