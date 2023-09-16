using BauMvvm.Views.Wpf;

namespace Bau.Libraries.GraphChart.ViewModels;

/// <summary>
/// Defines a network of nodes and connections between the nodes.
/// </summary>
public sealed class NetworkViewModel
{
    /// <summary>
    /// The collection of nodes in the network.
    /// </summary>
    private ImpObservableCollection<NodeViewModel> nodes = null;

    /// <summary>
    /// The collection of connections in the network.
    /// </summary>
    private ImpObservableCollection<ConnectionViewModel> connections = null;

    /// <summary>
    /// The collection of nodes in the network.
    /// </summary>
    public ImpObservableCollection<NodeViewModel> Nodes
    {
        get
        {
            if (nodes == null)
            {
                nodes = new ImpObservableCollection<NodeViewModel>();
            }

            return nodes;
        }
    }

    /// <summary>
    /// The collection of connections in the network.
    /// </summary>
    public ImpObservableCollection<ConnectionViewModel> Connections
    {
        get
        {
            if (connections == null)
            {
                connections = new ImpObservableCollection<ConnectionViewModel>();
                connections.ItemsRemoved += new EventHandler<CollectionItemsChangedEventArgs>(connections_ItemsRemoved);
            }

            return connections;
        }
    }

    /// <summary>
    /// Event raised then Connections have been removed.
    /// </summary>
    private void connections_ItemsRemoved(object sender, CollectionItemsChangedEventArgs e)
    {
        foreach (ConnectionViewModel connection in e.Items)
        {
            connection.SourceConnector = null;
            connection.DestConnector = null;
        }
    }
}
