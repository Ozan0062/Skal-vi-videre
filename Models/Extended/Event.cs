using Skal_vi_videre.Repository.Base;

namespace Skal_vi_videre
{
    public partial class Event : IHasId, IUpdateFromOther<Event>
    {
        public void Update(Event tOther)
        {
            Title = tOther.Title;
            Description = tOther.Description;
            Genre = tOther.Genre;
            StartDate = tOther.StartDate;
            EndDate = tOther.EndDate;
            Location = tOther.Location;
            CompanyId = tOther.CompanyId;
        }
    }
}
