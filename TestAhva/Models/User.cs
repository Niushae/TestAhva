using System.ComponentModel.DataAnnotations;

namespace TestAhva.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DocumentType { get; set; } = string.Empty;
        [Required]
        public string DocumentNumber { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public int FailedAttempts { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ProfileDetails? ProfileDetail { get; set; }
    }
}
