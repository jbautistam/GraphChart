using System.Windows;

namespace Bau.Controls.GraphChartControl.Adorners;

public delegate void AdornerEventHandler(object sender, AdornerEventArgs e);

/// <summary>
///     Argumentos del evento del adorno
/// </summary>
public class AdornerEventArgs : RoutedEventArgs
{
    public AdornerEventArgs(RoutedEvent routedEvent, object source, FrameworkElement adorner) : base(routedEvent, source)
    {
        Adorner = adorner;
    }

    /// <summary>
    ///     Adorno
    /// </summary>
    public FrameworkElement Adorner { get; }
}
