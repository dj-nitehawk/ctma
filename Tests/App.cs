namespace Tests;

public class App(IMessageSink s) : AppFixture<Program>(s)
{
    protected override Task SetupAsync()
        => Task.CompletedTask;

    protected override void ConfigureServices(IServiceCollection s) { }

    protected override Task TearDownAsync()
        => Task.CompletedTask;
}