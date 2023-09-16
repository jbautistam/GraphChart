using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.GraphChart.ViewModels;

namespace Bau.Controls.GraphChartControl;

/// <summary>
///     Control de usuario para mostrar un gráfico de un flujo de trabajo
/// </summary>
public partial class GraphChartView : UserControl
{
    /// <summary>
    /// Defines the event handler for the ConnectionDragStarted event.
    /// </summary>
    public delegate void ConnectionDragStartedEventHandler(object sender, EventArguments.ConnectionDragStartedEventArgs e);

    /// <summary>
    /// Defines the event handler for the QueryConnectionFeedback event.
    /// </summary>
    public delegate void QueryConnectionFeedbackEventHandler(object sender, EventArguments.QueryConnectionFeedbackEventArgs e);

    /// <summary>
    /// Defines the event handler for the ConnectionDragging event.
    /// </summary>
    public delegate void ConnectionDraggingEventHandler(object sender, EventArguments.ConnectionDraggingEventArgs e);

    /// <summary>
    /// Defines the event handler for the ConnectionDragCompleted event.
    /// </summary>
    public delegate void ConnectionDragCompletedEventHandler(object sender, EventArguments.ConnectionDragCompletedEventArgs e);


	public GraphChartView()
	{
		InitializeComponent();
        DataContext = ViewModel = new GraphChartViewModel();
	}

    /// <summary>
    ///     Crea un nuevo nodo en la posición del raónt
    /// </summary>
    public void CreateNode()
    {
        Point newNodePosition = Mouse.GetPosition(networkControl);

            // Crea un nodo
            ViewModel.CreateNode($"Node {(ViewModel.Network.Nodes.Count + 1).ToString()}", newNodePosition, true);
    }

    /// <summary>
    ///     Ejecución del comando DeleteSelectedNodes
    /// </summary>
    private void DeleteSelectedNodes_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        ViewModel.DeleteSelectedNodes();
    }

    /// <summary>
    ///     Ejecución del comando DeleteNode
    /// </summary>
    private void DeleteNode_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is NodeViewModel node)
            ViewModel.DeleteNode(node);
    }

    /// <summary>
    ///     Ejecución del comando DeleteConnection
    /// </summary>
    private void DeleteConnection_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is ConnectionViewModel connection)
            ViewModel.DeleteConnection(connection);
    }

    /// <summary>
    /// Event raised when the size of a node has changed.
    /// </summary>
    private void Node_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        //
        // The size of a node, as determined in the UI by the node's data-template,
        // has changed.  Push the size of the node through to the view-model.
        //
        var element = (FrameworkElement) sender;
        var node = (NodeViewModel) element.DataContext;
        node.Size = new Size(element.ActualWidth, element.ActualHeight);
    }

    /// <summary>
    /// Event raised when the user has started to drag out a connection.
    /// </summary>
    private void networkControl_ConnectionDragStarted(object sender, EventArguments.ConnectionDragStartedEventArgs e)
    {
        var draggedOutConnector = (ConnectorViewModel) e.ConnectorDraggedOut;
        var curDragPoint = Mouse.GetPosition(networkControl);

        //
        // Delegate the real work to the view model.
        //
        var connection = ViewModel.ConnectionDragStarted(draggedOutConnector, curDragPoint);

        //
        // Must return the view-model object that represents the connection via the event args.
        // This is so that NetworkView can keep track of the object while it is being dragged.
        //
        e.Connection = connection;
    }

    /// <summary>
    /// Event raised, to query for feedback, while the user is dragging a connection.
    /// </summary>
    private void networkControl_QueryConnectionFeedback(object sender, EventArguments.QueryConnectionFeedbackEventArgs e)
    {
        var draggedOutConnector = (ConnectorViewModel) e.ConnectorDraggedOut;
        var draggedOverConnector= (ConnectorViewModel) e.DraggedOverConnector;
        object feedbackIndicator = null;
        bool connectionOk = true;

        ViewModel.QueryConnnectionFeedback(draggedOutConnector, draggedOverConnector, out feedbackIndicator, out connectionOk);

        //
        // Return the feedback object to NetworkView.
        // The object combined with the data-template for it will be used to create a 'feedback icon' to
        // display (in an adorner) to the user.
        //
        e.FeedbackIndicator = feedbackIndicator;

        //
        // Let NetworkView know if the connection is ok or not ok.
        //
        e.ConnectionOk = connectionOk;
    }

    /// <summary>
    ///     Evento lanzado cuando el usuario arrastra una conexión
    /// </summary>
    private void networkControl_ConnectionDragging(object sender, EventArguments.ConnectionDraggingEventArgs e)
    {
        Point curDragPoint = Mouse.GetPosition(networkControl);
            
            // Arranca el evento de arrastrar una conexión
            if (e.Connection is ConnectionViewModel connection)
                ViewModel.ConnectionDragging(curDragPoint, connection);
    }

    /// <summary>
    ///     Evento lanzado cuando el usuario finaliza de arrastrar una conexión
    /// </summary>
    private void networkControl_ConnectionDragCompleted(object sender, EventArguments.ConnectionDragCompletedEventArgs e)
    {
        ConnectorViewModel connectorDraggedOut = (ConnectorViewModel) e.ConnectorDraggedOut;
        ConnectorViewModel connectorDraggedOver = (ConnectorViewModel) e.ConnectorDraggedOver;
        ConnectionViewModel newConnection = (ConnectionViewModel) e.Connection;
        ViewModel.ConnectionDragCompleted(newConnection, connectorDraggedOut, connectorDraggedOver);
    }

    /// <summary>
    ///     Ejecución del comando CreateNode
    /// </summary>
    private void CreateNode_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        CreateNode();
    }

    /// <summary>
    ///     Acceso para el ViewModel del control
    /// </summary>
    public GraphChartViewModel ViewModel { get; }
}
