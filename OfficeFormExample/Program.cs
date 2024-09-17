using OfficeFormExample.HttpClients;
using OfficeFormExample.Models;
using OfficeFormExample.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<AppSettingModel>(configuration);
services.AddControllers();
services.AddAntiforgery();
services.AddHttpContextAccessor();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddHttpClient<IOfficeFormHttpClient, OfficeFormHttpClient>().ConfigureHttpClient(act =>
{
    act.DefaultRequestHeaders.Add("x-ms-form-request-ring", "business");
    act.DefaultRequestHeaders.Add("x-ms-form-request-source", "ms-formweb");
    
});
services.AddScoped<IOfficeFormService, OfficeFormService>();
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection()
    .UseRouting()
    .UseEndpoints(act => { act.MapControllers(); });

await app.RunAsync();