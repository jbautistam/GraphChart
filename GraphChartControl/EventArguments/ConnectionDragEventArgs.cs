using System.Windows;

namespace Bau.Controls.GraphChartControl.EventArguments;

/// <summary>
///     Clase base para los argumentos del evento de arrastrar una conexión
/// </summary>
public class ConnectionDragEventArgs : RoutedEventArgs
{
    protected ConnectionDragEventArgs(RoutedEvent routedEvent, object source, object node, object connection, object connector) : base(routedEvent, source)
    {
        Node = node;
        ConnectorDraggedOut = connector;
        Connection = connection;
    }

    /// <summary>
    ///     El conector que se está arrastrando
    /// </summary>
    public object Connection { get; set; }

    /// <summary>
    ///     El NodeItem o su DataContext (cuando no sea null)
    /// </summary>
    public object Node { get; }

    /// <summary>
    ///     El ConnectorItem o su DataContext (cuando no sea null)
    /// </summary>
    public object ConnectorDraggedOut { get; }
}

