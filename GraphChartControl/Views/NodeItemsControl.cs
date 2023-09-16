using System.Windows.Controls;
using System.Windows;

namespace Bau.Controls.GraphChartControl.Views;

/// <summary>
///     Listbox para mostrar los nodos
/// </summary>
internal class NodeItemsControl : ListBox
{
    public NodeItemsControl()
    {
        Focusable = false;
    }

    /// <summary>
    ///     Encuentra el elemento de tipo NodeItem que tiene un contexto especificado
    /// </summary>
    internal NodeItem? FindAssociatedNodeItem(object nodeDataContext) => ItemContainerGenerator.ContainerFromItem(nodeDataContext) as NodeItem;

    /// <summary>
    ///     Crea o identifica el elemento utilizado para mostrar un elemento
    /// </summary>
    protected override DependencyObject GetContainerForItemOverride() => new NodeItem();

    /// <summary>
    ///     Determina si el elemento especificado es o puede ser un contenedor
    /// </summary>
    protected override bool IsItemItsOwnContainerOverride(object item) => item is NodeItem;
}
