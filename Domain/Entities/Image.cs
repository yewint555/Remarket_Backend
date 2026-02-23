using Domain.Common;
using Domain.Entities;

public class Image : BaseEntity<Guid>
{
    public string ImgPath { get; set; } = default!;
    public string? ImgUrl { get; set; } = default!;

    public Guid? UserId { get; set; }
    public virtual Users? User { get; set; }

    public Guid? PostId { get; set; }
    public virtual Posts? Post { get; set; }
}