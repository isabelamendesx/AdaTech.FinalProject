namespace Identity.DTO
{
    public class UserRegisterResponse
    {
        public bool Sucess { get; private set; }
        public List<string> Errors { get; private set; }

        public UserRegisterResponse()
        {
            Errors = new List<string>();
        }

         public UserRegisterResponse(bool sucess = true) : this()
            => Sucess = sucess;

        public void AddErrors(IEnumerable<string> errors) 
            => Errors.AddRange(errors);
    }
}
