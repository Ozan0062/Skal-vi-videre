using Microsoft.EntityFrameworkCore;
using Skal_vi_videre.Repository.Base;

namespace Skal_vi_videre.Repository
{
    public class EventRepository : BaseRepository<Event>
    {
        public static DBContext? DbContext { get; set; }

        protected override IQueryable<Event> GetAllWithIncludes(DbContext context)
        {
            return context.Set<Event>()
                          .Include(t => t.Company); // Inkluderer Company-data
        }
    }
}