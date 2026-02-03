using System.Net.Http.Json;
using ExamenApiSql.Models;

namespace ExamenApiSql.Services
{
	public class ApiService
	{
		private readonly string _url = "https://697e266a97386252a26a01ea.mockapi.io/Registro";
		private readonly HttpClient _http = new HttpClient();

		public async Task<List<Persona>> GetAll()
		{
			return await _http.GetFromJsonAsync<List<Persona>>(_url);
		}

		public async Task<bool> Insert(Persona p)
		{
			var resp = await _http.PostAsJsonAsync(_url, p);
			return resp.IsSuccessStatusCode;
		}

		public async Task<bool> Update(Persona p)
		{
			var resp = await _http.PutAsJsonAsync($"{_url}/{p.Id}", p);
			return resp.IsSuccessStatusCode;
		}

		public async Task<bool> Delete(int id)
		{
			var resp = await _http.DeleteAsync($"{_url}/{id}");
			return resp.IsSuccessStatusCode;
		}
	}
}
