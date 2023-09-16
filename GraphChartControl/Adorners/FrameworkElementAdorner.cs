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
    private double _positionX = double.NaN;
    private double _positionY = double.NaN;

    public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement,
                                   AdornedControl.AdornerPlacement horizontalAdornerPlacement = AdornedControl.AdornerPlacement.Inside, 
                                   AdornedControl.AdornerPlacement verticalAdornerPlacement = AdornedControl.AdornerPlacement.Inside,
                                   double offsetX = 0, double offsetY = 0)
        : base(adornedElement)
    {
        if (adornedElement == null)
        {
            throw new ArgumentNullException("adornedElement");
        }

        if (adornerChildElement == null)
        {
            throw new ArgumentNullException("adornerChildElement");
        }

        this._child = adornerChildElement;
        this._horizontalAdornerPlacement = horizontalAdornerPlacement;
        this._verticalAdornerPlacement = verticalAdornerPlacement;
        this._offsetX = offsetX;
        this._offsetY = offsetY;

        adornedElement.SizeChanged += new SizeChangedEventHandler(adornedElement_SizeChanged);

        base.AddLogicalChild(adornerChildElement);
        base.AddVisualChild(adornerChildElement);
    }

    /// <summary>
    ///     Evento lanzado cuando el tamaño del control adornado se ha modificado
    /// </summary>
    private void adornedElement_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        InvalidateMeasure();
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
    public double PositionX
    {
        get
        {
            return _positionX;
        }
        set
        {
            _positionX = value;
        }
    }

    /// <summary>
    ///     Posición X del control hijo (se inicializa a NaN)
    /// </summary>
    public double PositionY
    {
        get
        {
            return _positionY;
        }
        set
        {
            _positionY = value;
        }
    }

    /// <summary>
    ///     Sobrecarga el método MeasureOverride
    /// </summary>
    protected override Size MeasureOverride(Size constraint)
    {
        this._child.Measure(constraint);
        return this._child.DesiredSize;
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
                    double adornerWidth = this._child.DesiredSize.Width;
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
                    double adornerWidth = this._child.DesiredSize.Width;
                    double adornedWidth = AdornedElement.ActualWidth;
                    double x = adornedWidth - adornerWidth;
                    return x + _offsetX;
                }
            }
            case HorizontalAlignment.Center:
            {
                double adornerWidth = this._child.DesiredSize.Width;

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
                    double adornerWidth = this._child.DesiredSize.Width;
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
                    double adornerHeight = this._child.DesiredSize.Height;
                    double adornedHeight = AdornedElement.ActualHeight;
                    double x = adornedHeight - adornerHeight;
                    return x + _offsetY;
                }
            }
            case VerticalAlignment.Center:
            {
                double adornerHeight = this._child.DesiredSize.Height;

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
            return this._child.DesiredSize.Width;
        }

        switch (_child.HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
            {
                return this._child.DesiredSize.Width;
            }
            case HorizontalAlignment.Right:
            {
                return this._child.DesiredSize.Width;
            }
            case HorizontalAlignment.Center:
            {
                return this._child.DesiredSize.Width;
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
            return this._child.DesiredSize.Height;
        }

        switch (_child.VerticalAlignment)
        {
            case VerticalAlignment.Top:
            {
                return this._child.DesiredSize.Height;
            }
            case VerticalAlignment.Bottom:
            {
                return this._child.DesiredSize.Height;
            }
            case VerticalAlignment.Center:
            {
                return this._child.DesiredSize.Height; 
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
        this._child.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
        return finalSize;
    }

    /// <summary>
    ///     Número de elementos hijo
    /// </summary>
    protected override Int32 VisualChildrenCount
    {
        get { return 1; }
    }

    /// <summary>
    ///     Obtiene uno de los controles hijo
    /// </summary>
    protected override Visual GetVisualChild(Int32 index)
    {
        return this._child;
    }

    /// <summary>
    ///     Enumera los controles hijo
    /// </summary>
    protected override IEnumerator LogicalChildren
    {
        get
        {
            ArrayList list = new ArrayList();
            list.Add(this._child);
            return (IEnumerator)list.GetEnumerator();
        }
    }

    /// <summary>
    ///     Desconecta el elemento hijo del árbol visual para que se reutilice después
    /// </summary>
    public void DisconnectChild()
    {
        base.RemoveLogicalChild(_child);
        base.RemoveVisualChild(_child);
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
