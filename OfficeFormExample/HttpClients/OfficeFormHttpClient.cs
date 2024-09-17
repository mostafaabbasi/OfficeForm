using System.Text.Json;
using Microsoft.AspNetCore.Antiforgery;
using OfficeFormExample.Models;

namespace OfficeFormExample.HttpClients;

public class OfficeFormHttpClient(
    HttpClient httpClient,
    IAntiforgery antiforgery,
    IHttpContextAccessor httpContextAccessor,
    ILogger<OfficeFormHttpClient> logger) : IOfficeFormHttpClient
{
    public async Task SendAsync(
        SubmitFormOfficeModel model,
        string refer,
        string requestUri,
        CancellationToken cancellationToken)
    {
        try
        {
            var anti = antiforgery.GetAndStoreTokens(httpContextAccessor.HttpContext);
        
            httpClient.DefaultRequestHeaders.Add("referer", refer);
            httpClient.DefaultRequestHeaders.Add("__requestverificationtoken", anti.RequestToken);
            httpClient.DefaultRequestHeaders.Add("x-correlationid", Guid.NewGuid().ToString());
            httpClient.DefaultRequestHeaders.Add("x-usersessionid", Guid.NewGuid().ToString());
            httpClient.DefaultRequestHeaders.Add("x-ms-form-muid", Guid.NewGuid().ToString("N").ToUpper());
        
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var serializedModel = JsonSerializer.Serialize(model);
        
            var content = new StringContent(serializedModel, null, "application/json");
        
            request.Content = content;
        
            var response = await httpClient.SendAsync(request, cancellationToken);
        
            if(response.IsSuccessStatusCode)
                logger.LogInformation("data sent , Data:{Data}",serializedModel);
            else
            {
                logger.LogError("failed , Data:{Data}",model);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e,"failed catched, Data:{Data}",JsonSerializer.Serialize(model));
        }
    }
}