using System.ComponentModel.DataAnnotations;

namespace TestAhva.Models 
{ 
public class ProfileDetails
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string FirstLastName { get; set; } = string.Empty;
    [Required]
    public string SecondLastName { get; set; } = string.Empty;

    public string? JobTitle { get; set; }
    public string? Institution { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string Nationality { get; set; } = "Peruana";
    public string? Gender { get; set; }

    [Required, EmailAddress]
    public string PrimaryEmail { get; set; } = string.Empty;
    public string? SecondaryEmail { get; set; }
    public string? MobilePhone { get; set; }
    public string? SecondaryPhone { get; set; }
    public string? ContractType { get; set; }
    public DateOnly? HireDate { get; set; }

    public virtual User? User { get; set; }
}

}
