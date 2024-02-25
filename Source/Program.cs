using Amazon;
using Amazon.SimpleEmailV2;
using Ctma;
using Dom;
using FastEndpoints.Security;

var bld = WebApplication.CreateBuilder(args);
bld.Services
   .AddJWTBearerAuth(bld.Configuration["Auth:SigningKey"]!)
   .AddAuthorization()
   .AddFastEndpoints()
   .AddJobQueues<JobRecord, JobStorageProvider>()
   .AddSingleton<IAmazonSimpleEmailServiceV2>(
       new AmazonSimpleEmailServiceV2Client(
           awsAccessKeyId: bld.Configuration["Email:ApiKey"],
           awsSecretAccessKey: bld.Configuration["Email:ApiSecret"],
           region: RegionEndpoint.USEast1));

if (!bld.Environment.IsProduction())
{
    bld.Services.SwaggerDocument(
        d => d.DocumentSettings =
                 s =>
                 {
                     s.DocumentName = "v0";
                     s.Version = "0.0.0";
                 });
}

var app = bld.Build();
app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints(c => c.Errors.UseProblemDetails());

if (!app.Environment.IsProduction())
    app.UseSwaggerGen(uiConfig: u => u.DeActivateTryItOut());

await InitDatabase(app.Configuration);

app.UseJobQueues(
    o =>
    {
        o.MaxConcurrency = 4;
        o.ExecutionTimeLimit = TimeSpan.FromSeconds(20);
    });

app.Run();

async Task InitDatabase(IConfiguration config)
{
    await DB.InitAsync(config["Database:Name"]!);
    await DB.MigrateAsync();
    await Notification.Initialize();
}

public partial class Program;