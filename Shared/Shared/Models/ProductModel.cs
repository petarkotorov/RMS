using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = Constants.REQUIRED_FIELD_MSG)]
        [MaxLength(Constants.STRING_DB_MAX_LENGTH_100, ErrorMessage = Constants.FIELD_NO_LONGER_THAN_100)]
        public string Name { get; set; }

        [Required(ErrorMessage = Constants.REQUIRED_FIELD_MSG)]
        [MaxLength(Constants.STRING_DB_MAX_LENGTH_500, ErrorMessage = Constants.FIELD_NO_LONGER_THAN_500)]
        public string Description { get; set; }

        [Required(ErrorMessage = Constants.REQUIRED_FIELD_MSG)]
        [Range(0, double.MaxValue, ErrorMessage = Constants.FIELD_INVALID_NUMBER)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = Constants.REQUIRED_FIELD_MSG)]
        [Range(0, double.MaxValue, ErrorMessage = Constants.FIELD_INVALID_NUMBER)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinPrice { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime UpdatedOn { get; set; }

        public string? DestinationStore { get; set; }

        public string? SourceStoreId { get; set; }
    }
}
