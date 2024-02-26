using Amazon.SimpleEmailV2;

namespace Members.Signup.Tests;

public class Sut : AppFixture<Program>
{
    internal Request SignupRequest { get; set; } = default!;
    internal string? MemberId { get; set; }

    protected override void ConfigureApp(IWebHostBuilder a)
    {
        a.UseContentRoot(Directory.GetCurrentDirectory());
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        s.AddSingleton<IAmazonSimpleEmailServiceV2, SesClient>();
    }

    protected override Task SetupAsync()
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

        return Task.CompletedTask;
    }

    protected override async Task TearDownAsync()
    {
        await DB.DeleteAsync<Dom.Member>(MemberId);
        await DB.DeleteAsync<Dom.JobRecord>(j => j.IsComplete == true);
    }
}