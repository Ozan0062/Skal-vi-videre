using Skal_vi_videre.Repository.Base;

namespace Skal_vi_videre
{
    public partial class Company : IHasId, IUpdateFromOther<Company>
    {
        public void Update(Company tOther)
        {
            Cvr = tOther.Cvr;
            Name = tOther.Name;
            Address = tOther.Address;
            Email = tOther.Email;
            Password = tOther.Password;
            PhoneNumber = tOther.PhoneNumber;
            Role = tOther.Role;
            Description = tOther.Description;
        }
    }
}