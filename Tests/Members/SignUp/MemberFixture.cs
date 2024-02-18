using Members.Signup;
using MongoDB.Entities;

namespace Tests.Members.SignUp;

public class MemberFixture : IAsyncLifetime
{
    internal Request SignupRequest { get; init; }
    internal string? MemberId { get; set; }

    readonly Faker _fake = new();

    public MemberFixture()
    {
        SignupRequest = new()
        {
            UserName = new()
            {
                FirstName = _fake.Name.FirstName(),
                LastName = _fake.Name.LastName()
            },
            Address = new()
            {
                City = _fake.Address.City(),
                District = _fake.Address.State(),
                PostalCode = _fake.Address.ZipCode("#####"),
                Street = _fake.Address.StreetAddress()
            },
            BirthDay = "1983-10-10",
            Collaborate = _fake.Lorem.Text(),
            Contact = new()
            {
                MobileNumber = _fake.Phone.PhoneNumber("##########"),
                Telegram = true,
                Whatsapp = true
            },
            CurrentWork = _fake.Company.CompanyName(),
            Designation = _fake.Hacker.Phrase(),
            Email = _fake.Internet.Email(),
            Gender = "Male",
            Nic = "123456789v",
            Slmc = _fake.Phone.PhoneNumber("#####"),
            Qualifications = _fake.Lorem.Sentence(),
            Terms = true
        };
    }

    public Task InitializeAsync()
        => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await DB.DeleteAsync<Dom.Member>(MemberId);
    }
}