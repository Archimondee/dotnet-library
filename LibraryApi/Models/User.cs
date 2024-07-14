public class User : BaseEntity
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public required string Username { get; set; }
  public required string Password { get; set; }
  public required string Email { get; set; }
  public required UserRole Role { get; set; }
  public ICollection<Transaction>? Transactions { get; set; }
}

public enum UserRole
{
  User,
  Author
}