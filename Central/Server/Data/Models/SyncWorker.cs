using Microsoft.EntityFrameworkCore;
using Shared;
using Store.API.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Central.API.Data.Models
{
    [Table("SyncWorkers")]
    [Comment("Описва процес за sync на данни от Central към Store")]
    public class SyncWorker : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        [MaxLength(Constants.STRING_DB_MAX_LENGTH_10)]
        public string Action { get; set; } // "Created", "Updated", "Deleted"

        [MaxLength(Constants.STRING_DB_MAX_LENGTH_500)]
        public string? ErrorMessage { get; set; }

        [Required]
        [MaxLength(Constants.STRING_DB_MAX_LENGTH_10)]
        public string Status { get; set; } = StatusType.Pending; // "Pending", "Completed" , "Failed"

        [Required]
        public int AttemptsCount { get; set; }

        [Required]
        [MaxLength(Constants.STRING_DB_MAX_LENGTH_1000)]
        public string Payload { get; set; }

        [MaxLength(Constants.STRING_DB_MAX_LENGTH_100)]
        public string? DestinationStore { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
