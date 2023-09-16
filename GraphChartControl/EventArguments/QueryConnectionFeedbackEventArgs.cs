using System.Windows;

namespace Bau.Controls.GraphChartControl.EventArguments;

/// <summary>
/// Arguments for event raised while user is dragging a node in the network.
/// </summary>
public class QueryConnectionFeedbackEventArgs : ConnectionDragEventArgs
{
    internal QueryConnectionFeedbackEventArgs(RoutedEvent routedEvent, object source, 
        object node, object connection, object connector, object draggedOverConnector) :
        base(routedEvent, source, node, connection, connector)
    {
        DraggedOverConnector = draggedOverConnector;
    }

    /// <summary>
    /// The ConnectorItem or it's DataContext (when non-NULL).
    /// </summary>
    public object DraggedOverConnector { get; }

    /// <summary>
    /// Set to 'true' / 'false' to indicate that the connection from the dragged out connection to the dragged over connector is valid.
    /// </summary>
    public bool ConnectionOk { get; set; }

    /// <summary>
    /// The indicator to display.
    /// </summary>
    public object? FeedbackIndicator { get; set; }
}
