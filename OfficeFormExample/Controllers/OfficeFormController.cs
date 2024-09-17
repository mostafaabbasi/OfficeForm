using Microsoft.AspNetCore.Mvc;
using OfficeFormExample.Services;

namespace OfficeFormExample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OfficeFormController(IOfficeFormService officeFormService) : ControllerBase
{
    [HttpPost("submit-with-http-client")]
    public async Task SubmitWithHttpClient(IFormFile file)
    {
        await officeFormService.SendAsync(file, HttpContext.RequestAborted);
    }
}