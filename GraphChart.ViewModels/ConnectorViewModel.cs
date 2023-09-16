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
    private string _name = default!;
    private ConnectorType _type = ConnectorType.Undefined;
    private ImpObservableCollection<ConnectionViewModel> _attachedConnections = new();
    private Point hotspot; // Punto o centro del conector (asociado a ConnectorItem)

    public ConnectorViewModel(string name)
    {
        Name = name;
        Type = ConnectorType.Undefined;
    }

    /// <summary>
    ///     Trata el evento de datos añadidos a la conexión
    /// </summary>
    private void attachedConnections_ItemsAdded(object? sender, CollectionItemsChangedEventArgs e)
    {
        // Añade los eventos a los elementos asociados
        foreach (ConnectionViewModel connection in e.Items)
            connection.ConnectionChanged += new EventHandler<EventArgs>(connection_ConnectionChanged);
        // Se ha añadido la primera conexión, indica que se debe reevaluar la conexión
        if ((AttachedConnections.Count - e.Items.Count) == 0)
        {
            OnPropertyChanged("IsConnectionAttached");
            OnPropertyChanged("IsConnected");
        }
    }

    /// <summary>
    ///     Trata el evento de datos eliminados a la conexión
    /// </summary>
    private void attachedConnections_ItemsRemoved(object? sender, CollectionItemsChangedEventArgs e)
    {
        // Quita los eventos a los elementos asociados
        foreach (ConnectionViewModel connection in e.Items)
            connection.ConnectionChanged -= new EventHandler<EventArgs>(connection_ConnectionChanged);
        // Si no queda niguna conexión, indica que se debe reevaluar la conexión
        if (AttachedConnections.Count == 0)
        {
            OnPropertyChanged("IsConnectionAttached");
            OnPropertyChanged("IsConnected");
        }
    }

    /// <summary>
    ///     Evento lanzado cuando una conexión asociada se ha modificado
    /// </summary>
    private void connection_ConnectionChanged(object? sender, EventArgs e)
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
        OnPropertyChanged(nameof(Hotspot));
        // Llama al evento
		HotspotUpdated?.Invoke(this, EventArgs.Empty);
	}

    /// <summary>
    ///     Nombre del conector
    /// </summary>
    public string Name
    {
        get { return _name; }
        set { CheckProperty(ref _name, value); }
    }

    /// <summary>
    ///     Tipo del conector
    /// </summary>
    public ConnectorType Type
    {
        get { return _type; }
        set { CheckProperty(ref _type, value); }
    }

    /// <summary>
    ///     Nodo al que está asociado el conector o nulo si no está asociado a nada
    /// </summary>
    public NodeViewModel? ParentNode { get; internal set; }

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
            // Crea las conexiones si no existían
            if (_attachedConnections is null)
            {
                _attachedConnections = new ImpObservableCollection<ConnectionViewModel>();
                _attachedConnections.ItemsAdded += new EventHandler<CollectionItemsChangedEventArgs>(attachedConnections_ItemsAdded);
                _attachedConnections.ItemsRemoved += new EventHandler<CollectionItemsChangedEventArgs>(attachedConnections_ItemsRemoved);
            }
            // Devuelve las conexiones
            return _attachedConnections;
        }
    }

    /// <summary>
    ///     El punto de asociación del conector. Asociado al ConnectorItem de la interface de usuario
    /// </summary>
    public Point Hotspot
    {
        get { return hotspot; }
        set
        {
            if (hotspot != value)
            {
                hotspot = value;
                OnHotspotUpdated();
            }
        }
    }
}
