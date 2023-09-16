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
    private FrameworkElement _child = null;
    private AdornedControl.AdornerPlacement _horizontalAdornerPlacement = AdornedControl.AdornerPlacement.Inside;
    private AdornedControl.AdornerPlacement _verticalAdornerPlacement = AdornedControl.AdornerPlacement.Inside;
    private double _offsetX = 0.0;
    private double _offsetY = 0.0;

    public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement,
                                   AdornedControl.AdornerPlacement horizontalAdornerPlacement = AdornedControl.AdornerPlacement.Inside, 
                                   AdornedControl.AdornerPlacement verticalAdornerPlacement = AdornedControl.AdornerPlacement.Inside,
                                   double offsetX = 0, double offsetY = 0)
        : base(adornedElement)
    {
        // Comprueba los argumentos
        if (adornedElement is null)
            throw new ArgumentNullException("adornedElement");
        if (adornerChildElement is null)
            throw new ArgumentNullException("adornerChildElement");
        // Asigna las propiedades
        _child = adornerChildElement;
        _horizontalAdornerPlacement = horizontalAdornerPlacement;
        _verticalAdornerPlacement = verticalAdornerPlacement;
        _offsetX = offsetX;
        _offsetY = offsetY;
        // Asigna el manejador para el evento de cambio de tamaño del elemento hijo
        adornedElement.SizeChanged += new SizeChangedEventHandler(adornedElement_SizeChanged);
        // Añade los elementos hijo
        AddLogicalChild(adornerChildElement);
        AddVisualChild(adornerChildElement);
    }

    /// <summary>
    ///     Evento lanzado cuando el tamaño del control adornado se ha modificado
    /// </summary>
    private void adornedElement_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        InvalidateMeasure();
    }

    /// <summary>
    ///     Sobrecarga el método MeasureOverride
    /// </summary>
    protected override Size MeasureOverride(Size constraint)
    {
        _child.Measure(constraint);
        return _child.DesiredSize;
    }

    /// <summary>
    ///     Determina la coordenada X del control hijo
    /// </summary>
    private double DetermineX()
    {
        switch (_child.HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
            {
                if (_horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    double adornerWidth = _child.DesiredSize.Width;
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return (position.X - adornerWidth) + _offsetX;
                }
                else if (_horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Outside)
                {
                    return -_child.DesiredSize.Width + _offsetX;
                }
                else
                {
                    return _offsetX;
                }
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
                    double adornerWidth = _child.DesiredSize.Width;
                    double adornedWidth = AdornedElement.ActualWidth;
                    double x = adornedWidth - adornerWidth;
                    return x + _offsetX;
                }
            }
            case HorizontalAlignment.Center:
            {
                double adornerWidth = _child.DesiredSize.Width;

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
            {
                return 0.0;
            }
        }

        return 0.0;
    }

    /// <summary>
    ///     Determina la coordenada Y del control hijo
    /// </summary>
    private double DetermineY()
    {
        switch (_child.VerticalAlignment)
        {
            case VerticalAlignment.Top:
            {
                if (_verticalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    double adornerWidth = _child.DesiredSize.Width;
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return (position.Y - adornerWidth) + _offsetY;
                }
                else if (_verticalAdornerPlacement == AdornedControl.AdornerPlacement.Outside)
                {
                    return -_child.DesiredSize.Height + _offsetY;
                }
                else
                {
                    return _offsetY;
                }
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
                    double adornerHeight = _child.DesiredSize.Height;
                    double adornedHeight = AdornedElement.ActualHeight;
                    double x = adornedHeight - adornerHeight;
                    return x + _offsetY;
                }
            }
            case VerticalAlignment.Center:
            {
                double adornerHeight = _child.DesiredSize.Height;

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
            {
                return 0.0;
            }
        }

        return 0.0;
    }

    /// <summary>
    ///     Determina el ancho del control hijo
    /// </summary>
    private double DetermineWidth()
    {
        if (!double.IsNaN(PositionX))
        {
            return _child.DesiredSize.Width;
        }

        switch (_child.HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
            {
                return _child.DesiredSize.Width;
            }
            case HorizontalAlignment.Right:
            {
                return _child.DesiredSize.Width;
            }
            case HorizontalAlignment.Center:
            {
                return _child.DesiredSize.Width;
            }
            case HorizontalAlignment.Stretch:
            {
                return AdornedElement.ActualWidth;
            }
        }

        return 0.0;
    }

    /// <summary>
    ///     Determina la altura del control hijo
    /// </summary>
    private double DetermineHeight()
    {
        if (!double.IsNaN(PositionY))
        {
            return _child.DesiredSize.Height;
        }

        switch (_child.VerticalAlignment)
        {
            case VerticalAlignment.Top:
            {
                return _child.DesiredSize.Height;
            }
            case VerticalAlignment.Bottom:
            {
                return _child.DesiredSize.Height;
            }
            case VerticalAlignment.Center:
            {
                return _child.DesiredSize.Height; 
            }
            case VerticalAlignment.Stretch:
            {
                return AdornedElement.ActualHeight;
            }
        }

        return 0.0;
    }

    /// <summary>
    ///     Redimensiona el tamaño
    /// </summary>
    protected override Size ArrangeOverride(Size finalSize)
    {
        double x = PositionX;
        if (double.IsNaN(x))
        {
            x = DetermineX();
        }
        double y = PositionY;
        if (double.IsNaN(y))
        {
            y = DetermineY();
        }
        double adornerWidth = DetermineWidth();
        double adornerHeight = DetermineHeight();
        _child.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
        return finalSize;
    }

    /// <summary>
    ///     Desconecta el elemento hijo del árbol visual para que se reutilice después
    /// </summary>
    public void DisconnectChild()
    {
        RemoveLogicalChild(_child);
        RemoveVisualChild(_child);
    }

    /// <summary>
    ///     Número de elementos hijo
    /// </summary>
    protected override int VisualChildrenCount
    {
        get { return 1; }
    }

    /// <summary>
    ///     Obtiene uno de los controles hijo
    /// </summary>
    protected override Visual GetVisualChild(int index)
    {
        return _child;
    }

    /// <summary>
    ///     Control que se adorna
    /// </summary>
    public FrameworkElement Child
    {
        get
        {
            return _child;
        }
    }

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
    protected override IEnumerator LogicalChildren
    {
        get
        {
            ArrayList list = new ArrayList();
            list.Add(_child);
            return list.GetEnumerator();
        }
    }

    /// <summary>
    ///     Sobrescribe la propiedad AdornedElement de la clase base para no tener que comprobar constantemente el tipo
    /// </summary>
    public new FrameworkElement AdornedElement
    {
        get
        {
            return (FrameworkElement)base.AdornedElement;
        }
    }
}
