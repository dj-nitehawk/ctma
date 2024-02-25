using System.Net;
using Ctma;
using Ctma.Tests;
using Dom;
using MongoDB.Bson;

namespace Members.Signup.Tests;

public class Tests(App a, State s, ITestOutputHelper o) : TestClass<App, State>(a, s, o)
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

        var (rsp, res) = await App.Client.POSTAsync<Endpoint, Request, ProblemDetails>(req);

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
        var (rsp, res) = await App.Client.POSTAsync<Endpoint, Request, Response>(State.SignupRequest);

        rsp.IsSuccessStatusCode.Should().BeTrue();
        ObjectId.TryParse(res.MemberId, out _).Should().BeTrue();
        State.MemberId = res.MemberId;
        res.MemberNumber.Should().BeOfType(typeof(ulong)).And.BeGreaterThan(0);

        var actual = await DB.Find<Member>()
                             .MatchID(State.MemberId)
                             .ExecuteSingleAsync();

        var expected = new Member
        {
            Address = new()
            {
                City = State.SignupRequest.Address.City,
                District = State.SignupRequest.Address.District,
                PostalCode = State.SignupRequest.Address.PostalCode,
                Street = State.SignupRequest.Address.Street
            },
            BirthDay = DateOnly.Parse(State.SignupRequest.BirthDay),
            Collaborate = State.SignupRequest.Collaborate,
            CurrentWork = State.SignupRequest.CurrentWork,
            Designation = State.SignupRequest.Designation.TitleCase(),
            Email = State.SignupRequest.Email.LowerCase(),
            FirstName = State.SignupRequest.UserName.FirstName,
            Gender = State.SignupRequest.Gender,
            ID = State.MemberId,
            LastName = State.SignupRequest.UserName.LastName.TitleCase(),
            MemberNumber = res.MemberNumber,
            SignupDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Nic = State.SignupRequest.Nic.UpperCase(),
            Slmc = State.SignupRequest.Slmc,
            MobileNumber = State.SignupRequest.Contact.MobileNumber,
            Whatsapp = State.SignupRequest.Contact.Whatsapp,
            Telegram = State.SignupRequest.Contact.Telegram,
            Qualifications = State.SignupRequest.Qualifications
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact, Priority(2)]
    public async Task Duplicate_Info_Detection()
    {
        var (rsp, res) = await App.Client.POSTAsync<Endpoint, Request, ProblemDetails>(State.SignupRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errKeys = res.Errors.Select(e => e.Name).ToList();
        errKeys.Should().BeEquivalentTo(
            "email",
            "nic",
            "slmc",
            "contact.MobileNumber");
    }
}