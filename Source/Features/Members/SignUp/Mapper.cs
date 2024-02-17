using Ctma;
using Dom;

namespace Members.Signup;

sealed class Mapper : RequestMapper<Request, Member>
{
    public override Member ToEntity(Request r)
        => new()
        {
            FirstName = r.UserName.FirstName.TitleCase(),
            LastName = r.UserName.LastName.TitleCase(),
            Designation = r.Designation.TitleCase(),
            CurrentWork = r.CurrentWork,
            Email = r.Email.LowerCase(),
            BirthDay = DateOnly.Parse(r.BirthDay),
            Gender = r.Gender.TitleCase(),
            Nic = r.Nic.UpperCase(),
            Slmc = r.Slmc.Trim(),
            MobileNumber = r.Contact.MobileNumber.Trim(),
            Whatsapp = r.Contact.Whatsapp,
            Viber = r.Contact.Viber,
            Telegram = r.Contact.Telegram,
            Address = new()
            {
                City = r.Address.City.TitleCase(),
                District = r.Address.District.TitleCase(),
                PostalCode = r.Address.PostalCode.Trim(),
                Street = r.Address.Street.Trim()
            },
            Qualifications = r.Qualifications.Trim(),
            Collaborate = r.Collaborate.Trim()
        };
}