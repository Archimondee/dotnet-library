using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

public class LibraryContext : DbContext
{
  public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

  public DbSet<Author> Authors { get; set; }
  public DbSet<Book> Books { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<Transaction> Transactions { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>()
      .HasDiscriminator<UserRole>("Role")
      .HasValue<User>(UserRole.User)
      .HasValue<Author>(UserRole.Author);

    modelBuilder.Entity<Author>()
      .HasMany(a => a.Books)
      .WithOne(b => b.Author)
      .HasForeignKey(b => b.AuthorId);

    modelBuilder.Entity<Transaction>()
      .HasOne(t => t.User)
      .WithMany(u => u.Transactions)
      .HasForeignKey(t => t.UserId);

    modelBuilder.Entity<Transaction>()
      .HasOne(a => a.Book)
      .WithMany(b => b.Transactions)
      .HasForeignKey(t => t.BookId);

    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
      if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
      {
        var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(entityType.ClrType);
      }
    }
  }
  private static readonly MethodInfo SetIsDeletedQueryFilterMethod = typeof(LibraryContext)
        .GetMethod(nameof(SetIsDeletedQueryFilter), BindingFlags.NonPublic | BindingFlags.Static);

  private static void SetIsDeletedQueryFilter<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType) where TEntity : BaseEntity
  {
    modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.DeletedAt.HasValue);
  }

  public override int SaveChanges()
  {
    foreach (var entry in ChangeTracker.Entries<BaseEntity>())
    {
      switch (entry.State)
      {
        case EntityState.Added:
          entry.Entity.CreatedAt = DateTime.UtcNow;
          break;
        case EntityState.Modified:
          entry.Entity.UpdatedAt = DateTime.UtcNow;
          break;
      }
    }

    return base.SaveChanges();
  }
}



