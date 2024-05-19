namespace OnetimePayment;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime IssueAt { get; set; } = DateTime.UtcNow;

    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; } = null!;
}
