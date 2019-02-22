using System;

namespace FlogViewer
{
    public class FlogDetailPoco
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string Product { get; set; }
        public string Layer { get; set; }
        public string Location { get; set; }
        public string Hostname { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public long? ElapsedMilliseconds { get; set; }
        public string Exception { get; set; }
        public string CustomException { get; set; }
        public string CorrelationId { get; set; }
        public string AdditionalInfo { get; set; }
    }
}