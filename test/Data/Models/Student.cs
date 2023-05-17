using Malfurion.EntityFramework.Models;

public class Student : EntityBase, IPseudoDeletion
{
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}