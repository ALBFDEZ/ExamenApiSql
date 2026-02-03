using MySql.Data.MySqlClient;
using ExamenApiSql.Models;

namespace ExamenApiSql.Services
{
	public class MySqlService
	{
		private readonly string _conn =
			"Server=127.0.0.1;Port=3306;Database=mibase;Uid=root;Pwd=1234;";

		public List<PersonaMySql> GetAll()
		{
			var lista = new List<PersonaMySql>();

			using var conn = new MySqlConnection(_conn);
			conn.Open();

			string sql = "SELECT idPersona, nombre, salario, fecha, codigo FROM persona";

			using var cmd = new MySqlCommand(sql, conn);
			using var rd = cmd.ExecuteReader();

			while (rd.Read())
			{
				lista.Add(new PersonaMySql
				{
					Id = rd.GetInt32("idPersona"),
					Nombre = rd.GetString("nombre"),
					Salario = rd.GetDecimal("salario"),
					Fecha = rd.GetDateTime("fecha"),
					Codigo = rd.GetInt32("codigo")
				});
			}

			return lista;
		}

		public void Insert(PersonaMySql p)
		{
			using var conn = new MySqlConnection(_conn);
			conn.Open();

			string sql = @"INSERT INTO persona (nombre, salario, fecha, codigo)
                           VALUES (@n, @s, @f, @c)";

			using var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@n", p.Nombre);
			cmd.Parameters.AddWithValue("@s", p.Salario);
			cmd.Parameters.AddWithValue("@f", p.Fecha);
			cmd.Parameters.AddWithValue("@c", p.Codigo);
			cmd.ExecuteNonQuery();
		}

		public void Update(PersonaMySql p)
		{
			using var conn = new MySqlConnection(_conn);
			conn.Open();

			string sql = @"UPDATE persona SET nombre=@n, salario=@s, fecha=@f, codigo=@c
                           WHERE id=@id";

			using var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@n", p.Nombre);
			cmd.Parameters.AddWithValue("@s", p.Salario);
			cmd.Parameters.AddWithValue("@f", p.Fecha);
			cmd.Parameters.AddWithValue("@c", p.Codigo);
			cmd.Parameters.AddWithValue("@id", p.Id);
			cmd.ExecuteNonQuery();
		}

		public void Delete(int id)
		{
			using var conn = new MySqlConnection(_conn);
			conn.Open();

			string sql = "DELETE FROM persona WHERE idPersona=@id";

			using var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@id", id);
			cmd.ExecuteNonQuery();
		}
	}
}
