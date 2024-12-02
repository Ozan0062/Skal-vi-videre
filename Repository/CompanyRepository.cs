using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skal_vi_videre.Repository.Base;
using System.Net.Http;
using Newtonsoft.Json;

namespace Skal_vi_videre.Repository
{
    public class CompanyRepository : BaseRepository<Company>
    {
        private PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();
        private readonly HttpClient _httpClient;

        public CompanyRepository()
        {
            _httpClient = new HttpClient();
        }

        public override int Create(Company company)
        {
            company.Password = _passwordHasher.HashPassword(company.Email, company.Password);
            return base.Create(company);
        }

        public Company? GetByCvr(string cvrId)
        {
            using DbContext context = CreateDbContext();
            Company? result = GetAllWithIncludes(context).FirstOrDefault(t => t.Cvr == cvrId);

            return result;
        }

        public Company? VerifyCompany(string providedEmail, string providedPassword)
        {
            Company? company = GetAll().FirstOrDefault(u => u.Email == providedEmail);

            if (company == null ||
            _passwordHasher.VerifyHashedPassword(
                    providedEmail,
                    company.Password,
                    providedPassword)
                != PasswordVerificationResult.Success)
                return null;

            return company;
        }

        public async Task<bool> ValidateCompanyAsync(string cvrNumber, string companyName)
        {
            if (string.IsNullOrEmpty(cvrNumber) || string.IsNullOrEmpty(companyName))
            {
                throw new ArgumentException("CVR-nummer og virksomhedsnavn må ikke være tomme.");
            }

            try
            {
                // Opbyg URL med CVR API
                string url = $"https://cvrapi.dk/api?search={cvrNumber}&country=dk";

                // Tilføj User-Agent header
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Afsluttende eksamensprojekt");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                // Tjek om svaret er succesfuldt
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Fejl ved kald til CVR API: {response.StatusCode}");
                }

                // Læs og parse JSON-resultatet
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Respons: {responseBody}");
                var companyData = JsonConvert.DeserializeObject<CvrApiResponse>(responseBody);

                // Tjek om virksomhedsnavnet matcher
                return companyData?.Name?.Equals(companyName, StringComparison.OrdinalIgnoreCase) ?? false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl under validering: {ex.Message}");
                return false;
            }
        }


        // Data model til at matche CVR API-respons
        public class CvrApiResponse
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("vat")]
            public string Cvr { get; set; }
        }
    }
}
