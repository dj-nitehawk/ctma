var bld = WebApplication.CreateBuilder(args);
bld.Services
   .AddFastEndpoints()
   .SwaggerDocument();

var app = bld.Build();
app.UseFastEndpoints(c=>c.Errors.UseProblemDetails())
   .UseSwaggerGen();
app.Run();

public partial class Program;