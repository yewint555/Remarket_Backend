using Domain.Common;

namespace Domain.Entities;

public class SocialMediaLinks : BaseEntity<Guid>
{
    public string? Links { get; set; } = default!;

    // Foreign Key
    public Guid UserId { get; set; }
    public virtual Users User { get; set; } = default!;
}