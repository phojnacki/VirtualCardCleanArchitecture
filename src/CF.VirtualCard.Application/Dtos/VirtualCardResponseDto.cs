namespace CF.VirtualCard.Application.Dtos;

public record VirtualCardResponseDto
{
    public long Id { get; set; }
    public string CardNumber { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string FullName { get; set; }
    public DateTime ExpiryDate { get; set; }
    public decimal Funds { get; set; }
    public bool IsActive { get; set; }
}