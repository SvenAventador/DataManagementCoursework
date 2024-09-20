using System.ComponentModel.DataAnnotations.Schema;

namespace ShopsGoodsLibrary.Models;

public partial class User
{
    public int Id { get; set; }

    public string? UserFio { get; set; }

    public DateOnly? UserBirthday { get; set; }

    public string UserEmail { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string? UserPhone { get; set; }

    public string? UserRole { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [NotMapped]
    public DateTime? UserBirthdayForBinding
    {
        get => UserBirthday.HasValue ? UserBirthday.Value.ToDateTime(new TimeOnly()) : null;
        set => UserBirthday = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }
}
