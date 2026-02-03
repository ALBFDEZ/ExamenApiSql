namespace ExamenApiSql.Models
{
	public class Persona
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public int Edad { get; set; }
		public decimal Salario { get; set; }
		public bool Activo { get; set; }
		public string Ciudad { get; set; }

		public bool Seleccionado { get; set; }
	}
}
