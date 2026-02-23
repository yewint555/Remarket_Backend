using Domain.Common;
using Domain.Enum;

namespace Domain.Entities;

public class Orders : BaseEntity<Guid>
{
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string PhoneNumber { get; set; } = default!;

    // 
    public OrderComfirmStatus OrderComfirmationStatus { get; set; } = default!;

    // Foreign Keys
    public Guid UserId { get; set; }
    public virtual Users User { get; set; } = default!;
    public Guid PostId { get; set; }
    public virtual Posts Post { get; set; } = default!;
}