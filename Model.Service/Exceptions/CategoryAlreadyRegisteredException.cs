namespace Model.Service.Exceptions
{
    public class CategoryAlreadyRegisteredException : Exception
    {
        public int StatusCode { get; set; }
        public uint ConflictingCategoryId { get; set; }
        public CategoryAlreadyRegisteredException(uint conflictingCategoryId) : base("You can not create the same category twice.")
        {
            StatusCode = 400;
            ConflictingCategoryId = conflictingCategoryId;
        }
    }
}
