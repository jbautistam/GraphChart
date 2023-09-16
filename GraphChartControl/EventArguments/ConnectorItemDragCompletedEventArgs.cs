using System.Windows;

namespace Bau.Controls.GraphChartControl.EventArguments;

/// <summary>
///     Argumentos del evento lanzado cuando el usuario ha finalizado de arrastrar el conector
/// </summary>
internal class ConnectorItemDragCompletedEventArgs : RoutedEventArgs
{
    internal ConnectorItemDragCompletedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) {}
}
