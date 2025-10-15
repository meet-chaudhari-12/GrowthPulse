using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrowthPulse.Models
{
    public enum UserRoles
    {
        Admin,
        Manager,
        Customer
    }
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(7, ErrorMessage = "Password must be at least 7 characters long")]
        public string PasswordHash { get; set; }

        public ICollection<Plant> Plants { get; set; }
        public ICollection<Order> Orders { get; set; }

        public UserRoles Role { get; set; }
    }
}