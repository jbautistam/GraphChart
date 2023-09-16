using System.Windows;

using Bau.Libraries.GraphChart.ViewModels;
using Bau.Libraries.GraphChart.ViewModels.Base;

namespace Bau.Controls.GraphChartControl;

/// <summary>
///		ViewModel del control de gráficos
/// </summary>
public class GraphChartViewModel : BaseObservableObject
{
	// Variables privadas
	private double _contentScale = 1;
	private double _contentOffsetX, _contentOffsetY, _contentViewportWidth, _contentViewportHeight;
	private double _contentWidth = 1000;
	private double _contentHeight = 1000;
	private NetworkViewModel _networkViewModel = default!;

	public GraphChartViewModel()
	{
		Network = new NetworkViewModel();
		PopulateWithTestData();
		CreateNodeCommand = new BaseCommand(_ => CreateNode());
	}

	/// <summary>
	/// Called when the user has started to drag out a connector, thus creating a new connection.
	/// </summary>
	public ConnectionViewModel ConnectionDragStarted(ConnectorViewModel draggedOutConnector, Point curDragPoint)
	{
		//
		// Create a new connection to add to the view-model.
		//
		var connection = new ConnectionViewModel();

		if (draggedOutConnector.Type == ConnectorViewModel.ConnectorType.Output)
		{
			//
			// The user is dragging out a source connector (an output) and will connect it to a destination connector (an input).
			//
			connection.SourceConnector = draggedOutConnector;
			connection.DestConnectorHotspot = curDragPoint;
		}
		else
		{
			//
			// The user is dragging out a destination connector (an input) and will connect it to a source connector (an output).
			//
			connection.DestConnector = draggedOutConnector;
			connection.SourceConnectorHotspot = curDragPoint;
		}

		//
		// Add the new connection to the view-model.
		//
		this.Network.Connections.Add(connection);

		return connection;
	}

	/// <summary>
	/// Called to query the application for feedback while the user is dragging the connection.
	/// </summary>
	public void QueryConnnectionFeedback(ConnectorViewModel draggedOutConnector, ConnectorViewModel draggedOverConnector, out object feedbackIndicator, out bool connectionOk)
	{
		if (draggedOutConnector == draggedOverConnector)
		{
			//
			// Can't connect to self!
			// Provide feedback to indicate that this connection is not valid!
			//
			feedbackIndicator = new Indicators.ConnectionBadIndicator();
			connectionOk = false;
		}
		else
		{
			var sourceConnector = draggedOutConnector;
			var destConnector = draggedOverConnector;

			//
			// Only allow connections from output connector to input connector (ie each
			// connector must have a different type).
			// Also only allocation from one node to another, never one node back to the same node.
			//
			connectionOk = sourceConnector.ParentNode != destConnector.ParentNode &&
							 sourceConnector.Type != destConnector.Type;

			if (connectionOk)
			{
				// 
				// Yay, this is a valid connection!
				// Provide feedback to indicate that this connection is ok!
				//
				feedbackIndicator = new Indicators.ConnectionOkIndicator();
			}
			else
			{
				//
				// Connectors with the same connector type (eg input & input, or output & output)
				// can't be connected.
				// Only connectors with separate connector type (eg input & output).
				// Provide feedback to indicate that this connection is not valid!
				//
				feedbackIndicator = new Indicators.ConnectionBadIndicator();
			}
		}
	}

	/// <summary>
	/// Called as the user continues to drag the connection.
	/// </summary>
	public void ConnectionDragging(Point curDragPoint, ConnectionViewModel connection)
	{
		if (connection.DestConnector == null)
			connection.DestConnectorHotspot = curDragPoint;
		else
			connection.SourceConnectorHotspot = curDragPoint;
	}

	/// <summary>
	/// Called when the user has finished dragging out the new connection.
	/// </summary>
	public void ConnectionDragCompleted(ConnectionViewModel newConnection, ConnectorViewModel connectorDraggedOut, ConnectorViewModel connectorDraggedOver)
	{
		if (connectorDraggedOver == null)
		{
			//
			// The connection was unsuccessful.
			// Maybe the user dragged it out and dropped it in empty space.
			//
			this.Network.Connections.Remove(newConnection);
			return;
		}

		//
		// Only allow connections from output connector to input connector (ie each
		// connector must have a different type).
		// Also only allocation from one node to another, never one node back to the same node.
		//
		bool connectionOk = connectorDraggedOut.ParentNode != connectorDraggedOver.ParentNode &&
							connectorDraggedOut.Type != connectorDraggedOver.Type;

		if (!connectionOk)
		{
			//
			// Connections between connectors that have the same type,
			// eg input -> input or output -> output, are not allowed,
			// Remove the connection.
			//
			this.Network.Connections.Remove(newConnection);
			return;
		}

		//
		// The user has dragged the connection on top of another valid connector.
		//

		//
		// Remove any existing connection between the same two connectors.
		//
		var existingConnection = FindConnection(connectorDraggedOut, connectorDraggedOver);
		if (existingConnection != null)
			this.Network.Connections.Remove(existingConnection);

		//
		// Finalize the connection by attaching it to the connector
		// that the user dragged the mouse over.
		//
		if (newConnection.DestConnector == null)
			newConnection.DestConnector = connectorDraggedOver;
		else
			newConnection.SourceConnector = connectorDraggedOver;
	}

	/// <summary>
	/// Retrieve a connection between the two connectors.
	/// Returns null if there is no connection between the connectors.
	/// </summary>
	public ConnectionViewModel? FindConnection(ConnectorViewModel start, ConnectorViewModel end)
	{
		//
		// Figure out which one is the source connector and which one is the
		// destination connector based on their connector types.
		//
		var sourceConnector = start.Type == ConnectorViewModel.ConnectorType.Output ? start : end;
		var destConnector = start.Type == ConnectorViewModel.ConnectorType.Output ? end : start;

		//
		// Now we can just iterate attached connections of the source
		// and see if it each one is attached to the destination connector.
		//

		foreach (var connection in sourceConnector.AttachedConnections)
			if (connection.DestConnector == destConnector)
				return connection;

		return null;
	}

