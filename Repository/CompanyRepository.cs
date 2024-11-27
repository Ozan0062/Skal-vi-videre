using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skal_vi_videre.Repository.Base;

namespace Skal_vi_videre.Repository
{
    public class CompanyRepository : BaseRepository<Company> //Det er en repository for Company
    {
        private PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();

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
    }
}
