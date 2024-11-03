using System.Text.Json;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Options;
using OfficeFormExample.Enums;
using OfficeFormExample.Models;

namespace OfficeFormExample.HttpClients;

public class OfficeFormHttpClient(
    IOptions<AppSettingModel> options,
    HttpClient httpClient,
    IAntiforgery antiforgery,
    IHttpContextAccessor httpContextAccessor,
    ILogger<OfficeFormHttpClient> logger) : IOfficeFormHttpClient
{
    private readonly CommonDataModel commonData = options.Value.CommonData;
    private readonly RefundOfficeForm refundAppSettingModel = options.Value.RefundOfficeForm;
    private readonly ReIssueOfficeForm reIssueAppSettingModel = options.Value.ReIssueOfficeForm;


    public async Task<OfficeFormInputModel> SendAsync(
        OfficeFormInputModel inputModel,
        CancellationToken cancellationToken)
    {
        var model = ReturnSubmitModel(inputModel);

        var serializedModel = JsonSerializer.Serialize(model);

        try
        {
            var content = new StringContent(serializedModel, null, "application/json");

            var requestUri = ReturnRequestUri(inputModel);

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            request.Content = content;

            var anti = antiforgery.GetAndStoreTokens(httpContextAccessor.HttpContext);

            httpClient.DefaultRequestHeaders.Add("__requestverificationtoken", anti.RequestToken);
            httpClient.DefaultRequestHeaders.Add("x-correlationid", Guid.NewGuid().ToString());
            httpClient.DefaultRequestHeaders.Add("x-usersessionid", Guid.NewGuid().ToString());
            httpClient.DefaultRequestHeaders.Add("x-ms-form-muid", Guid.NewGuid().ToString("N").ToUpper());

            var response = await httpClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("data sent , Data:{Data}", serializedModel);
                inputModel.IsSent = true;
            }
            else
            {
                logger.LogError("failed , Data:{Data}", serializedModel);
                inputModel.IsSent = false;
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "failed catched, Data:{Data}", serializedModel);
            inputModel.IsSent = false;
        }

        return inputModel;
    }

    private SubmitFormOfficeModel ReturnSubmitModel(OfficeFormInputModel input)
    {
        return input.RequestType.Equals(RequestTypeEnum.Refund) ?
                ReturnRefundModel(input) :
                ReturnReIssueModel(input);
    }

    private SubmitFormOfficeModel ReturnRefundModel(OfficeFormInputModel input)
    {
        return new SubmitFormOfficeModel()
        {
            startDate = DateTime.UtcNow,
            submitDate = DateTime.UtcNow.AddSeconds(10),
            answers = JsonSerializer.Serialize(new[]
                {
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.Name, answer1 = commonData.Name },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.Email, answer1 = commonData.Email },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.Phone, answer1 = commonData.Phone },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.AgencyName, answer1 = commonData.AgencyName },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.IATANumber, answer1 = commonData.IATANumber },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.EmailReceiver, answer1 = commonData.EmailReceiver },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.PNR, answer1 = input.PNR },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.TravelDate, answer1 = input.TravelDate },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.TicketStock, answer1 = input.TicketStock },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.FlightNumber, answer1 = input.FlightNumber },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.VKAReasonCodeREF, answer1 = commonData.VKAReasonCodeREF },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.AuditRemark, answer1 = commonData.VKAReasonCodeREF },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.FirstName, answer1 = input.FirstName },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.LastName, answer1 = input.LastName },
                    new AnswerModel { questionId = refundAppSettingModel.QuestionModel.TicketNumber, answer1 = input.TicketNumber}
                })
        };
    }
    private SubmitFormOfficeModel ReturnReIssueModel(OfficeFormInputModel input)
    {
        return new SubmitFormOfficeModel()
        {
            startDate = DateTime.UtcNow,
            submitDate = DateTime.UtcNow.AddSeconds(10),
            answers = JsonSerializer.Serialize(new[]
                {
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.Name, answer1 = commonData.Name },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.Email, answer1 = commonData.Email },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.Phone, answer1 = commonData.Phone },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.AgencyName, answer1 = commonData.AgencyName },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.IATANumber, answer1 = commonData.IATANumber },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.EmailReceiver, answer1 = commonData.EmailReceiver },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.PNR, answer1 = input.PNR },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.TravelDate, answer1 = input.TravelDate },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.TicketStock, answer1 = input.TicketStock },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.FlightNumber, answer1 = input.FlightNumber },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.VKAReasonCodeSAL, answer1 = commonData.VKAReasonCodeSAL },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.VKAReasonCodeEXC, answer1 = commonData.VKAReasonCodeEXC },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.AuditRemark, answer1 = input.TicketNumber },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.FirstName, answer1 = input.FirstName },
                    new AnswerModel { questionId = reIssueAppSettingModel.QuestionModel.LastName, answer1 = input.LastName }
                })
        };
    }

    private string ReturnRequestUri(OfficeFormInputModel input)
    {
        return input.RequestType.Equals(RequestTypeEnum.Refund) ?
                refundAppSettingModel.RequestUri :
                reIssueAppSettingModel.RequestUri;
    }
}