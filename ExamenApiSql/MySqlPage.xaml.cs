using ExamenApiSql.Services;
using ExamenApiSql.Models;

namespace ExamenApiSql
{
	public partial class MySqlPage : ContentPage
	{
		private MySqlService _sql = new MySqlService();
		private List<PersonaMySql> _lista = new List<PersonaMySql>();
		private int _idEditando = 0;

		public MySqlPage()
		{
			InitializeComponent();
			Cargar();
		}

		private void Cargar()
		{
			_lista = _sql.GetAll();
			AplicarFiltros();
		}

		private void Refrescar_Clicked(object sender, EventArgs e)
		{
			Cargar();
		}

		private PersonaMySql ObtenerFormulario()
		{
			decimal.TryParse(SalarioEntry.Text, out decimal salario);
			int.TryParse(CodigoEntry.Text, out int codigo);
			DateTime.TryParse(FechaEntry.Text, out DateTime fecha);

			return new PersonaMySql
			{
				Id = _idEditando,
				Nombre = NombreEntry.Text,
				Salario = salario,
				Fecha = fecha,
				Codigo = codigo
			};
		}

		private void Insertar_Clicked(object sender, EventArgs e)
		{
			_sql.Insert(ObtenerFormulario());
			_idEditando = 0;
			Cargar();
		}

		private void Actualizar_Clicked(object sender, EventArgs e)
		{
			if (_idEditando == 0)
				return;

			_sql.Update(ObtenerFormulario());
			_idEditando = 0;
			Cargar();
		}

		private void Borrar_Clicked(object sender, EventArgs e)
		{
			var seleccionados = _lista.Where(x => x.Seleccionado).ToList();

			foreach (var p in seleccionados)
				_sql.Delete(p.Id);

			Cargar();
		}

		private void Editar_Clicked(object sender, EventArgs e)
		{
			var p = (sender as Button).BindingContext as PersonaMySql;

			_idEditando = p.Id;

			NombreEntry.Text = p.Nombre;
			SalarioEntry.Text = p.Salario.ToString();
			FechaEntry.Text = p.Fecha.ToString("yyyy-MM-dd");
			CodigoEntry.Text = p.Codigo.ToString();
		}

		private void Filtros_Changed(object sender, EventArgs e)
		{
			AplicarFiltros();
		}

		private void AplicarFiltros()
		{
			string filtroNombre = FiltroNombreEntry.Text?.ToLower() ?? "";
			string filtroFecha = FiltroFechaEntry.Text ?? "";
			int.TryParse(FiltroCodigoEntry.Text, out int filtroCodigo);

			var filtrada = _lista
				.Where(p =>
					(string.IsNullOrEmpty(filtroNombre) ||
					 p.Nombre.ToLower().StartsWith(filtroNombre))
					&&
					(string.IsNullOrEmpty(filtroFecha) ||
					 p.Fecha.ToString("yyyy-MM-dd").StartsWith(filtroFecha))
					&&
					(filtroCodigo == 0 || p.Codigo == filtroCodigo)
				)
				.OrderBy(p => p.Nombre)
				.ToList();

			PersonasCollectionView.ItemsSource = filtrada;
		}
	}
}