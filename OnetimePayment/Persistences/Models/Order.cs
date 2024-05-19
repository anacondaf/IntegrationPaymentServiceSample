using Microsoft.EntityFrameworkCore;

namespace OnetimePayment;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Precision(14, 2)]
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Film> Films { get; set; } = [];
    public virtual Transaction Transaction { get; set; } = null!;
}