using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(_eventRepository.GetAll());
        }
    }
}