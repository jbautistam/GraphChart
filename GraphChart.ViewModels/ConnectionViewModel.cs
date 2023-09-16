using System.Windows.Media;
using System.Windows;

using Bau.Libraries.GraphChart.ViewModels.Base;

namespace Bau.Libraries.GraphChart.ViewModels;

/// <summary>
/// Defines a connection between two connectors (aka connection points) of two nodes.
/// </summary>
public sealed class ConnectionViewModel : BaseObservableObject
{
    // Eventos públicos
    public event EventHandler<EventArgs>? ConnectionChanged;
    // Variables privadas
    private ConnectorViewModel? _sourceConnector = null;
    private ConnectorViewModel? _targetConnector = null;
    private Point _sourceConnectorHotspot;
    private Point _targetConnectorHotspot;
    private PointCollection _points = new();

    /// <summary>
    ///     Lanza el evento <see cref="ConnectionChanged"/>
    /// </summary>
    private void OnConnectionChanged()
    {
		ConnectionChanged?.Invoke(this, EventArgs.Empty);
	}

    /// <summary>
    ///     Evento lanzado cuando se modifica el punto de origen del conector
    /// </summary>
    private void sourceConnector_HotspotUpdated(object? sender, EventArgs e)
    {
        SourceConnectorHotspot = SourceConnector?.Hotspot ?? new Point();
    }

    /// <summary>
    ///     Evento lanzado cuando se modifica el punto de destino del conector
    /// </summary>
    private void destConnector_HotspotUpdated(object? sender, EventArgs e)
    {
        DestConnectorHotspot = DestConnector?.Hotspot ?? new Point();
    }

    /// <summary>
    ///     Recalcula los puntos de conexión
    /// </summary>
    private void ComputeConnectionPoints()
    {
        PointCollection computedPoints = new()
                                            {
                                                SourceConnectorHotspot
                                            };
        double deltaX = Math.Abs(DestConnectorHotspot.X - SourceConnectorHotspot.X);
        double deltaY = Math.Abs(DestConnectorHotspot.Y - SourceConnectorHotspot.Y);

            // Añade los puntos medios calculados
            if (deltaX > deltaY)
            {
                double midPointX = SourceConnectorHotspot.X + ((DestConnectorHotspot.X - SourceConnectorHotspot.X) / 2);

                    computedPoints.Add(new Point(midPointX, SourceConnectorHotspot.Y));
                    computedPoints.Add(new Point(midPointX, DestConnectorHotspot.Y));
            }
            else
            {
                double midPointY = SourceConnectorHotspot.Y + ((DestConnectorHotspot.Y - SourceConnectorHotspot.Y) / 2);

                    computedPoints.Add(new Point(SourceConnectorHotspot.X, midPointY));
                    computedPoints.Add(new Point(DestConnectorHotspot.X, midPointY));
            }
            // Añade los puntos de destino
            computedPoints.Add(DestConnectorHotspot);
            computedPoints.Freeze();
            // Devuelve los puntos calculados
            Points = computedPoints;
    }

    /// <summary>
    ///     Conector origen al que se adjunta la conexión
    /// </summary>
    public ConnectorViewModel? SourceConnector
    {
        get { return _sourceConnector; }
        set
        {
            if (_sourceConnector != value)
            {
                // Elimina los puntos de origen y los eventos
                if (_sourceConnector is not null)
                {
                    _sourceConnector.AttachedConnections.Remove(this);
                    _sourceConnector.HotspotUpdated -= new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                }
                // Asigna el punto de origen
                _sourceConnector = value;
                // Añade los puntos de origen y los eventos
                if (_sourceConnector is not null)
                {
                    _sourceConnector.AttachedConnections.Add(this);
                    _sourceConnector.HotspotUpdated += new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                    SourceConnectorHotspot = _sourceConnector.Hotspot;
                }
                // Lanza los eventos de modificación
                OnPropertyChanged(nameof(SourceConnector));
                OnConnectionChanged();
            }
        }
    }

    /// <summary>
    ///     Conector origen al que se adjunta la conexión
    /// </summary>
    public ConnectorViewModel? DestConnector
    {
        get { return _targetConnector; }
        set
        {
            if (_targetConnector != value)
            {
                // Elimina los puntos de destino y los eventos
                if (_targetConnector is not null)
                {
                    _targetConnector.AttachedConnections.Remove(this);
                    _targetConnector.HotspotUpdated -= new EventHandler<EventArgs>(destConnector_HotspotUpdated);
                }
                // Asigna el punto de destino
                _targetConnector = value;
                // Añade los puntos de destino y los eventos
                if (_targetConnector is not null)
                {
                    _targetConnector.AttachedConnections.Add(this);
                    _targetConnector.HotspotUpdated += new EventHandler<EventArgs>(destConnector_HotspotUpdated);
                    DestConnectorHotspot = _targetConnector.Hotspot;
                }
                // Lanza los eventos de modificación
                OnPropertyChanged(nameof(DestConnector));
                OnConnectionChanged();
            }
        }
    }

    /// <summary>
    ///     Punto origen utilizado para los puntos de la conexión
    /// </summary>
    public Point SourceConnectorHotspot
    {
        get { return _sourceConnectorHotspot; }
        set
        {
            if (CheckObject(ref _sourceConnectorHotspot, value))
                ComputeConnectionPoints();
        }
    }

    /// <summary>
    ///     Punto destino utilizado para los puntos de la conexión
    /// </summary>
    public Point DestConnectorHotspot
    {
        get { return _targetConnectorHotspot; }
        set
        {
            if (CheckObject(ref _targetConnectorHotspot, value))
                ComputeConnectionPoints();
        }
    }

    /// <summary>
    ///     Puntos que componen la conexión
    /// </summary>
    public PointCollection Points
    {
        get { return _points; }
        set { CheckObject(ref _points, value); }
    }
}
