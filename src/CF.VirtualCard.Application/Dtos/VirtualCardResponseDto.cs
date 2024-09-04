namespace CF.VirtualCard.Application.Dtos;

public record VirtualCardResponseDto
{
    public long Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string FullName { get; set; }
}