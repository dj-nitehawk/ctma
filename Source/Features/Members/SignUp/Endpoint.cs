namespace Members.Signup;

sealed class Endpoint : EndpointWithMapper<Request, Mapper>
{
    public override void Configure()
    {
        Post("members/signup");
        PreProcessor<DuplicateInfoChecker>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        await SendAsync("ok");
    }
}