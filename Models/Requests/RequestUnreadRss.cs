namespace Web_API_tests.Models.Requests
{
    public class RequestUnreadRss : BaseRequest
    {
        public DateTime Date { get; set; }
        public int AddDays { get; set; }
    }
}
