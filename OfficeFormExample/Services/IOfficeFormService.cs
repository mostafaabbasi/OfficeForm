using OfficeFormExample.Models;

namespace OfficeFormExample.Services;

public interface IOfficeFormService
{
    Task<List<OfficeFormInputModel>> SendAsync(IFormFile file, CancellationToken cancellationToken);
}