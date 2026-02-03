namespace ExamenApiSql.Models
{
	public class PersonaMySql
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public decimal Salario { get; set; }
		public DateTime Fecha { get; set; }
		public int Codigo { get; set; }

		public bool Seleccionado { get; set; }
	}
}
