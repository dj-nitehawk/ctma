namespace Ctma;

public class App(IMessageSink s) : AppFixture<Program>(s)
{
    protected override void ConfigureApp(IWebHostBuilder a)
    {
        //only needed when tests are not in a separate project
        a.UseContentRoot(Directory.GetCurrentDirectory());
    }

    protected override void ConfigureServices(IServiceCollection s) { }

    protected override Task SetupAsync()
        => Task.CompletedTask;

    protected override Task TearDownAsync()
        => Task.CompletedTask;
}