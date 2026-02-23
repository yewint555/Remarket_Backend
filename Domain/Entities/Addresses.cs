using Domain.Common;

namespace Domain.Entities;

public class Addresses : BaseEntity<Guid>
{
    public string HomeNumber { get; set; } = default!;
    public string? AptNumber { get; set; }
    public string Street { get; set; } = default!;
    public string TownShip { get; set; } = default!;
    public string City { get; set; } = default!;

    // Foreign Key
    public Guid UserId { get; set; }
    public virtual Users User { get; set; } = default!;
}