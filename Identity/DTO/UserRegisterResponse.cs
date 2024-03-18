namespace Identity.DTO
{
    public class UserRegisterResponse
    {
        public bool Success { get; private set; }
        public List<string> Errors { get; private set; }

        public UserRegisterResponse()
        {
            Errors = new List<string>();
        }

         public UserRegisterResponse(bool success = true) : this()
            => Success = success;

        public void AddErrors(IEnumerable<string> errors) 
            => Errors.AddRange(errors);
    }
}
