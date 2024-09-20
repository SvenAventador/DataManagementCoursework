namespace ShopsGoodsLibrary.Models;

public partial class Type
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Good> Goods { get; set; } = new List<Good>();
}
