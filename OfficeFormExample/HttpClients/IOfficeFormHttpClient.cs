using OfficeFormExample.Models;

namespace OfficeFormExample.HttpClients;

public interface IOfficeFormHttpClient
{
    Task SendAsync(SubmitFormOfficeModel model, string requestUri, CancellationToken cancellationToken);
}