using Ctma;
using Members.Signup;
using MongoDB.Bson;
using MongoDB.Entities;

namespace Tests.Members.SignUp;

public class Tests(App f, MemberFixture mem, ITestOutputHelper o) : TestClass<App>(f, o), IClassFixture<MemberFixture>
{
    [Fact]
    public async Task Invalid_User_Input()
    {
        var req = new Request
        {
            UserName = new()
            {
                FirstName = "aa",
                LastName = "bb"
            },
            Email = "badmail.cc",
            BirthDay = "2020-10-10",
            Gender = "nada",
            Nic = "76876",
            Slmc = "123456",
            Contact = new() { MobileNumber = "12345" },
            Address = new()
            {
                City = "c",
                Street = "s"
            },
            Collaborate = "abcd"
        };

        var (rsp, res) = await Fx.Client.POSTAsync<Endpoint, Request, ProblemDetails>(req);

        rsp.IsSuccessStatusCode.Should().BeFalse();

        var errKeys = res.Errors.Select(e => e.Name).ToList();
        errKeys.Should().BeEquivalentTo(
            "userName.FirstName",
            "userName.LastName",
            "designation",
            "currentWork",
            "email",
            "birthDay",
            "gender",
            "nic",
            "slmc",
            "contact.MobileNumber",
            "address.District",
            "address.PostalCode",
            "collaborate",
            "terms");
    }

    [Fact, Priority(1)]
    public async Task Successful_Member_Creation()
    {
        var (rsp, res) = await Fx.Client.POSTAsync<Endpoint, Request, Response>(mem.SignupRequest);

        rsp.IsSuccessStatusCode.Should().BeTrue();
        ObjectId.TryParse(res.MemberId, out _).Should().BeTrue();
        mem.MemberId = res.MemberId;
        res.MemberNumber.Should().BeOfType(typeof(ulong)).And.BeGreaterThan(0);

        var actual = await DB.Find<Dom.Member>()
                             .MatchID(mem.MemberId)
                             .ExecuteSingleAsync();

        var expected = new Dom.Member
        {
            Address = new()
            {
                City = mem.SignupRequest.Address.City,
                District = mem.SignupRequest.Address.District,
                PostalCode = mem.SignupRequest.Address.PostalCode,
                Street = mem.SignupRequest.Address.Street
            },
            BirthDay = DateOnly.Parse(mem.SignupRequest.BirthDay),
            Collaborate = mem.SignupRequest.Collaborate,
            CurrentWork = mem.SignupRequest.CurrentWork,
            Designation = mem.SignupRequest.Designation.TitleCase(),
            Email = mem.SignupRequest.Email.LowerCase(),
            FirstName = mem.SignupRequest.UserName.FirstName,
            Gender = mem.SignupRequest.Gender,
            ID = mem.MemberId,
            LastName = mem.SignupRequest.UserName.LastName.TitleCase(),
            MemberNumber = res.MemberNumber,
            SignupDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Nic = mem.SignupRequest.Nic.UpperCase(),
            Slmc = mem.SignupRequest.Slmc,
            MobileNumber = mem.SignupRequest.Contact.MobileNumber,
            Whatsapp = mem.SignupRequest.Contact.Whatsapp,
            Telegram = mem.SignupRequest.Contact.Telegram,
            Qualifications = mem.SignupRequest.Qualifications
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact, Priority(2)]
    public async Task Duplicate_Info_Detection()
    {
        var (rsp, res) = await Fx.Client.POSTAsync<Endpoint, Request, ProblemDetails>(mem.SignupRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errKeys = res.Errors.Select(e => e.Name).ToList();
        errKeys.Should().BeEquivalentTo(
            "email",
            "nic",
            "slmc",
            "contact.MobileNumber");
    }
}