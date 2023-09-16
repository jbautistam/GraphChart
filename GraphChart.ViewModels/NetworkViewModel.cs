using Bau.Libraries.GraphChart.ViewModels.Base;

namespace Bau.Libraries.GraphChart.ViewModels;

/// <summary>
///     Declara una red de notods y las conexiones entre los nodos
/// </summary>
public sealed class NetworkViewModel
{
    // Variables privadas
    private ImpObservableCollection<NodeViewModel> _nodes = default!;
    private ImpObservableCollection<ConnectionViewModel> _connections = default!;

	/// <summary>
	///     Elimina los nodos seleccionados
	/// </summary>
	public void DeleteSelectedNodes()
	{
        for (int index = Nodes.Count - 1; index >= 0; index--)
        {
            // Elimina las conexiones
            DeleteConnections(Nodes[index]);
            // Elimina el nodo
            Nodes.RemoveAt(index);
        }
	}

	/// <summary>
	///     Elimina los datos de un nodo
	/// </summary>
	public void DeleteNode(NodeViewModel node)
	{
        // Elimina las conexiones asociadas al nodo
        DeleteConnections(node);
        // Elimina el nodo de la red
		Nodes.Remove(node);
	}

    /// <summary>
    ///     Elimina las conexiones del nodo
    /// </summary>
	private void DeleteConnections(NodeViewModel node)
	{
		Connections.RemoveRange(node.AttachedConnections);
	}

	/// <summary>
	///     Evento lanzado cuando se eliminan las conexiones
	/// </summary>
	private void connections_ItemsRemoved(object? sender, CollectionItemsChangedEventArgs e)
    {
        if (e.Items is not null)
            foreach (ConnectionViewModel connection in e.Items)
            {
                connection.SourceConnector = null;
                connection.DestConnector = null;
            }
    }

    /// <summary>
    ///     Nodos de la red
    /// </summary>
    public ImpObservableCollection<NodeViewModel> Nodes
    {
        get
        {
            // Genera los nodos si no existían
            if (_nodes is null)
                _nodes = new ImpObservableCollection<NodeViewModel>();
            // Devuelve lalista de nodos
            return _nodes;
        }
    }

    /// <summary>
    ///     Conexiones de la red
    /// </summary>
    public ImpObservableCollection<ConnectionViewModel> Connections
    {
        get
        {
            // Genera las conexiones si no existían
            if (_connections is null)
            {
                _connections = new ImpObservableCollection<ConnectionViewModel>();
                _connections.ItemsRemoved += new EventHandler<CollectionItemsChangedEventArgs>(connections_ItemsRemoved);
            }
            // Devuelve la lista de conexiones
            return _connections;
        }
    }
}
