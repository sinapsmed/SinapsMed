using System.Net;

namespace Core.Utilities.Results.Concrete.ErrorResult
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(string message, HttpStatusCode statusCode, Exception exception) : base(default, false, message, statusCode, exception)
        {
        }

        public ErrorDataResult(string message, HttpStatusCode statusCode, string errorMessage) : base(default, false, message, statusCode, errorMessage)
        {
        }

        public ErrorDataResult(string message, HttpStatusCode statusCode) : base(default, false, message, statusCode, message)
        {
        }

        public ErrorDataResult(string message, Exception exception) : base(default, false, message, HttpStatusCode.BadRequest, exception)
        {
        }

        public ErrorDataResult(string message, string exceptionMessage) : base(default, false, message, HttpStatusCode.BadRequest, exceptionMessage)
        {
        }


        public ErrorDataResult(string message) : base(default, false, message, HttpStatusCode.BadRequest, message)
        {
        }

    }
}
