using System.Text.Json.Serialization;

namespace Identity.DTO
{
    public class UserLoginResponse
    {
        public bool Success { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Token { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public DateTime? ExpirationDate { get; private set; }
        public List<string> Erros { get; private set; }

        public UserLoginResponse() =>
            Erros = new List<string>();

        public UserLoginResponse(bool success = true) : this() 
            => Success = success;

        public UserLoginResponse(bool success, string token, DateTime expirationDate) : this() 
        {
            Token = token;
            ExpirationDate = expirationDate;
            Success = success;
          
        }



    }
}
