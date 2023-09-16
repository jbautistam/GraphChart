using System.Windows;

using Bau.Libraries.GraphChart.ViewModels.Base;

namespace Bau.Libraries.GraphChart.ViewModels;

/// <summary>
///     ViewModel de un nodo
///     Los nodos se conectan a otros a través de los <see cref="ConnectorViewModel"/>
/// </summary>
public sealed class NodeViewModel : BaseObservableObject
{
    // Eventos públicos
    public event EventHandler<EventArgs>? SizeChanged;
    // Variables privadas
    private string _name = string.Empty;
    private double _x,_y;
    private int _zIndex;
    private Size _size = Size.Empty;
    private ImpObservableCollection<ConnectorViewModel> _inputConnectors = default!;
    private ImpObservableCollection<ConnectorViewModel> _outputConnectors = default!;
    private bool _isSelected = false;

    public NodeViewModel() : this(string.Empty) {}

    public NodeViewModel(string name)
    {
        Name = name;
    }

    /// <summary>
    ///     Eventos lanzados cuando los conectores se añaden al nodo
    /// </summary>
    private void inputConnectors_ItemsAdded(object? sender, CollectionItemsChangedEventArgs e)
    {
        UpdateConnectorsType(e, ConnectorViewModel.ConnectorType.Input);
    }

    /// <summary>
    ///     Evento lanzado cuando los conectores se eliminan del nodo
    /// </summary>
    private void inputConnectors_ItemsRemoved(object? sender, CollectionItemsChangedEventArgs e)
    {
        UpdateConnectorsType(e, ConnectorViewModel.ConnectorType.Undefined);
    }

    /// <summary>
    ///     Evento lanzado cuando los conectores se añaden al nodo
    /// </summary>
    private void outputConnectors_ItemsAdded(object? sender, CollectionItemsChangedEventArgs e)
    {
        UpdateConnectorsType(e, ConnectorViewModel.ConnectorType.Output);
    }

    /// <summary>
    ///     Evento lanzado cuando los conectores se eliminan del nodo
    /// </summary>
    private void outputConnectors_ItemsRemoved(object? sender, CollectionItemsChangedEventArgs e)
    {
        UpdateConnectorsType(e, ConnectorViewModel.ConnectorType.Undefined);
    }

    /// <summary>
    ///     Modifica los tipos de conectores
    /// </summary>
    private void UpdateConnectorsType(CollectionItemsChangedEventArgs e, ConnectorViewModel.ConnectorType type)
    {
        foreach (ConnectorViewModel connector in e.Items)
        {
            connector.ParentNode = this;
            connector.Type = type;
        }
    }

    /// <summary>
    ///     Nombre del nodo
    /// </summary>
    public string Name
    {
        get { return _name; }
        set { CheckProperty(ref _name, value); }
    }

    /// <summary>
    ///     Coordenada X del nodo
    /// </summary>
    public double X
    {
        get { return _x; }
        set { CheckProperty(ref _x, value); }
    }

    /// <summary>
    ///     Coordenada Y del nodo
    /// </summary>
    public double Y
    {
        get { return _y; }
        set { CheckProperty(ref _y, value); }
    }

    /// <summary>
    ///     El índice Z del nodo
    /// </summary>
    public int ZIndex
    {
        get { return _zIndex; }
        set { CheckProperty(ref _zIndex, value); }
    }

    /// <summary>
    ///     Tamaño del nodo
    ///     El tamaño de un nodo en la IU no lo determina esta propiedad
    ///     En lugar de este tamaño, en la IU se determina la plantilla de datos de la clase del nodo
    ///     Cuando el tamaño se calcula en la IU, se introduce en el ViewModel de modo que la aplicación
    /// puede acceder a este tamaño
    /// </summary>
    public Size Size
    {
        get { return _size; }
        set
        {
            if (CheckObject(ref _size, value))
				SizeChanged?.Invoke(this, EventArgs.Empty);
		}
    }

    /// <summary>
    ///     Lista de puntos de conexión de entrada asociados al nodo
    /// </summary>
    public ImpObservableCollection<ConnectorViewModel> InputConnectors
    {
        get
        {
            // Crea los conectores si no existían
            if (_inputConnectors is null)
            {
                _inputConnectors = new ImpObservableCollection<ConnectorViewModel>();
                _inputConnectors.ItemsAdded += new EventHandler<CollectionItemsChangedEventArgs>(inputConnectors_ItemsAdded);
                _inputConnectors.ItemsRemoved += new EventHandler<CollectionItemsChangedEventArgs>(inputConnectors_ItemsRemoved);
            }
            // Devuelve los conectores de entrada
            return _inputConnectors;
        }
    }

    /// <summary>
    ///     Lista de puntos de conexión de salida asociados al nodo
    /// </summary>
    public ImpObservableCollection<ConnectorViewModel> OutputConnectors
    {
        get
        {
            // Crea los conectores si no existen
            if (_outputConnectors == null)
            {
                _outputConnectors = new ImpObservableCollection<ConnectorViewModel>();
                _outputConnectors.ItemsAdded += new EventHandler<CollectionItemsChangedEventArgs>(outputConnectors_ItemsAdded);
                _outputConnectors.ItemsRemoved += new EventHandler<CollectionItemsChangedEventArgs>(outputConnectors_ItemsRemoved);
            }
            // Devuelve los conectores de salida
            return _outputConnectors;
        }
    }

    /// <summary>
    ///     Recupera una lista de todas las conexiones asociadas al nodo
    /// </summary>
    public ICollection<ConnectionViewModel> AttachedConnections
    {
        get
        {
            List<ConnectionViewModel> attachedConnections = new();

                // Añade las conexiones de entrada
                foreach (ConnectorViewModel connector in InputConnectors)
                    attachedConnections.AddRange(connector.AttachedConnections);
                // Añade las conexiones de salida
                foreach (ConnectorViewModel connector in OutputConnectors)
                    attachedConnections.AddRange(connector.AttachedConnections);
                // Devuelve las conexiones asociadas
                return attachedConnections;
        }
    }

    /// <summary>
    ///     Indica si el nodo está seleccionado
    /// </summary>
    public bool IsSelected
    {
        get { return _isSelected; }
        set { CheckProperty(ref _isSelected, value); }
    }
}
