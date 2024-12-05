using Skal_vi_videre.Repository.Base;
using System.Data;
using System.Net;

namespace Skal_vi_videre
{
    public partial class Event : IHasId, IUpdateFromOther<Event>
    {
        public void Update(Event tOther)
        {
            Validate();
            Title = tOther.Title;
            Description = tOther.Description;
            Genre = tOther.Genre;
            StartDate = tOther.StartDate;
            EndDate = tOther.EndDate;
            Location = tOther.Location;
            CompanyId = tOther.CompanyId;
        }

        public void ValidateData()
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(Genre) || string.IsNullOrEmpty(Location))
            {
                throw new ArgumentNullException("Det er null eller tom");
            }
        }

        public void ValidateDate()
        {
            if (StartDate > EndDate)
            {
                throw new ArgumentOutOfRangeException("Startdatoen er senere end slutdatoen");
            }
            else if (StartDate < DateTime.Now || EndDate < DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("Startdatoen eller Slutdatoen er i fortiden");
            }
        }

        public void Validate()
        {
            ValidateData();
            ValidateDate();
        }
    }
}