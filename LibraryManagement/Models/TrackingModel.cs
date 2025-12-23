using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class TrackingModel
    {
        public int TrackingUid { get; set; }

        [Required]
        public int BookUid { get; set; }

        public string? Title { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(15)]
        public string? Phone { get; set; }

        [Required]
        public string TrackingStatus { get; set; } = string.Empty;

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class TrackingModelById
    {
        public int TrackingUid { get; set; }

        [Required]
        public int BookUid { get; set; }

        public string Title { get; set; }


        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [Required]
        public string TrackingStatus { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
