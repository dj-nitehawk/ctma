﻿using Amazon.SimpleEmailV2;

namespace Ctma.Tests;

public class App(IMessageSink s) : AppFixture<Program>(s)
{
    protected override void ConfigureApp(IWebHostBuilder a)
    {
        //only needed when tests are not in a separate project
        a.UseContentRoot(Directory.GetCurrentDirectory());
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        s.AddSingleton<IAmazonSimpleEmailServiceV2, FakeSesClient>();
    }
}