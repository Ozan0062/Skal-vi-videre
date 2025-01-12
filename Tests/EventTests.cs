using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skal_vi_videre.Repository;

namespace Skal_vi_videre.Tests
{
    [TestClass()]
    public class EventTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            EventRepository eventRepository = new EventRepository();
            Event events = new Event
            {
                Title = "Test",
                Description = "Test",
                Genre = "Test",
                StartDate = new System.DateTime(2025, 12, 12, 15, 00, 00),
                EndDate = new System.DateTime(2025, 12, 15, 15, 00, 00),
                Location = "Test",
                CompanyId = 4
            };
            eventRepository.Create(events);
            Assert.AreEqual(events.Title, "Test");
        }

        [TestMethod()]
        public void ValidateTest()
        {
            Event nullEvent = new Event
            {
                Title = "",
                Description = "",
                Genre = "",
                StartDate = new System.DateTime(2025, 12, 12, 15, 00, 00),
                EndDate = new System.DateTime(2025, 12, 15, 15, 00, 00),
                Location = "",
                CompanyId = 4
            };
            Assert.ThrowsException<ArgumentNullException>(() => nullEvent.Validate());

            Event wrongDate = new Event
            {
                Title = "Test",
                Description = "Test",
                Genre = "Test",
                StartDate = new System.DateTime(2025, 12, 12, 15, 00, 00),
                EndDate = new System.DateTime(2025, 12, 11, 15, 00, 00),
                Location = "Test",
                CompanyId = 4
            };
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => wrongDate.Validate());

            Event wrongDate1 = new Event
            {
                Title = "Test",
                Description = "Test",
                Genre = "Test",
                StartDate = new System.DateTime(2024, 11, 12, 15, 00, 00),
                EndDate = new System.DateTime(2024, 12, 15, 15, 00, 00),
                Location = "Test",
                CompanyId = 4
            };
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => wrongDate1.Validate());
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            EventRepository eventRepository = new EventRepository();
            // Fjern den oprettede Event fra databasen efter testen
            var eventsToDelete = eventRepository.GetAll().Where(l => l.Location == "Test").ToList();
            foreach (var events in eventsToDelete)
            {
                eventRepository.Delete(events.Id);
            }
        }
    }
}