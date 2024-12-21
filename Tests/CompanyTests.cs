using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skal_vi_videre.Repository;
using Microsoft.Extensions.Configuration;

namespace Skal_vi_videre.Tests
{
    [TestClass()]
    public class CompanyRepositoryTests
    {
        private static CompanyRepository _companyRepository;

        [TestMethod()]
        public void CreateTest()
        {
            CompanyRepository companyRepository = new CompanyRepository();
            Company company = new Company
            {
                Cvr = "36716967",
                Name = "123 Flyt Aps",
                Address = "Test 2",
                Email = "flyt@gmail.com",
                Password = "12345678",
                PhoneNumber = "23145345",
                Description = "Unit Test",
                Role = "CompanyTest"
            };
            companyRepository.Create(company);
            Assert.AreEqual(company.Name, "123 Flyt Aps");

            Company companyWithoutAps = new Company
            {
                Cvr = "35480013",
                Name = "kmg 32",
                Address = "kmg 32",
                Email = "kmg32@gmail.com",
                Password = "12345678",
                PhoneNumber = "23423333",
                Description = "KMG 32 ApS",
                Role = "CompanyTest"
            };
            companyRepository.Create(companyWithoutAps);
            Assert.AreEqual(companyWithoutAps.Name, "kmg 32");

            Company nullCompany = new Company
            {
                Cvr = "",
                Name = "",
                Address = "",
                Email = "",
                Password = "",
                PhoneNumber = "",
                Description = "",
                Role = ""
            };
            Assert.ThrowsException<ArgumentNullException>(() => companyRepository.Create(nullCompany));
        }

        [TestMethod()]
        public void GetByCvrTest()
        {
            CompanyRepository companyRepository = new CompanyRepository();
            Company? company = companyRepository.GetByCvr("31551315");
            Assert.AreEqual(4, company?.Id);
            Assert.AreEqual("9GConsult", company?.Name);
            Assert.AreEqual(4, company?.Id);

            Company? company1 = companyRepository.GetByCvr("13432345");
            Assert.IsNull(company1);
        }

        [TestMethod()]
        public void VerifyCompanyTest()
        {
            CompanyRepository companyRepository = new CompanyRepository();
            Company? company = companyRepository.VerifyCompany("9g@gmail.com", "12345678");
            Assert.AreEqual(4, company?.Id);
            Assert.AreEqual("9GConsult", company?.Name);

            Company wrongPassword = companyRepository.VerifyCompany("9g@gmail.com", "forkert");
            Assert.IsNull(wrongPassword);
        }

        [TestMethod()]
        public void ValidateCompanyAsyncTest()
        {
            Company wrongCompany = new Company
            {
                Cvr = "12345678",
                Name = "Eksisterer ikke",
                Address = "N/A",
                Email = "na@gmail.com",
                Password = "Eksistererikke23",
                PhoneNumber = "14673134",
                Description = "Virksomheden eksisterer ikke",
                Role = "Company",
            };
            Assert.ThrowsExceptionAsync<ArgumentException>(() => wrongCompany.ValidateCompanyAsync(wrongCompany.Cvr, wrongCompany.Name));

            Company wrongCompanyName = new Company
            {
                Cvr = "40310312",
                Name = "Forkert Navn",
                Address = "N/A",
                Email = "fo@gmail.com",
                Password = "Forkertnavn21",
                PhoneNumber = "34567853",
                Description = "Cvr nummeret er rigtig, men navnet er forker",
                Role = "Company"
            };
            Assert.ThrowsExceptionAsync<ArgumentException>(() => wrongCompanyName.ValidateCompanyAsync(wrongCompanyName.Cvr, wrongCompany.Name));
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            CompanyRepository companyRepository = new CompanyRepository();
            // Fjern alle oprettede Company-objekter med rollen "CompanyTest"
            var companiesToDelete = companyRepository.GetAll().Where(r => r.Role == "CompanyTest").ToList();
            foreach (var company in companiesToDelete)
            {
                companyRepository.Delete(company.Id);
            }
        }
    }
}