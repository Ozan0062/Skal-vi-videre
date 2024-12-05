using Newtonsoft.Json;
using Skal_vi_videre.Repository.Base;

namespace Skal_vi_videre
{
    public partial class Company : IHasId, IUpdateFromOther<Company>
    {
        private readonly HttpClient _httpClient;

        public Company()
        {
            _httpClient = new HttpClient();
        }

        public void Update(Company tOther)
        {
            Validate();
            Cvr = tOther.Cvr;
            Name = tOther.Name;
            Address = tOther.Address;
            Email = tOther.Email;
            Password = tOther.Password;
            PhoneNumber = tOther.PhoneNumber;
            Role = tOther.Role;
            Description = tOther.Description;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Role) || string.IsNullOrEmpty(Description))
            {
                throw new ArgumentNullException("Det er null eller tom");
            }
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

                // Tjek om CVR API returnerede data
                if (companyData == null || string.IsNullOrEmpty(companyData.Name))
                {
                    throw new ArgumentException("CVR-nummer findes ikke eller returnerede ingen data.");
                }

                // Normaliser virksomhedsnavne til sammenligning
                string normalizedApiName = NormalizeCompanyName(companyData.Name);
                string normalizedInputName = NormalizeCompanyName(companyName);

                // Tjek om de normaliserede navne matcher
                if (!string.Equals(normalizedApiName, normalizedInputName, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("Virksomhedsnavnet matcher ikke CVR-nummeret.");
                }

                //return companyData?.Name?.Equals(companyName, StringComparison.OrdinalIgnoreCase) ?? false;
                return true; // Valideringen er succesfuld
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl under validering: {ex.Message}");
                return false;
            }
        }

        private string NormalizeCompanyName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            // Fjern kendte suffixer som ApS, A/S, etc.
            string[] suffixesToRemove = { "aps", "a/s", "as", "i/s", "is", "pmv" };

            // Fjern suffixer fra navnet
            foreach (var suffix in suffixesToRemove)
            {
                name = name.Replace(suffix, "", StringComparison.OrdinalIgnoreCase);
            }

            // Trim ekstra mellemrum og returner
            return name.Trim();
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