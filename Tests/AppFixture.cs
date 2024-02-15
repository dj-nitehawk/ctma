namespace Tests;

public class AppFixture(IMessageSink s) : TestFixture<Program>(s)
{
    protected override Task SetupAsync()
        => Task.CompletedTask;

    protected override void ConfigureServices(IServiceCollection s) { }

    protected override Task TearDownAsync()
        => Task.CompletedTask;
}