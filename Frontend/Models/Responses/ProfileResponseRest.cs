namespace Frontend.Models.Responses
{
    public class ProfileResponseRest
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }
    }
}