using System.Net;

namespace CollegeManagement.Models
{
    public class APIResponse
    {
        public bool Status { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? StatusMessage { get; set; }
        public dynamic? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public APIResponse ResponseToClient(bool status, HttpStatusCode statusCode, string message, dynamic? data, string error)
        {
            var _apiResponse = new APIResponse()
            {
                Status = status,
                StatusCode = statusCode,
                StatusMessage = message,
                Data = data,
            };
            _apiResponse.Errors.Add(error);

            return _apiResponse;
        }
    }
}
