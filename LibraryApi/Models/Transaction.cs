public class Transaction : BaseEntity
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public Guid UserId { get; set; }
  public required User User { get; set; }
  public Guid BookId { get; set; }
  public Book Book { get; set; }
  public DateTime BorrowedOn { get; set; }
  public DateTime? ReturnedOn { get; set; }
}