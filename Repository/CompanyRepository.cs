using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Skal_vi_videre.Repository.Base;

namespace Skal_vi_videre.Repository
{
    public class CompanyRepository : BaseRepository<Company>
    {
        private PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        public static DBContext? dbContext { get; set; }

        public override int Create(Company company)
        {
            company.Validate();
            company.Password = passwordHasher.HashPassword(company.Email, company.Password);

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
            passwordHasher.VerifyHashedPassword(
                    providedEmail,
                    company.Password,
                    providedPassword)
                != PasswordVerificationResult.Success)
                return null;

            return company;
        }
    }
}