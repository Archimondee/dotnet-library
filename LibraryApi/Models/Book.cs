public class Book : BaseEntity
{
  public int Id { get; set; }
  public string? Title { get; set; }

  public Guid AuthorId { get; set; }
  public Author? Author { get; set; }

  public ICollection<Transaction>? Transactions { get; set; }
}