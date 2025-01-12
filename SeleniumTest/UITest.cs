using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Skal_vi_videre.Repository;

namespace Skal_vi_videre.SeleniumTest
{
    [TestClass]
    public class UITest
    {
        private static readonly string DriverDirectory = "C:\\Driver";
        private static IWebDriver _driver;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _driver = new ChromeDriver(DriverDirectory);
            _driver.Navigate().GoToUrl("https://skalvividere.nu/");
        }

        [TestMethod]
        public void CreateCompanyTest()
        {
            IWebElement createCompany = _driver.FindElement(By.Id("createCompany"));
            createCompany.Click();

            string title = _driver.Title;
            Assert.AreEqual("Opret Virksomhed", title);

            IWebElement addCVRNumber = _driver.FindElement(By.Id("cvr"));
            addCVRNumber.SendKeys("43341537");
            IWebElement addName = _driver.FindElement(By.Id("name"));
            addName.SendKeys("Saturnvej 65");
            IWebElement addAddress = _driver.FindElement(By.Id("address"));
            addAddress.SendKeys("August Bournonvilles Passage 1");
            IWebElement addEmail = _driver.FindElement(By.Id("email"));
            addEmail.SendKeys("saturnvej@gmail.com");
            IWebElement addPassword = _driver.FindElement(By.Id("password"));
            addPassword.SendKeys("12345678");
            IWebElement addPhoneNumber = _driver.FindElement(By.Id("phoneNumber"));
            addPhoneNumber.SendKeys("32456666");
            IWebElement addDescription = _driver.FindElement(By.Id("description"));
            addDescription.SendKeys("Selenium Test");

            IWebElement addButton = _driver.FindElement(By.Id("create"));
            addButton.Click();

            string titleEvent = _driver.Title;
            Assert.AreEqual("Opret Opret", titleEvent);
        }

        [TestMethod]
        public void CreateEventTest()
        {
            IWebElement createEvent = _driver.FindElement(By.Id("createEvent"));
            createEvent.Click();

            string title = _driver.Title;
            Assert.AreEqual("Opret Event", title);

            IWebElement addTitle = _driver.FindElement(By.Id("title"));
            addTitle.SendKeys("Selenium Test Titel");
            IWebElement addDescription = _driver.FindElement(By.Id("description"));
            addDescription.SendKeys("Selenium Test Beskrivelse");
            IWebElement addGenre = _driver.FindElement(By.Id("genre"));
            SelectElement select = new SelectElement(addGenre);
            select.SelectByText("Andet");
            IWebElement addStartDate = _driver.FindElement(By.Id("startDate"));
            addStartDate.SendKeys("2025-12-12T15:00");
            IWebElement addEndDate = _driver.FindElement(By.Id("endDate"));
            addEndDate.SendKeys("2025-12-15T15:00");
            IWebElement addLocation = _driver.FindElement(By.Id("location"));
            addLocation.SendKeys("Selenium Test");

            IWebElement createButton = _driver.FindElement(By.Id("create"));
            createButton.Click();
        }

        [TestMethod]
        public void LogInTest()
        {
            IWebElement logIn = _driver.FindElement(By.Id("logIn"));
            logIn.Click();

            string title = _driver.Title;
            Assert.AreEqual("Log ind", title);

            IWebElement email = _driver.FindElement(By.Id("email"));
            email.SendKeys("123@gmail.com");
            IWebElement password = _driver.FindElement(By.Id("password"));
            password.SendKeys("123");

            IWebElement logInButton = _driver.FindElement(By.Id("logIn"));
            logInButton.Click();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            EventRepository eventRepository = new EventRepository();
            // Fjern alle oprettede Event-objekter med lokationen "Selenium Test"
            var eventsToDelete = eventRepository.GetAll().Where(l => l.Location == "Selenium Test").ToList();
            foreach (var events in eventsToDelete)
            {
                eventRepository.Delete(events.Id);
            }

            CompanyRepository companyRepository = new CompanyRepository();
            // Fjern alle oprettede Company-objekter med rollen "CompanyTest"
            var companiesToDelete = companyRepository.GetAll().Where(r => r.Description == "Selenium Test").ToList();
            foreach (var company in companiesToDelete)
            {
                companyRepository.Delete(company.Id);
            }
        }
    }
}