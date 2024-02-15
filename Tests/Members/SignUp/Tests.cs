using Members.Signup;

namespace Tests.Members.SignUp;

public class Tests(AppFixture f, ITestOutputHelper o) : TestClass<AppFixture>(f, o)
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
}