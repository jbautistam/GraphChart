using System.Windows;

namespace Bau.Controls.GraphChartControl.EventArguments;

/// <summary>
/// Arguments for event raised when the user has completed dragging a connector.
/// </summary>
public class ConnectionDragCompletedEventArgs : ConnectionDragEventArgs
{
    internal ConnectionDragCompletedEventArgs(RoutedEvent routedEvent, object source, object node, object connection, object connector, object connectorDraggedOver) :
        base(routedEvent, source, node, connection, connector)
    {
        ConnectorDraggedOver = connectorDraggedOver;
    }

    /// <summary>
    /// The ConnectorItem or it's DataContext (when non-NULL).
    /// </summary>
    public object ConnectorDraggedOver { get; }
}
