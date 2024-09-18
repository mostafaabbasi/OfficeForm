using System.Text.Json;
using Microsoft.Extensions.Options;
using OfficeFormExample.Enums;
using OfficeFormExample.Extensions;
using OfficeFormExample.HttpClients;
using OfficeFormExample.Models;

namespace OfficeFormExample.Services;

public class OfficeFormService(IOfficeFormHttpClient officeFormHttpClient, IOptions<AppSettingModel> options) : IOfficeFormService
{
    private readonly AppSettingModel appSetting = options.Value;
    public async Task SendAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var list = file.GetList<OfficeFormInputModel>();
        
        if (!list.Any())
            throw new Exception("There is no data");

        var refundRequests = list.Where(w => w is { IsSent: false, RequestType: RequestTypeEnum.Refund }).ToList();
        var reIssueRequests = list.Where(w => w is { IsSent: false, RequestType: RequestTypeEnum.ReIssue }).ToList();

        List<Task> tasks = new List<Task>();

        var commonData = appSetting.CommonData;
        
        var refundAppSettingModel = appSetting.RefundOfficeForm;
        foreach (var refundRequest in refundRequests)
        {
            var model = new SubmitFormOfficeModel()
            {
                startDate = DateTime.UtcNow,
                submitDate = DateTime.UtcNow.AddSeconds(10),
                answers = JsonSerializer.Serialize(new []
                {
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.Name, answer1 = commonData.Name },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.Email, answer1 = commonData.Email },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.Phone, answer1 = commonData.Phone },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.AgencyName, answer1 = commonData.AgencyName },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.IATANumber, answer1 = commonData.IATANumber },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.EmailReceiver, answer1 = commonData.EmailReceiver },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.PNR, answer1 = refundRequest.PNR },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.TravelDate, answer1 = refundRequest.TravelDate },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.TicketStock, answer1 = refundRequest.TicketStock },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.FlightNumber, answer1 = refundRequest.FlightNumber },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.VKAReasonCodeREF, answer1 = commonData.VKAReasonCodeREF },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.AuditRemark, answer1 = commonData.VKAReasonCodeREF },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.FirstName, answer1 = refundRequest.FirstName },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.LastName, answer1 = refundRequest.LastName },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.TicketNumber, answer1 = refundRequest.TicketNumber}
                })
            };

            var task = officeFormHttpClient.SendAsync(model, refundAppSettingModel.Refer, refundAppSettingModel.RequestUri,
                cancellationToken);
            
            tasks.Add(task);
        }
        
        var reIssueAppSettingModel = appSetting.ReIssueOfficeForm;
        foreach (var reIssueRequest in reIssueRequests)
        {
            var model = new SubmitFormOfficeModel()
            {
                startDate = DateTime.UtcNow,
                submitDate = DateTime.UtcNow.AddSeconds(10),
                answers = JsonSerializer.Serialize(new []
                {
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.Name, answer1 = commonData.Name },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.Email, answer1 = commonData.Email },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.Phone, answer1 = commonData.Phone },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.AgencyName, answer1 = commonData.AgencyName },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.IATANumber, answer1 = commonData.IATANumber },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.EmailReceiver, answer1 = commonData.EmailReceiver },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.PNR, answer1 = reIssueRequest.PNR },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.TravelDate, answer1 = reIssueRequest.TravelDate },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.TicketStock, answer1 = reIssueRequest.TicketStock },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.FlightNumber, answer1 = reIssueRequest.FlightNumber },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.VKAReasonCodeSAL, answer1 = commonData.VKAReasonCodeSAL },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.VKAReasonCodeEXC, answer1 = commonData.VKAReasonCodeEXC },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.AuditRemark, answer1 = reIssueRequest.TicketNumber },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.FirstName, answer1 = reIssueRequest.FirstName },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.LastName, answer1 = reIssueRequest.LastName }
                })
            };
            
            var task = officeFormHttpClient.SendAsync(model, reIssueAppSettingModel.Refer, reIssueAppSettingModel.RequestUri,
                cancellationToken);
            
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }
}