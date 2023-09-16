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
    private void sourceConnector_HotspotUpdated(object sender, EventArgs e)
    {
        this.SourceConnectorHotspot = this.SourceConnector.Hotspot;
    }

    /// <summary>
    ///     Evento lanzado cuando se modifica el punto de destino del conector
    /// </summary>
    private void destConnector_HotspotUpdated(object sender, EventArgs e)
    {
        this.DestConnectorHotspot = this.DestConnector.Hotspot;
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

        double deltaX = Math.Abs(this.DestConnectorHotspot.X - this.SourceConnectorHotspot.X);
        double deltaY = Math.Abs(this.DestConnectorHotspot.Y - this.SourceConnectorHotspot.Y);
        if (deltaX > deltaY)
        {
            double midPointX = this.SourceConnectorHotspot.X + ((this.DestConnectorHotspot.X - this.SourceConnectorHotspot.X) / 2);
            computedPoints.Add(new Point(midPointX, this.SourceConnectorHotspot.Y));
            computedPoints.Add(new Point(midPointX, this.DestConnectorHotspot.Y));
        }
        else
        {
            double midPointY = this.SourceConnectorHotspot.Y + ((this.DestConnectorHotspot.Y - this.SourceConnectorHotspot.Y) / 2);
            computedPoints.Add(new Point(this.SourceConnectorHotspot.X, midPointY));
            computedPoints.Add(new Point(this.DestConnectorHotspot.X, midPointY));
        }

        computedPoints.Add(this.DestConnectorHotspot);
        computedPoints.Freeze();

        this.Points = computedPoints;
    }
    /// <summary>
    ///     Conector origen al que se adjunta la conexión
    /// </summary>
    public ConnectorViewModel? SourceConnector
    {
        get
        {
            return _sourceConnector;
        }
        set
        {
            if (_sourceConnector == value)
            {
                return;
            }

            if (_sourceConnector != null)
            {
                _sourceConnector.AttachedConnections.Remove(this);
                _sourceConnector.HotspotUpdated -= new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
            }

            _sourceConnector = value;

            if (_sourceConnector != null)
            {
                _sourceConnector.AttachedConnections.Add(this);
                _sourceConnector.HotspotUpdated += new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                this.SourceConnectorHotspot = _sourceConnector.Hotspot;
            }

            OnPropertyChanged("SourceConnector");
            OnConnectionChanged();
        }
    }

    /// <summary>
    ///     Conector origen al que se adjunta la conexión
    /// </summary>
    public ConnectorViewModel? DestConnector
    {
        get
        {
            return _targetConnector;
        }
        set
        {
            if (_targetConnector == value)
            {
                return;
            }

            if (_targetConnector != null)
            {
                _targetConnector.AttachedConnections.Remove(this);
                _targetConnector.HotspotUpdated -= new EventHandler<EventArgs>(destConnector_HotspotUpdated);
            }

            _targetConnector = value;

            if (_targetConnector != null)
            {
                _targetConnector.AttachedConnections.Add(this);
                _targetConnector.HotspotUpdated += new EventHandler<EventArgs>(destConnector_HotspotUpdated);
                this.DestConnectorHotspot = _targetConnector.Hotspot;
            }

            OnPropertyChanged("DestConnector");
            OnConnectionChanged();
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
