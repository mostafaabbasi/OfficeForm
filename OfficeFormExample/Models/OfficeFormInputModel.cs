using OfficeFormExample.Enums;

namespace OfficeFormExample.Models;

public class OfficeFormInputModel
{
    public bool IsSent { get; set; }
    public RequestTypeEnum RequestType { get; set; }
    public string PNR { get; set; } = String.Empty;
    public string TravelDate { get; set; } = String.Empty;
    public string TicketStock { get; set; } = String.Empty;
    public string FlightNumber { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string TicketNumber { get; set; } = String.Empty;
}