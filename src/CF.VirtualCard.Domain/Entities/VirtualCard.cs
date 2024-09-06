namespace CF.VirtualCard.Domain.Entities;

public class VirtualCard
{
    public long Id { get; set; }
    public DateTime ExpiryDate { get; set; } 
    public string CardNumber { get; set; } 
    public string FirstName { get; set; }
    public string Surname { get; set; }
    //public decimal Limit { get; set; }
    //public bool IsActive { get; set; }
}