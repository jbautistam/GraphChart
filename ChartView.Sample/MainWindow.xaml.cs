using System.Windows;

namespace Bau.ChartView.Sample;

/// <summary>
///     Ventana de ejemplo de visualización del grafo
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Crea un nodo
    /// </summary>
    private void CreateNode()
    {
        grpControl.CreateNode();
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
    }

	private void CreateNode_Click(object sender, RoutedEventArgs e)
	{
        CreateNode();
	}
}
