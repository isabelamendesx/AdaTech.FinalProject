namespace Model.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
