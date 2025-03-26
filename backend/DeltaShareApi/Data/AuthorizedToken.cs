namespace DeltaShareApi.Data;

public class AuthorizedToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = default!;
}
