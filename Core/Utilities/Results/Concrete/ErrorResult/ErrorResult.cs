using System.Net;

namespace Core.Utilities.Results.Concrete.ErrorResult
{
    public class ErrorResult : Result
    {
        public ErrorResult(string message, HttpStatusCode statusCode, Exception exception) : base(false, message, statusCode, exception)
        {
        }
        public ErrorResult(string message, HttpStatusCode statusCode, string errorMessage) : base(false, message, statusCode, errorMessage)
        {
        }
        public ErrorResult(string message, HttpStatusCode statusCode) : base(false, message, statusCode, message)
        {
        }
        public ErrorResult(string message, Exception ex) : base(false, message, HttpStatusCode.BadRequest, message)
        {
        }
        public ErrorResult(string message, string exmessgae) : base(false, message, HttpStatusCode.BadRequest, exmessgae)
        {
        }
        public ErrorResult(string message) : base(false, message, HttpStatusCode.BadRequest, message)
        {
        }
        public ErrorResult() : base(false, "", HttpStatusCode.BadRequest, "")
        {
        }
    }
}
