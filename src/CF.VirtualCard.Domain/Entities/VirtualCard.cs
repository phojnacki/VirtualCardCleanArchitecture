namespace CF.VirtualCard.Domain.Entities;

public class VirtualCard
{
    public long Id { get; set; }
    public DateTime Created { get; set; } 
    public string Email { get; set; } 
    public string FirstName { get; set; }
    public string Surname { get; set; }

}