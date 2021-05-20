namespace OEEMicroservice.Models
{
    public class ResponseMessage
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
    }

    public class ResponseMessage<T> : ResponseMessage
    {
        public T Content { get; set; }
    }
}
