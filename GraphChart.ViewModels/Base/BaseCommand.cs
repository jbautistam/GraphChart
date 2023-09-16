using System.ComponentModel;
using System.Windows.Input;

namespace Bau.Libraries.GraphChart.ViewModels.Base;

/// <summary>
///		Clase base para los comandos
/// </summary>
public class BaseCommand : ICommand
{ 
	// Eventos públicos
	public event EventHandler? CanExecuteChanged;
	// Variables privadas
	private readonly Action<object?> _executeMethod;
	private readonly Predicate<object?>? _canExecuteMethod = null;

	public BaseCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
	{
		_executeMethod = execute;
		_canExecuteMethod = canExecute;
	}

	/// <summary>
	///		Ejecuta un comando
	/// </summary>
	public void Execute(object? parameter)
	{
		_executeMethod?.Invoke(parameter);
	}

	/// <summary>
	///		Comprueba si se puede ejecutar un comando
	/// </summary>
	public bool CanExecute(object? parameter)
	{
		if (_canExecuteMethod is not null)
			return _canExecuteMethod(parameter);
		else
			return true;
	}

	/// <summary>
	///		Añade un listener de eventos al comando para un nombre de propiedad
	/// </summary>
	public BaseCommand AddListener(INotifyPropertyChanged source, string? propertyName)
	{ 
		// Añade el manejador de eventos
		source.PropertyChanged += (sender, args) => OnCanExecuteChanged();
		// Devuelve este objeto (permite un interface fluent)
		return this;
	}

	/// <summary>
	///		Rutina a la que se llama para lanzar el evento de modificación de CanExecute
	/// </summary>
	public void OnCanExecuteChanged()
	{
		try
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
		catch (Exception ex) 
		{
			System.Diagnostics.Debug.WriteLine(ex.Message);
		}
	}
}
