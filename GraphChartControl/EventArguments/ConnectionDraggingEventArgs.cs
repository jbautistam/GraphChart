using System.Windows;

namespace Bau.Controls.GraphChartControl.EventArguments;

/// <summary>
/// Arguments for event raised while user is dragging a node in the network.
/// </summary>
public class ConnectionDraggingEventArgs : ConnectionDragEventArgs
{
    internal ConnectionDraggingEventArgs(RoutedEvent routedEvent, object source, object node, object connection, object connector) 
                : base(routedEvent, source, node, connection, connector)
    {
    }
}
