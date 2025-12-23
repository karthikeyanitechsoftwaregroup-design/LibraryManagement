using LibraryManagement.Validation;
using System.ComponentModel.DataAnnotations;
namespace LibraryManagement.Models
{
    public class BookModel
    {
        public int BookUid { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        [CurrentYear]
        public int? PublishedYear { get; set; }
        public int Quantity { get; set; }
        public int AvailableCopies { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}
