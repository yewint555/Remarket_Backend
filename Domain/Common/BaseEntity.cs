namespace Domain.Common;

public abstract class BaseEntity<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}