using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enum;

namespace Domain.Entities;

public class Posts : BaseEntity<Guid>
{ 
    public string ItemName { get; set; } = default!;
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
    public int ViewCount { get; set; } = 0;

    // Enum
    public BuyerCondition BuyerCondition { get; set; } = default!;
    public ItemCondition ItemCondition { get; set; } = default!;
    public ItemStatus ItemStatus { get; set; } = default!;

    // Foreign Keys
    public Guid UserId { get; set; }
    public virtual Users User { get; set; } = default!;
    public Guid MarketId { get; set; }
    public virtual Mark Market { get; set; } = default!;

    // Navigation Properties
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

}