	/// <summary>
	///		Crea un nodo
	/// </summary>
	private void CreateNode()
	{
		System.Diagnostics.Debug.WriteLine("Create node");
	}

	/// <summary>
	///		Elimina los nodos seleccionados del ViewModel
	/// </summary>
	public void DeleteSelectedNodes()
	{
		Network.DeleteSelectedNodes();
	}

	/// <summary>
	///		Borra los datos de un nodo y sus conectores asociados
	/// </summary>
	public void DeleteNode(NodeViewModel node)
	{
		Network.DeleteNode(node);
	}

	/// <summary>
	/// Create a node and add it to the view-model.
	/// </summary>
	public NodeViewModel CreateNode(string name, Point nodeLocation, bool centerNode)
	{
		var node = new NodeViewModel(name);
		node.X = nodeLocation.X;
		node.Y = nodeLocation.Y;

		node.InputConnectors.Add(new ConnectorViewModel("In1"));
		node.InputConnectors.Add(new ConnectorViewModel("In2"));
		node.OutputConnectors.Add(new ConnectorViewModel("Out1"));
		node.OutputConnectors.Add(new ConnectorViewModel("Out2"));

		if (centerNode)
		{
			// 
			// We want to center the node.
			//
			// For this to happen we need to wait until the UI has determined the 
			// size based on the node's data-template.
			//
			// So we define an anonymous method to handle the SizeChanged event for a node.
			//
			// Note: If you don't declare sizeChangedEventHandler before initializing it you will get
			//       an error when you try and unsubscribe the event from within the event handler.
			//
			EventHandler<EventArgs> sizeChangedEventHandler = null;
			sizeChangedEventHandler =
				delegate (object sender, EventArgs e)
				{
					//
					// This event handler will be called after the size of the node has been determined.
					// So we can now use the size of the node to modify its position.
					//
					node.X -= node.Size.Width / 2;
					node.Y -= node.Size.Height / 2;

					//
					// Don't forget to unhook the event, after the initial centering of the node
					// we don't need to be notified again of any size changes.
					//
					node.SizeChanged -= sizeChangedEventHandler;
				};

			//
			// Now we hook the SizeChanged event so the anonymous method is called later
			// when the size of the node has actually been determined.
			//
			node.SizeChanged += sizeChangedEventHandler;
		}

		//
		// Add the node to the view-model.
		//
		this.Network.Nodes.Add(node);

		return node;
	}

	/// <summary>
	/// Utility method to delete a connection from the view-model.
	/// </summary>
	public void DeleteConnection(ConnectionViewModel connection)
	{
		this.Network.Connections.Remove(connection);
	}

	/// <summary>
	/// A function to conveniently populate the view-model with test data.
	/// </summary>
	private void PopulateWithTestData()
	{
		//
		// Create a network, the root of the view-model.
		//
		this.Network = new NetworkViewModel();

		//
		// Create some nodes and add them to the view-model.
		//
		NodeViewModel node1 = CreateNode("Node1", new Point(100, 60), false);
		NodeViewModel node2 = CreateNode("Node2", new Point(350, 80), false);

		//
		// Create a connection between the nodes.
		//
		ConnectionViewModel connection = new ConnectionViewModel();
		connection.SourceConnector = node1.OutputConnectors[0];
		connection.DestConnector = node2.InputConnectors[0];

		//
		// Add the connection to the view-model.
		//
		this.Network.Connections.Add(connection);
	}

	/// <summary>
	/// This is the network that is displayed in the window.
	/// It is the main part of the view-model.
	/// </summary>
	public NetworkViewModel Network
	{
		get { return _networkViewModel; }
		set { CheckObject(ref _networkViewModel, value); }
	}

	///
	/// The current scale at which the content is being viewed.
	/// 
	public double ContentScale
	{
		get { return _contentScale; }
		set { CheckProperty(ref _contentScale, value); }
	}

	///
	/// The X coordinate of the offset of the viewport onto the content (in content coordinates).
	/// 
	public double ContentOffsetX
	{
		get { return _contentOffsetX; }
		set { CheckProperty(ref _contentOffsetX, value); }
	}

	///
	/// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
	/// 
	public double ContentOffsetY
	{
		get { return _contentOffsetY; }
		set { CheckProperty(ref _contentOffsetY, value); }
	}

	///
	/// The width of the content (in content coordinates).
	/// 
	public double ContentWidth
	{
		get { return _contentWidth; }
		set { CheckProperty(ref _contentWidth, value); }
	}

	///
	/// The heigth of the content (in content coordinates).
	/// 
	public double ContentHeight
	{
		get { return _contentHeight; }
		set { CheckProperty(ref _contentHeight, value); }
	}

	///
	/// The width of the viewport onto the content (in content coordinates).
	/// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
	/// view-model so that the value can be shared with the overview window.
	/// 
	public double ContentViewportWidth
	{
		get { return _contentViewportWidth; }
		set { CheckProperty(ref _contentViewportWidth, value); }
	}

	///
	/// The heigth of the viewport onto the content (in content coordinates).
	/// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
	/// view-model so that the value can be shared with the overview window.
	/// 
	public double ContentViewportHeight
	{
		get { return _contentViewportHeight; }
		set { CheckProperty(ref _contentViewportHeight, value); }
	}

	/// <summary>
	///		Comando de creación de un nodo
	/// </summary>
	public BaseCommand CreateNodeCommand { get; }
}