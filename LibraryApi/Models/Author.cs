public class Author : BaseEntity
{
  public string? Bio { get; set; }
  public ICollection<Book>? Books { get; set; }
}