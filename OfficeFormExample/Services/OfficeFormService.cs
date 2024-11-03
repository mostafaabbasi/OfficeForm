using OfficeFormExample.Extensions;
using OfficeFormExample.HttpClients;
using OfficeFormExample.Models;

namespace OfficeFormExample.Services;

public class OfficeFormService(IOfficeFormHttpClient officeFormHttpClient) : IOfficeFormService
{
    public async Task<List<OfficeFormInputModel>> SendAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var list = file.GetList<OfficeFormInputModel>();

        if (!list.Any())
            throw new Exception("There is no data");

        list = list.Where(x => x.IsSent == false).ToList();

        var listResult = new List<OfficeFormInputModel>();

        const int batchSize = 10;

        for (int i = 0; i <= list.Count; i += batchSize)
        {
            var batch = list.Skip(i).Take(batchSize);

            var tasks = batch.Select(item => officeFormHttpClient.SendAsync(item, cancellationToken));
            var batchResults = await Task.WhenAll(tasks);

            listResult.AddRange(batchResults);
        }

        return listResult;
    }
}