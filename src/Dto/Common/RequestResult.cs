namespace Dto.Common
{
    public class RequestResult
    {
        public RequestState State { get; set; }
        public string Msg { get; set; }
        public RequestResultData Data { get; set; }
    }
}
