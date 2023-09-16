using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Collections;
using System.Windows.Input;

namespace Bau.Controls.GraphChartControl.Adorners;

/// <summary>
///     Esta clase es un <see cref="Adorner"/> que permite que una clase derivada de <see cref="FrameworkElement"/>
/// adorne otro <see cref="FrameworkElement"/>
/// </summary>
/// <remarks>
///     Basado en este código http://www.codeproject.com/KB/WPF/WPFJoshSmith.aspx
/// </remarks>
public class FrameworkElementAdorner : Adorner
{
    // Variables privadas
    private AdornedControl.AdornerPlacement _horizontalAdornerPlacement = AdornedControl.AdornerPlacement.Inside;
    private AdornedControl.AdornerPlacement _verticalAdornerPlacement = AdornedControl.AdornerPlacement.Inside;
    private double _offsetX = 0.0;
    private double _offsetY = 0.0;

    public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement,
                                   AdornedControl.AdornerPlacement horizontalAdornerPlacement = AdornedControl.AdornerPlacement.Inside, 
                                   AdornedControl.AdornerPlacement verticalAdornerPlacement = AdornedControl.AdornerPlacement.Inside,
                                   double offsetX = 0, double offsetY = 0) : base(adornedElement)
    {
        // Comprueba los argumentos
        if (adornedElement is null)
            throw new ArgumentNullException(nameof(adornedElement));
        if (adornerChildElement is null)
            throw new ArgumentNullException(nameof(adornerChildElement));
        // Asigna las propiedades
        Child = adornerChildElement;
        _horizontalAdornerPlacement = horizontalAdornerPlacement;
        _verticalAdornerPlacement = verticalAdornerPlacement;
        _offsetX = offsetX;
        _offsetY = offsetY;
        // Asigna el manejador para el evento de cambio de tamaño del elemento hijo
        adornedElement.SizeChanged += (sender, args) => InvalidateMeasure();
        // adornedElement.SizeChanged += new SizeChangedEventHandler(adornedElement_SizeChanged);
        // Añade los elementos hijo
        AddLogicalChild(adornerChildElement);
        AddVisualChild(adornerChildElement);
    }

    /// <summary>
    ///     Sobrecarga el método MeasureOverride
    /// </summary>
    protected override Size MeasureOverride(Size constraint)
    {
        // Mide los elementos hijo
        Child.Measure(constraint);
        // Devuelve el tamaño deseado
        return Child.DesiredSize;
    }

    /// <summary>
    ///     Determina la coordenada X del control hijo
    /// </summary>
    private double DetermineX()
    {
        switch (Child.HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
            {
                if (_horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    double adornerWidth = Child.DesiredSize.Width;
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return (position.X - adornerWidth) + _offsetX;
                }
                else if (_horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Outside)
                    return -Child.DesiredSize.Width + _offsetX;
                else
                    return _offsetX;
            }
            case HorizontalAlignment.Right:
            {
                if (_horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return position.X + _offsetX;
                }
                else if (_horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Outside)
                {
                    double adornedWidth = AdornedElement.ActualWidth;
                    return adornedWidth + _offsetX;
                }
                else
                {
                    double adornerWidth = Child.DesiredSize.Width;
                    double adornedWidth = AdornedElement.ActualWidth;
                    double x = adornedWidth - adornerWidth;
                    return x + _offsetX;
                }
            }
            case HorizontalAlignment.Center:
            {
                double adornerWidth = Child.DesiredSize.Width;

                if (_horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return (position.X - (adornerWidth / 2)) + _offsetX;
                }
                else
                {
                    double adornedWidth = AdornedElement.ActualWidth;
                    double x = (adornedWidth / 2) - (adornerWidth / 2);
                    return x + _offsetX;
                }
            }
            case HorizontalAlignment.Stretch:
                return 0.0;
            default:
                return 0.0;
        }
    }

    /// <summary>
    ///     Determina la coordenada Y del control hijo
    /// </summary>
    private double DetermineY()
    {
        switch (Child.VerticalAlignment)
        {
            case VerticalAlignment.Top:
            {
                if (_verticalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    double adornerWidth = Child.DesiredSize.Width;
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return (position.Y - adornerWidth) + _offsetY;
                }
                else if (_verticalAdornerPlacement == AdornedControl.AdornerPlacement.Outside)
                    return -Child.DesiredSize.Height + _offsetY;
                else
                    return _offsetY;
            }
            case VerticalAlignment.Bottom:
            {
                if (_verticalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return position.Y + _offsetY;
                }
                else if (_verticalAdornerPlacement == AdornedControl.AdornerPlacement.Outside)
                {
                    double adornedHeight = AdornedElement.ActualHeight;
                    return adornedHeight + _offsetY;
                }
                else
                {
                    double adornerHeight = Child.DesiredSize.Height;
                    double adornedHeight = AdornedElement.ActualHeight;
                    double x = adornedHeight - adornerHeight;
                    return x + _offsetY;
                }
            }
            case VerticalAlignment.Center:
            {
                double adornerHeight = Child.DesiredSize.Height;

                if (_verticalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return (position.Y - (adornerHeight/2)) + _offsetY;
                }
                else
                {
                    double adornedHeight = AdornedElement.ActualHeight;
                    double y = (adornedHeight / 2) - (adornerHeight / 2);
                    return y + _offsetY;
                }
            }
            case VerticalAlignment.Stretch:
                return 0.0;
            default:
                return 0.0;
        }
    }

    /// <summary>
    ///     Determina el ancho del control hijo
    /// </summary>
    private double DetermineWidth()
    {
        if (!double.IsNaN(PositionX))
            return Child.DesiredSize.Width;
        else
            switch (Child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    return Child.DesiredSize.Width;
                case HorizontalAlignment.Right:
                    return Child.DesiredSize.Width;
                case HorizontalAlignment.Center:
                    return Child.DesiredSize.Width;
                case HorizontalAlignment.Stretch:
                    return AdornedElement.ActualWidth;
                default:
                    return 0.0;
            }
    }

    /// <summary>
    ///     Determina la altura del control hijo
    /// </summary>
    private double DetermineHeight()
    {
        if (!double.IsNaN(PositionY))
            return Child.DesiredSize.Height;
        else
            switch (Child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    return Child.DesiredSize.Height;
                case VerticalAlignment.Bottom:
                    return Child.DesiredSize.Height;
                case VerticalAlignment.Center:
                    return Child.DesiredSize.Height; 
                case VerticalAlignment.Stretch:
                    return AdornedElement.ActualHeight;
                default:
                    return 0.0;
            }
    }

    /// <summary>
    ///     Redimensiona el tamaño
    /// </summary>
    protected override Size ArrangeOverride(Size finalSize)
    {
        double x = PositionX, y = PositionY;

            // Determina las posiciones X e Y
            if (double.IsNaN(x))
                x = DetermineX();
            if (double.IsNaN(y))
                y = DetermineY();
            // Calcula el tamaño
            Child.Arrange(new Rect(x, y, DetermineWidth(), DetermineHeight()));
            // Devuelve el tamaño final
            return finalSize;
    }

    /// <summary>
    ///     Desconecta el elemento hijo del árbol visual para que se reutilice después
    /// </summary>
    public void DisconnectChild()
    {
        RemoveLogicalChild(Child);
        RemoveVisualChild(Child);
    }

    /// <summary>
    ///     Número de elementos hijo
    /// </summary>
    protected override int VisualChildrenCount => 1;

    /// <summary>
    ///     Obtiene uno de los controles hijo
    /// </summary>
    protected override Visual GetVisualChild(int index) => Child;

    /// <summary>
    ///     Control que se adorna
    /// </summary>
    public FrameworkElement Child { get; }

    /// <summary>
    ///     Posición X del control hijo (se inicializa a NaN)
    /// </summary>
    public double PositionX { get; set; } = double.NaN;

    /// <summary>
    ///     Posición X del control hijo (se inicializa a NaN)
    /// </summary>
    public double PositionY { get; set; } = double.NaN;

    /// <summary>
    ///     Enumera los controles hijo
    /// </summary>
    protected override IEnumerator LogicalChildren => new ArrayList { Child }.GetEnumerator();

    /// <summary>
    ///     Sobrescribe la propiedad AdornedElement de la clase base para no tener que comprobar constantemente el tipo
    /// </summary>
    public new FrameworkElement AdornedElement => (FrameworkElement) base.AdornedElement;
}
