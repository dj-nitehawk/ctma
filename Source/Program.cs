var bld = WebApplication.CreateBuilder(args);
bld.Services
   .AddFastEndpoints()
   .SwaggerDocument(
       d => d.DocumentSettings =
                s =>
                {
                    s.DocumentName = "v0";
                    s.Version = "0.0.0";
                });

var app = bld.Build();
app.UseFastEndpoints(c => c.Errors.UseProblemDetails())
   .UseSwaggerGen(uiConfig: u => u.DeActivateTryItOut());

await DB.InitAsync(app.Configuration["Database:Name"]!);

app.Run();

public partial class Program;