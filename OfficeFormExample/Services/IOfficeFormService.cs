namespace OfficeFormExample.Services;

public interface IOfficeFormService
{
    Task SendAsync(IFormFile file, CancellationToken cancellationToken);
}