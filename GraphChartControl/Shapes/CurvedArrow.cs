using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace Bau.Controls.GraphChartControl.Shapes;

/// <summary>
///     Define una flecha que pasa por varios puntos
/// </summary>
public class CurvedArrow : Shape
{
    // Propiedades de dependencia
    public static readonly DependencyProperty ArrowHeadLengthProperty = DependencyProperty.Register(nameof(ArrowHeadLength), typeof(double), typeof(CurvedArrow),
                                                                                                    new FrameworkPropertyMetadata(20.0, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty ArrowHeadWidthProperty = DependencyProperty.Register(nameof(ArrowHeadWidth), typeof(double), typeof(CurvedArrow),
                                                                                                   new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(CurvedArrow),
                                                                                           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Generate the geometry for the three optional arrow symbols at the start, middle and end of the arrow.
    /// </summary>
    private void GenerateArrowHeadGeometry(GeometryGroup geometryGroup)
    {
        Point startPoint = Points[0];
        Point penultimatePoint = Points[Points.Count - 2];
        Point arrowHeadTip = Points[Points.Count - 1];
        Vector startDir = arrowHeadTip - penultimatePoint;
        startDir.Normalize();
        Point basePoint = arrowHeadTip - (startDir * ArrowHeadLength);
        Vector crossDir = new Vector(-startDir.Y, startDir.X);
        Point[] arrowHeadPoints = new Point[3];
        arrowHeadPoints[0] = arrowHeadTip;
        arrowHeadPoints[1] = basePoint - (crossDir * (ArrowHeadWidth / 2));
        arrowHeadPoints[2] = basePoint + (crossDir * (ArrowHeadWidth / 2));

        //
        // Build geometry for the arrow head.
        //
        PathFigure arrowHeadFig = new PathFigure();
        arrowHeadFig.IsClosed = true;
        arrowHeadFig.IsFilled = true;
        arrowHeadFig.StartPoint = arrowHeadPoints[0];
        arrowHeadFig.Segments.Add(new LineSegment(arrowHeadPoints[1], true));
        arrowHeadFig.Segments.Add(new LineSegment(arrowHeadPoints[2], true));

        PathGeometry pathGeometry = new PathGeometry();
        pathGeometry.Figures.Add(arrowHeadFig);

        geometryGroup.Children.Add(pathGeometry);
    }

    /// <summary>
    ///     Genera la geometría de la figura
    /// </summary>
    protected Geometry GenerateGeometry()
    {
        PathGeometry pathGeometry = new();

            if (Points.Count == 2 || Points.Count == 3)
            {
                // Make a straight line.
                PathFigure figure = new();

                    // Crea una línea recta
                    figure.IsClosed = false;
                    figure.IsFilled = false;
                    figure.StartPoint = Points[0];

                    for (int i = 1; i < Points.Count; ++i)
                        figure.Segments.Add(new LineSegment(Points[i], true));

                    pathGeometry.Figures.Add(figure);
            }
            else
            {
                PointCollection adjustedPoints = new PointCollection();
                adjustedPoints.Add(Points[0]);
                for (int i = 1; i < Points.Count; ++i)
                {
                    adjustedPoints.Add(Points[i]);
                }

                if (adjustedPoints.Count == 4)
                {
                    PathFigure fig = new PathFigure();

                    // Crea una línea curva
                    fig.IsClosed = false;
                    fig.IsFilled = false;
                    fig.StartPoint = adjustedPoints[0];
                    fig.Segments.Add(new BezierSegment(adjustedPoints[1], adjustedPoints[2], adjustedPoints[3], true));

                    pathGeometry.Figures.Add(fig);
                }
                else if (adjustedPoints.Count >= 5)
                {
                    PathFigure fig = new PathFigure();

                    // Crea una línea curva
                    fig.IsClosed = false;
                    fig.IsFilled = false;
                    fig.StartPoint = adjustedPoints[0];

                    adjustedPoints.RemoveAt(0);

                    while (adjustedPoints.Count > 3)
                    {
                        Point generatedPoint = adjustedPoints[1] + ((adjustedPoints[2] - adjustedPoints[1]) / 2);

                        fig.Segments.Add(new BezierSegment(adjustedPoints[0], adjustedPoints[1], generatedPoint, true));

                        adjustedPoints.RemoveAt(0);
                        adjustedPoints.RemoveAt(0);
                    }

                    if (adjustedPoints.Count == 2)
                        fig.Segments.Add(new BezierSegment(adjustedPoints[0], adjustedPoints[0], adjustedPoints[1], true));
                    else
                        fig.Segments.Add(new BezierSegment(adjustedPoints[0], adjustedPoints[1], adjustedPoints[2], true));

                    pathGeometry.Figures.Add(fig);
                }
            }
            return pathGeometry;
    }

    /// <summary>
    ///     Angulo en grados de la punta de la flecha
    /// </summary>
    public double ArrowHeadLength
    {
        get { return (double) GetValue(ArrowHeadLengthProperty); }
        set { SetValue(ArrowHeadLengthProperty, value); }
    }

    /// <summary>
    ///     Ancho de la punta de la flecha
    /// </summary>
    public double ArrowHeadWidth
    {
        get { return (double) GetValue(ArrowHeadWidthProperty); }
        set { SetValue(ArrowHeadWidthProperty, value); }
    }

    /// <summary>
    ///     Puntos intermedios que conforman la línea entre el inicio y el final
    /// </summary>
    public PointCollection Points
    {
        get { return (PointCollection) GetValue(PointsProperty); }
        set { SetValue(PointsProperty, value); }
    }

    /// <summary>
    ///     Obtiene la geometría de la figura
    /// </summary>
    protected override Geometry DefiningGeometry
    {
        get
        {
            if (Points is null || Points.Count < 2)
                return new GeometryGroup();
            else
            {
                GeometryGroup group = new();
                Geometry geometry = GenerateGeometry();

                    // Añade la geometría al group
                    group.Children.Add(geometry);
                    // Añade la punta de la flcha a la geometría
                    GenerateArrowHeadGeometry(group);
                    // Devuelve la geometría cacheada
                    return group;
            }
        }
    }
}
