using OfficeFormExample.Models;

namespace OfficeFormExample.HttpClients;

public interface IOfficeFormHttpClient
{
    Task<OfficeFormInputModel> SendAsync(
        OfficeFormInputModel inputModel,
        CancellationToken cancellationToken);
}