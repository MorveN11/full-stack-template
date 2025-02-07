namespace SharedKernel.Domain;

public abstract class Register
{
    public required DateTime CreatedOnUtc { get; init; }
    public DateTime? UpdatedOnUtc { get; set; }
}
