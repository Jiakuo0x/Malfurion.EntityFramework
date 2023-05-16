using Malfurion.EntityFramework.Models;

public class User : EntityBase<Guid>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}