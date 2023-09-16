using System.Windows;

namespace Bau.Controls.GraphChartControl.EventArguments;

/// <summary>
/// Arguments for event raised when the user starts to drag a connection out from a node.
/// </summary>
public class ConnectionDragStartedEventArgs : ConnectionDragEventArgs
{
    internal ConnectionDragStartedEventArgs(RoutedEvent routedEvent, object source, object node, object connector) :
        base(routedEvent, source, node, null, connector)
    {
    }
}
