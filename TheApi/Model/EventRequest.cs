namespace TheApi.Model;

public class EventRequest
{
    public Guid RequestId { get; set; }
    public string Message { get; set; } = string.Empty; 
}
