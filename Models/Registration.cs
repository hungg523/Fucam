using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fucam.Models
{
    public class Registration : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên phụ huynh")]
        [StringLength(100)]
        public string? ParentName { get; set; }

        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Số điện thoại phải bao gồm đúng 10 chữ số")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên và tuổi của con")]
        [StringLength(200)]
        public string? ChildInfo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int Status { get; set; } = 0; // 0 = Chưa liên hệ, 1 = Đã liên hệ

        [StringLength(500)]
        public string? Notes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber) && string.IsNullOrWhiteSpace(Email))
            {
                yield return new ValidationResult(
                    "Theo yêu cầu mới, bạn vui lòng cung cấp ít nhất [Số điện thoại] HOẶC [Email] để chúng tôi liên hệ.",
                    new[] { nameof(PhoneNumber), nameof(Email) }
                );
            }
        }
    }
}
