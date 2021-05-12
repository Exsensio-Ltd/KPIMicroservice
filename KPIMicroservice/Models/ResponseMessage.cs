namespace KPIMicroservice.Models
{
    public class ResponseMessage
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
    }

    public class ResponseMessage<T>
    {
        public T Content { get; set; }
        public bool HasError { get; set; }
        public string Message { get; set; }
    }
}
