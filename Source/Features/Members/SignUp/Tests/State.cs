namespace Members.Signup.Tests;

public class State : StateFixture
{
    internal Request SignupRequest { get; init; }
    internal string? MemberId { get; set; }

    public State()
    {
        SignupRequest = new()
        {
            UserName = new()
            {
                FirstName = Fake.Name.FirstName(),
                LastName = Fake.Name.LastName()
            },
            Address = new()
            {
                City = Fake.Address.City(),
                District = Fake.Address.State(),
                PostalCode = Fake.Address.ZipCode("#####"),
                Street = Fake.Address.StreetAddress()
            },
            BirthDay = "1983-10-10",
            Collaborate = Fake.Lorem.Text(),
            Contact = new()
            {
                MobileNumber = Fake.Phone.PhoneNumber("##########"),
                Telegram = true,
                Whatsapp = true
            },
            CurrentWork = Fake.Company.CompanyName(),
            Designation = Fake.Hacker.Phrase(),
            Email = Fake.Internet.Email(),
            Gender = "Male",
            Nic = "123456789v",
            Slmc = Fake.Phone.PhoneNumber("#####"),
            Qualifications = Fake.Lorem.Sentence(),
            Terms = true
        };
    }

    protected override async Task TearDownAsync()
    {
        await DB.DeleteAsync<Dom.Member>(MemberId);
    }
}