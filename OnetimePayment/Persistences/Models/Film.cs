using Microsoft.EntityFrameworkCore;

namespace OnetimePayment;

public class Film
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public long Duration { get; set; }
    public DateTime UploadedTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<FilmPrice> Prices { get; set; } = [];
    public virtual ICollection<Order> Orders { get; set; } = [];
}

public class FilmPrice
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Precision(14, 2)]
    public decimal Amount { get; set; }

    public Guid FilmId { get; set; }
    public virtual Film Film { get; set; } = null!;
}