using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skal_vi_videre.Components.Pages;
using Skal_vi_videre.Repository;

namespace Skal_vi_videre.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private EventRepository _eventRepository;

        public EventController(EventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: api/<ModelsController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<List<Event>> Get()
        {
            // Hent alle events fra databasen uden at inkludere Company-data
            var events = _eventRepository.GetAll();

            // Filtrer events, så de kun vises hvis:
            // - EndDate er i fremtiden (ikke startet endnu eller stadig igang)
            // - Eller hvis EndDate er null, men StartDate er i fremtiden
            events = events.Where(e =>
                (e.EndDate != null && e.EndDate >= DateTime.Now) ||  // EndDate i fremtiden
                (e.EndDate == null && e.StartDate > DateTime.Now)    // StartDate i fremtiden og EndDate mangler
            ).ToList();

            // Opret en liste af events med de nødvendige data
            var eventsList = events.Select(e => new Event
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Genre = e.Genre,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Location = e.Location,
                CompanyId = e.CompanyId
            }).ToList();

            return Ok(eventsList);
        }
    }
}