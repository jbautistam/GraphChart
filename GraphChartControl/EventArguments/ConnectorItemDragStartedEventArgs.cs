using System.Windows;

namespace Bau.Controls.GraphChartControl.EventArguments;

/// <summary>
///     Argumentos de los eventos lanzados cuando el usuario comienza a arrastrar un conectors desde un nodo
/// </summary>
internal class ConnectorItemDragStartedEventArgs : RoutedEventArgs
{
    internal ConnectorItemDragStartedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }

    /// <summary>
    ///     Cancela la acción de arrastre fuera del conector
    /// </summary>
    internal bool Cancel { get; set; }
}
