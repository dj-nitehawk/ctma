using Ctma;
using FluentValidation;

namespace Members.Signup;

sealed class Request
{
    public User UserName { get; set; }
    public string Designation { get; set; }
    public string CurrentWork { get; set; }
    public string Email { get; set; }
    public string BirthDay { get; set; }
    public string Gender { get; set; }
    public string Nic { get; set; }
    public string Slmc { get; set; }
    public ContactDetails Contact { get; set; }
    public AddressDetails Address { get; set; }
    public string Qualifications { get; set; }
    public string Collaborate { get; set; }
    public bool Terms { get; set; }

    public sealed class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public sealed class ContactDetails
    {
        public string MobileNumber { get; set; }
        public bool Whatsapp { get; set; }
        public bool Viber { get; set; }
        public bool Telegram { get; set; }
    }

    public sealed class AddressDetails
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
    }

    sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.UserName.FirstName).Length(3, 100).WithMessage("Invalid First Name!");
            RuleFor(x => x.UserName.LastName).Length(3, 100).WithMessage("Invalid Last Name!");
            RuleFor(x => x.Designation).NotEmpty().WithMessage("Designation is required!");
            RuleFor(x => x.CurrentWork).NotEmpty().WithMessage("Current work is required!");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email address is required!");
            RuleFor(x => x.BirthDay).Must(bDay => bDay.IsAValidBirthDay()).WithMessage("Birthday is required!");
            RuleFor(x => x.Gender).Must(gender => gender.IsAValidGender()).WithMessage("Gender is required!");
            RuleFor(x => x.Nic).Must(nic => nic.IsAValidNic()).WithMessage("Invalid NIC!");
            RuleFor(x => x.Slmc).Must(slmc => slmc.IsAValidSlmcNo()).WithMessage("Invalid SLMC number!");
            RuleFor(x => x.Contact.MobileNumber).Must(mob => mob.IsAValidMobileNumber()).WithMessage("Invalid mobile number!");
            RuleFor(x => x.Address.Street).NotEmpty().WithMessage("Street is required!");
            RuleFor(x => x.Address.City).NotEmpty().WithMessage("City is required!");
            RuleFor(x => x.Address.District).NotEmpty().WithMessage("District is required!");
            RuleFor(x => x.Address.PostalCode).NotEmpty().WithMessage("Postal code is required!");
            RuleFor(x => x.Collaborate).MinimumLength(5).WithMessage("Intended contribution is required!");
            RuleFor(x => x.Terms).Must(v => v).WithMessage("Accepting terms is required!");
        }
    }
}