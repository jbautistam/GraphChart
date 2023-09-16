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
    private FrameworkElement child = null;
    private AdornedControl.AdornerPlacement horizontalAdornerPlacement = AdornedControl.AdornerPlacement.Inside;
    private AdornedControl.AdornerPlacement verticalAdornerPlacement = AdornedControl.AdornerPlacement.Inside;
    private double offsetX = 0.0;
    private double offsetY = 0.0;
    private double positionX = Double.NaN;
    private double positionY = Double.NaN;

    public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement)
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

        this.child = adornerChildElement;

        base.AddLogicalChild(adornerChildElement);
        base.AddVisualChild(adornerChildElement);
    }

    public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement,
                                   AdornedControl.AdornerPlacement horizontalAdornerPlacement, AdornedControl.AdornerPlacement verticalAdornerPlacement,
                                   double offsetX, double offsetY)
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

        this.child = adornerChildElement;
        this.horizontalAdornerPlacement = horizontalAdornerPlacement;
        this.verticalAdornerPlacement = verticalAdornerPlacement;
        this.offsetX = offsetX;
        this.offsetY = offsetY;

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
            return child;
        }
    }

    /// <summary>
    ///     Posición X del control hijo (se inicializa a NaN)
    /// </summary>
    public double PositionX
    {
        get
        {
            return positionX;
        }
        set
        {
            positionX = value;
        }
    }

    /// <summary>
    ///     Posición X del control hijo (se inicializa a NaN)
    /// </summary>
    public double PositionY
    {
        get
        {
            return positionY;
        }
        set
        {
            positionY = value;
        }
    }

    /// <summary>
    ///     Sobrecarga el método MeasureOverride
    /// </summary>
    protected override Size MeasureOverride(Size constraint)
    {
        this.child.Measure(constraint);
        return this.child.DesiredSize;
    }

    /// <summary>
    ///     Determina la coordenada X del control hijo
    /// </summary>
    private double DetermineX()
    {
        switch (child.HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
            {
                if (horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    double adornerWidth = this.child.DesiredSize.Width;
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return (position.X - adornerWidth) + offsetX;
                }
                else if (horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Outside)
                {
                    return -child.DesiredSize.Width + offsetX;
                }
                else
                {
                    return offsetX;
                }
            }
            case HorizontalAlignment.Right:
            {
                if (horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return position.X + offsetX;
                }
                else if (horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Outside)
                {
                    double adornedWidth = AdornedElement.ActualWidth;
                    return adornedWidth + offsetX;
                }
                else
                {
                    double adornerWidth = this.child.DesiredSize.Width;
                    double adornedWidth = AdornedElement.ActualWidth;
                    double x = adornedWidth - adornerWidth;
                    return x + offsetX;
                }
            }
            case HorizontalAlignment.Center:
            {
                double adornerWidth = this.child.DesiredSize.Width;

                if (horizontalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return (position.X - (adornerWidth / 2)) + offsetX;
                }
                else
                {
                    double adornedWidth = AdornedElement.ActualWidth;
                    double x = (adornedWidth / 2) - (adornerWidth / 2);
                    return x + offsetX;
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
        switch (child.VerticalAlignment)
        {
            case VerticalAlignment.Top:
            {
                if (verticalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    double adornerWidth = this.child.DesiredSize.Width;
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return (position.Y - adornerWidth) + offsetY;
                }
                else if (verticalAdornerPlacement == AdornedControl.AdornerPlacement.Outside)
                {
                    return -child.DesiredSize.Height + offsetY;
                }
                else
                {
                    return offsetY;
                }
            }
            case VerticalAlignment.Bottom:
            {
                if (verticalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return position.Y + offsetY;
                }
                else if (verticalAdornerPlacement == AdornedControl.AdornerPlacement.Outside)
                {
                    double adornedHeight = AdornedElement.ActualHeight;
                    return adornedHeight + offsetY;
                }
                else
                {
                    double adornerHeight = this.child.DesiredSize.Height;
                    double adornedHeight = AdornedElement.ActualHeight;
                    double x = adornedHeight - adornerHeight;
                    return x + offsetY;
                }
            }
            case VerticalAlignment.Center:
            {
                double adornerHeight = this.child.DesiredSize.Height;

                if (verticalAdornerPlacement == AdornedControl.AdornerPlacement.Mouse)
                {
                    Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                    return (position.Y - (adornerHeight/2)) + offsetY;
                }
                else
                {
                    double adornedHeight = AdornedElement.ActualHeight;
                    double y = (adornedHeight / 2) - (adornerHeight / 2);
                    return y + offsetY;
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
        if (!Double.IsNaN(PositionX))
        {
            return this.child.DesiredSize.Width;
        }

        switch (child.HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
            {
                return this.child.DesiredSize.Width;
            }
            case HorizontalAlignment.Right:
            {
                return this.child.DesiredSize.Width;
            }
            case HorizontalAlignment.Center:
            {
                return this.child.DesiredSize.Width;
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
        if (!Double.IsNaN(PositionY))
        {
            return this.child.DesiredSize.Height;
        }

        switch (child.VerticalAlignment)
        {
            case VerticalAlignment.Top:
            {
                return this.child.DesiredSize.Height;
            }
            case VerticalAlignment.Bottom:
            {
                return this.child.DesiredSize.Height;
            }
            case VerticalAlignment.Center:
            {
                return this.child.DesiredSize.Height; 
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
        if (Double.IsNaN(x))
        {
            x = DetermineX();
        }
        double y = PositionY;
        if (Double.IsNaN(y))
        {
            y = DetermineY();
        }
        double adornerWidth = DetermineWidth();
        double adornerHeight = DetermineHeight();
        this.child.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
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
        return this.child;
    }

    /// <summary>
    ///     Enumera los controles hijo
    /// </summary>
    protected override IEnumerator LogicalChildren
    {
        get
        {
            ArrayList list = new ArrayList();
            list.Add(this.child);
            return (IEnumerator)list.GetEnumerator();
        }
    }

    /// <summary>
    ///     Desconecta el elemento hijo del árbol visual para que se reutilice después
    /// </summary>
    public void DisconnectChild()
    {
        base.RemoveLogicalChild(child);
        base.RemoveVisualChild(child);
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
