using System.Text.Json.Serialization;

namespace Identity.DTO
{
    public class UserLoginResponse
    {
        public bool Success => Errors.Count == 0;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AcessToken { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RefreshToken { get; private set; }

        public List<string> Errors { get; private set; }

        public UserLoginResponse() =>
            Errors = new List<string>();

        public UserLoginResponse(bool success, string accessToken, string refreshToken) : this() 
        {
            AcessToken = accessToken;
            RefreshToken = refreshToken;          
        }

        public void AddError(string error) =>
            Errors.Add(error);

        public void AddErros(IEnumerable<string> erros) =>
            Errors.AddRange(erros);


    }
}
