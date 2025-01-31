namespace SharedKernel.Domain;

public abstract class Register
{
    public required DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; } = true;
}
