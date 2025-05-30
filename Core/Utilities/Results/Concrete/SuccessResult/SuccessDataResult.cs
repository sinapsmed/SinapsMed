using System.Net;

namespace Core.Utilities.Results.Concrete.SuccessResult
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        public SuccessDataResult(T data, string message, HttpStatusCode statusCode) : base(data, true, message, statusCode)
        {
        }
        public SuccessDataResult(T data, HttpStatusCode statusCode) : base(data, true, statusCode)
        {
        }
        public SuccessDataResult(string message, HttpStatusCode statusCode) : base(default, true, message, statusCode)
        {
        }
        public SuccessDataResult(HttpStatusCode statusCode) : base(default, true, statusCode)
        {
        }
        public SuccessDataResult(T data) : base(data, true, HttpStatusCode.OK)
        {
        }
        public SuccessDataResult() : base(default, true, HttpStatusCode.OK)
        {
        }
    }
}
