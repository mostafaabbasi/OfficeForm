using Microsoft.AspNetCore.Mvc;
using OfficeFormExample.Extensions;
using OfficeFormExample.Services;

namespace OfficeFormExample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OfficeFormController(IOfficeFormService officeFormService) : ControllerBase
{
    [HttpPost("submit-with-http-client")]
    public async Task<IActionResult> SubmitWithHttpClient(IFormFile file)
    {
        var result = await officeFormService.SendAsync(file, HttpContext.RequestAborted);
        return await result.ExportExcel("WaverCodes",$"Result-{DateTime.Now}",HttpContext.RequestAborted);
    }
}