namespace OfficeFormExample.Models;

public class AppSettingModel
{
    public RefundOfficeForm RefundOfficeForm { get; set; }
    public ReIssueOfficeForm ReIssueOfficeForm { get; set; }
    public CommonDataModel CommonData { get; set; }
}

public class RefundOfficeForm : OfficeForm
{
    public RefundQuestionModel QuestionModel { get; set; }
}

public class ReIssueOfficeForm : OfficeForm
{
    public ReIssueQuestionModel QuestionModel { get; set; }
}

public class OfficeForm
{
    public string RequestUri { get; set; }
}

public class RefundQuestionModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string AgencyName { get; set; }
    public string IATANumber { get; set; }
    public string EmailReceiver { get; set; }
    public string PNR { get; set; }
    public string TravelDate { get; set; }
    public string TicketStock { get; set; }
    public string FlightNumber { get; set; }
    public string VKAReasonCodeREF { get; set; }
    public string AuditRemark { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string TicketNumber { get; set; }
}

public class ReIssueQuestionModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string AgencyName { get; set; }
    public string IATANumber { get; set; }
    public string EmailReceiver { get; set; }
    public string PNR { get; set; }
    public string TravelDate { get; set; }
    public string TicketStock { get; set; }
    public string FlightNumber { get; set; }
    public string VKAReasonCodeSAL { get; set; }
    public string VKAReasonCodeEXC { get; set; }
    public string AuditRemark { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class CommonDataModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string AgencyName { get; set; }
    public string IATANumber { get; set; }
    public string EmailReceiver { get; set; }
    public string VKAReasonCodeREF { get; set; }
    public string VKAReasonCodeSAL { get; set; }
    public string VKAReasonCodeEXC { get; set; }
}