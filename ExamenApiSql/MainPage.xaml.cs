using ExamenApiSql.Models;
using ExamenApiSql.Services;

namespace ExamenApiSql
{
	public partial class MainPage : ContentPage
	{
		private ApiService _api = new ApiService();
		private List<Persona> _lista = new List<Persona>();
		private int _idEditando = 0;

		public MainPage()
		{
			InitializeComponent();
			Cargar();
		}

		private async void Cargar()
		{
			_lista = await _api.GetAll();
			AplicarFiltros();
		}

		private Persona ObtenerFormulario()
		{
			int.TryParse(EdadEntry.Text, out int edad);
			decimal.TryParse(SalarioEntry.Text, out decimal salario);

			return new Persona
			{
				Id = _idEditando,
				Nombre = NombreEntry.Text,
				Edad = edad,
				Salario = salario,
				Ciudad = CiudadEntry.Text,
				Activo = ActivoSwitch.IsToggled
			};
		}

		private async void Insertar_Clicked(object sender, EventArgs e)
		{
			await _api.Insert(ObtenerFormulario());
			_idEditando = 0;
			Cargar();
		}

		private async void Actualizar_Clicked(object sender, EventArgs e)
		{
			if (_idEditando == 0)
				return;

			await _api.Update(ObtenerFormulario());
			_idEditando = 0;
			Cargar();
		}

		private async void Borrar_Clicked(object sender, EventArgs e)
		{
			var seleccionados = _lista.Where(x => x.Seleccionado).ToList();

			foreach (var p in seleccionados)
				await _api.Delete(p.Id);

			Cargar();
		}

		private void Editar_Clicked(object sender, EventArgs e)
		{
			var p = (sender as Button).BindingContext as Persona;

			_idEditando = p.Id;

			NombreEntry.Text = p.Nombre;
			EdadEntry.Text = p.Edad.ToString();
			SalarioEntry.Text = p.Salario.ToString();
			CiudadEntry.Text = p.Ciudad;
			ActivoSwitch.IsToggled = p.Activo;
		}

		private void Filtros_Changed(object sender, EventArgs e)
		{
			AplicarFiltros();
		}

		private void Refrescar_Clicked(object sender, EventArgs e)
		{
			Cargar();
		}

		private void AplicarFiltros()
		{
			string fn = FiltroNombreEntry.Text?.ToLower() ?? "";
			string fc = FiltroCiudadEntry.Text?.ToLower() ?? "";
			decimal.TryParse(FiltroSalarioEntry.Text, out decimal smin);
			bool activos = FiltroActivoSwitch.IsToggled;

			var filtrada = _lista
				.Where(p =>
					(fn == "" || p.Nombre.ToLower().StartsWith(fn)) &&
					(fc == "" || p.Ciudad.ToLower().StartsWith(fc)) &&
					(smin == 0 || p.Salario >= smin) &&
					(!activos || p.Activo)
				)
				.OrderBy(p => p.Nombre)
				.ToList();

			PersonasCollectionView.ItemsSource = filtrada;
		}
	}
}