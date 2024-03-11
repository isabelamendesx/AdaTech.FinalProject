using System.Text.Json.Serialization;

namespace Model.Application.API.DTO
{
    public class UserLoginResponse
    {
        public bool Sucess { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Token { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public DateTime? ExpirationDate { get; private set; }
        public List<string> Erros { get; private set; }

        public UserLoginResponse() =>
            Erros = new List<string>();

        public UserLoginResponse(bool sucess = true) : this() 
            => Sucess = sucess;

        public UserLoginResponse(bool sucess, string token, DateTime expirationDate) : this() 
        {
            Token = token;
            ExpirationDate = expirationDate;
            Sucess = sucess;
          
        }



    }
}
