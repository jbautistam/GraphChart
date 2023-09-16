using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Collections;
using System.Windows.Input;

namespace Bau.Controls.GraphChartControl.Adorners;

/// <summary>
///     Clase que define un <see cref="Adorner"/> que permite que una clase derivada de <see cref="FrameworkElement"/>
/// adorne otro <see cref="FrameworkElement"/>
/// </summary>
public class FrameworkElementAdorner : Adorner
{
    private AdornedControl.AdornerPlacement _horizontalAdornerPlacement = AdornedControl.AdornerPlacement.Inside;
    private AdornedControl.AdornerPlacement _verticalAdornerPlacement = AdornedControl.AdornerPlacement.Inside;
    private double _offsetX = 0.0;
    private double _offsetY = 0.0;
    //private double _positionX = double.NaN;
    //private double _positionY = double.NaN;

    public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement,
                                   AdornedControl.AdornerPlacement horizontalAdornerPlacement = AdornedControl.AdornerPlacement.Mouse, 
                                   AdornedControl.AdornerPlacement verticalAdornerPlacement = AdornedControl.AdornerPlacement.Mouse,
                                   double offsetX = 0, double offsetY = 0) : base(adornedElement)
    {
        // Comprueba los argumentos
        if (adornedElement == null)
            throw new ArgumentNullException("adornedElement");
        if (adornerChildElement == null)
            throw new ArgumentNullException("adornerChildElement");
        // Asigna las propiedades
        Child = adornerChildElement;
        _horizontalAdornerPlacement = horizontalAdornerPlacement;
        _verticalAdornerPlacement = verticalAdornerPlacement;
        _offsetX = offsetX;
        _offsetY = offsetY;
        // Añade el manejador del evento de cambio de tamaño
        adornedElement.SizeChanged += new SizeChangedEventHandler(adornedElement_SizeChanged);
        // Añade el elemento hijo al árbol visual
        AddLogicalChild(adornerChildElement);
        AddVisualChild(adornerChildElement);
    }

    /// <summary>
    ///     Evento lanzado cuando el tamaño del control adornado ha cambiado
    /// </summary>
    private void adornedElement_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        InvalidateMeasure();
    }

    /// <summary>
    ///     Control hijo
    /// </summary>
    public FrameworkElement Child { get; }

    /// <summary>
    ///     Posición X del elemento hijo (cuando no está asignada a NaN)
    /// </summary>
    public double PositionX { get; set; } = double.NaN;

    /// <summary>
    ///     Posición Y del elemento hijo (cuando no está asignada a NaN)
    /// </summary>
    public double PositionY { get; set; } = double.NaN;

    protected override Size MeasureOverride(Size constraint)
    {
        this.Child.Measure(constraint);
        return this.Child.DesiredSize;
    }

    /// <summary>
    /// Determine the X coordinate of the child.
    /// </summary>
    private double DetermineX()
    {
        switch (Child.HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
            {
                if (_horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    double adornerWidth = this.Child.DesiredSize.Width;
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return (position.X - adornerWidth) + _offsetX;
                }
                else if (_horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Outside)
                {
                    return -Child.DesiredSize.Width + _offsetX;
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
                    double adornerWidth = this.Child.DesiredSize.Width;
                    double adornedWidth = AdornedElement.ActualWidth;
                    double x = adornedWidth - adornerWidth;
                    return x + _offsetX;
                }
            }
            case HorizontalAlignment.Center:
            {
                double adornerWidth = this.Child.DesiredSize.Width;

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
    /// Determine the Y coordinate of the child.
    /// </summary>
    private double DetermineY()
    {
        switch (Child.VerticalAlignment)
        {
            case VerticalAlignment.Top:
            {
                if (_verticalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    double adornerWidth = this.Child.DesiredSize.Width;
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return (position.Y - adornerWidth) + _offsetY;
                }
                else if (_verticalAdornerPlacement == AdornedControl.AdornerPlacement.Outside)
                {
                    return -Child.DesiredSize.Height + _offsetY;
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
                    double adornerHeight = this.Child.DesiredSize.Height;
                    double adornedHeight = AdornedElement.ActualHeight;
                    double x = adornedHeight - adornerHeight;
                    return x + _offsetY;
                }
            }
            case VerticalAlignment.Center:
            {
                double adornerHeight = this.Child.DesiredSize.Height;

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
    /// Determine the width of the child.
    /// </summary>
    private double DetermineWidth()
    {
        if (!double.IsNaN(PositionX))
        {
            return this.Child.DesiredSize.Width;
        }

        switch (Child.HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
            {
                return this.Child.DesiredSize.Width;
            }
            case HorizontalAlignment.Right:
            {
                return this.Child.DesiredSize.Width;
            }
            case HorizontalAlignment.Center:
            {
                return this.Child.DesiredSize.Width;
            }
            case HorizontalAlignment.Stretch:
            {
                return AdornedElement.ActualWidth;
            }
        }

        return 0.0;
    }

    /// <summary>
    /// Determine the height of the child.
    /// </summary>
    private double DetermineHeight()
    {
        if (!double.IsNaN(PositionY))
        {
            return this.Child.DesiredSize.Height;
        }

        switch (Child.VerticalAlignment)
        {
            case VerticalAlignment.Top:
            {
                return this.Child.DesiredSize.Height;
            }
            case VerticalAlignment.Bottom:
            {
                return this.Child.DesiredSize.Height;
            }
            case VerticalAlignment.Center:
            {
                return this.Child.DesiredSize.Height; 
            }
            case VerticalAlignment.Stretch:
            {
                return AdornedElement.ActualHeight;
            }
        }

        return 0.0;
    }

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
        this.Child.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
        return finalSize;
    }

    protected override int VisualChildrenCount
    {
        get { return 1; }
    }

    protected override Visual GetVisualChild(int index)
    {
        return this.Child;
    }

    protected override IEnumerator LogicalChildren
    {
        get
        {
            ArrayList list = new ArrayList();
            list.Add(this.Child);
            return (IEnumerator)list.GetEnumerator();
        }
    }

    /// <summary>
    /// Disconnect the child element from the visual tree so that it may be reused later.
    /// </summary>
    public void DisconnectChild()
    {
        base.RemoveLogicalChild(Child);
        base.RemoveVisualChild(Child);
    }

    /// <summary>
    /// Override AdornedElement from base class for less type-checking.
    /// </summary>
    public new FrameworkElement AdornedElement
    {
        get
        {
            return (FrameworkElement)base.AdornedElement;
        }
    }
}
