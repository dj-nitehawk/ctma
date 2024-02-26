using System.Net;
using Amazon.SimpleEmailV2;
using Ctma;
using Dom;
using MongoDB.Bson;

namespace Members.Signup.Tests;

public class Cases(Sut App) : TestBase<Sut>
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
        var (rsp, res) = await App.Client.POSTAsync<Endpoint, Request, Response>(App.SignupRequest);

        rsp.IsSuccessStatusCode.Should().BeTrue();
        ObjectId.TryParse(res.MemberId, out _).Should().BeTrue();
        App.MemberId = res.MemberId;
        res.MemberNumber.Should().BeOfType(typeof(ulong)).And.BeGreaterThan(0);

        var actual = await DB.Find<Member>()
                             .MatchID(App.MemberId)
                             .ExecuteSingleAsync();

        var expected = new Member
        {
            Address = new()
            {
                City = App.SignupRequest.Address.City,
                District = App.SignupRequest.Address.District,
                PostalCode = App.SignupRequest.Address.PostalCode,
                Street = App.SignupRequest.Address.Street
            },
            BirthDay = DateOnly.Parse(App.SignupRequest.BirthDay),
            Collaborate = App.SignupRequest.Collaborate,
            CurrentWork = App.SignupRequest.CurrentWork,
            Designation = App.SignupRequest.Designation.TitleCase(),
            Email = App.SignupRequest.Email.LowerCase(),
            FirstName = App.SignupRequest.UserName.FirstName,
            Gender = App.SignupRequest.Gender,
            ID = App.MemberId,
            LastName = App.SignupRequest.UserName.LastName.TitleCase(),
            MemberNumber = res.MemberNumber,
            SignupDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Nic = App.SignupRequest.Nic.UpperCase(),
            Slmc = App.SignupRequest.Slmc,
            MobileNumber = App.SignupRequest.Contact.MobileNumber,
            Whatsapp = App.SignupRequest.Contact.Whatsapp,
            Telegram = App.SignupRequest.Contact.Telegram,
            Qualifications = App.SignupRequest.Qualifications
        };

        actual.Should().BeEquivalentTo(expected);

        var fakeSesClient = (SesClient)App.Services.GetRequiredService<IAmazonSimpleEmailServiceV2>();
        (await fakeSesClient.EmailReceived(App.MemberId)).Should().BeTrue();
    }

    [Fact, Priority(2)]
    public async Task Duplicate_Info_Validation()
    {
        var (rsp, res) = await App.Client.POSTAsync<Endpoint, Request, ProblemDetails>(App.SignupRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errKeys = res.Errors.Select(e => e.Name).ToList();
        errKeys.Should().BeEquivalentTo(
            "email",
            "nic",
            "slmc",
            "contact.MobileNumber");
    }
}