using System.Windows;

using Bau.Libraries.GraphChart.ViewModels.Base;

namespace Bau.Libraries.GraphChart.ViewModels;

/// <summary>
/// Defines a connector (aka connection point) that can be attached to a node and is used to connect the node to another node.
/// </summary>
public sealed class ConnectorViewModel : BaseObservableObject
{
    /// <summary>
    ///     Tipo del conector
    /// </summary>
    public enum ConnectorType
    {
        /// <summary>Indefinido</summary>
        Undefined,
        /// <summary>Conector de entrada</summary>
        Input,
        /// <summary>Conector de salida</summary>
        Output
    }
    // Eventos públicos
    public event EventHandler<EventArgs>? HotspotUpdated;

    // Variables privadas
    private ImpObservableCollection<ConnectionViewModel> _attachedConnections = new();
    private Point hotspot; // Punto o centro del conector (asociado a ConnectorItem)

    public ConnectorViewModel(string name)
    {
        Name = name;
        Type = ConnectorType.Undefined;
    }

    /// <summary>
    ///     Nombre del conector
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
    ///     Tipo del conector
    /// </summary>
    public ConnectorType Type
    {
        get;
        internal set;
    }

    /// <summary>
    ///     Comprueba si el conector está conectado a otro nodo
    /// </summary>
    public bool IsConnected
    {
        get
        {
            // Comprueba si hay una conexión origen y destino
            foreach (ConnectionViewModel connection in AttachedConnections)
                if (connection.SourceConnector != null && connection.DestConnector != null)
                    return true;
            // Si ha llegado hasta aquí es porque no ha encontrado nada
            return false;
        }
    }

    /// <summary>
    ///     Comrpueba si tiene alguna conexión origen o destino
    /// </summary>
    public bool IsConnectionAttached => AttachedConnections.Count > 0;

    /// <summary>
    ///     Conexiones asociadas al conector
    /// </summary>
    public ImpObservableCollection<ConnectionViewModel> AttachedConnections
    {
        get
        {
            if (_attachedConnections is null)
            {
                _attachedConnections = new ImpObservableCollection<ConnectionViewModel>();
                _attachedConnections.ItemsAdded += new EventHandler<CollectionItemsChangedEventArgs>(attachedConnections_ItemsAdded);
                _attachedConnections.ItemsRemoved += new EventHandler<CollectionItemsChangedEventArgs>(attachedConnections_ItemsRemoved);
            }

            return _attachedConnections;
        }
    }

    /// <summary>
    ///     Nodo al que está asociado el conector o nulo si no está asociado a nada
    /// </summary>
    public NodeViewModel? ParentNode { get; internal set; }

    /// <summary>
    /// The hotspot (or center) of the connector.
    /// This is pushed through from ConnectorItem in the UI.
    /// </summary>
    public Point Hotspot
    {
        get
        {
            return hotspot;
        }
        set
        {
            if (hotspot == value)
            {
                return;
            }

            hotspot = value;

            OnHotspotUpdated();
        }
    }

    /// <summary>
    /// Debug checking to ensure that no connection is added to the list twice.
    /// </summary>
    private void attachedConnections_ItemsAdded(object sender, CollectionItemsChangedEventArgs e)
    {
        foreach (ConnectionViewModel connection in e.Items)
        {
            connection.ConnectionChanged += new EventHandler<EventArgs>(connection_ConnectionChanged);
        }

        if ((AttachedConnections.Count - e.Items.Count) == 0)
        {
            // 
            // The first connection has been added, notify the data-binding system that
            // 'IsConnected' should be re-evaluated.
            //
            OnPropertyChanged("IsConnectionAttached");
            OnPropertyChanged("IsConnected");
        }
    }

    /// <summary>
    /// Event raised when connections have been removed from the connector.
    /// </summary>
    private void attachedConnections_ItemsRemoved(object sender, CollectionItemsChangedEventArgs e)
    {
        foreach (ConnectionViewModel connection in e.Items)
        {
            connection.ConnectionChanged -= new EventHandler<EventArgs>(connection_ConnectionChanged);
        }

        if (AttachedConnections.Count == 0)
        {
            // 
            // No longer connected to anything, notify the data-binding system that
            // 'IsConnected' should be re-evaluated.
            //
            OnPropertyChanged("IsConnectionAttached");
            OnPropertyChanged("IsConnected");
        }
    }

    /// <summary>
    /// Event raised when a connection attached to the connector has changed.
    /// </summary>
    private void connection_ConnectionChanged(object sender, EventArgs e)
    {
        OnPropertyChanged("IsConnectionAttached");
        OnPropertyChanged("IsConnected");
    }

    /// <summary>
    ///     Método al que se llama cuando se modifica el punto de entrada del conector
    /// </summary>
    private void OnHotspotUpdated()
    {
        // Lanza el evento de propiedad cambiada
        OnPropertyChanged("Hotspot");
        // Llama al evento
		HotspotUpdated?.Invoke(this, EventArgs.Empty);
	}
}
