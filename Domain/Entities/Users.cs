using Domain.Common;
using Domain.Enum;

namespace Domain.Entities;

public class Users : BaseEntity<Guid>
{
    public string UserName { get; set; } = default!;

    // Enum for Account Type
    public AccountTypeStatus AccountType { get; set; } = default!;

    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public bool TrustedVerified { get; set; } = false;
    public string? RefreshToken { get; set; } = default!;
    public DateTime? RefreshTokenExpiry { get; set; } = default!;

    // Navigation Properties
    public virtual ICollection<Addresses> Addresses { get; set; } = new List<Addresses>();
    public virtual ICollection<SocialMediaLinks> SocialMediaLinks { get; set; } = new List<SocialMediaLinks>();
    public virtual ICollection<Posts> Posts { get; set; } = new List<Posts>();
    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
    public virtual ICollection<Mark> SavedPosts { get; set; } = new List<Mark>();
    public virtual ICollection<Image> Images {get; set;} = new List<Image>();
}