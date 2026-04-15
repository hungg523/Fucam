using System.ComponentModel.DataAnnotations;

namespace Fucam.Models
{
    public class AdminUser
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
    }
}
