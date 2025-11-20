using Microsoft.EntityFrameworkCore;
using Shared;
using Store.API.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.API.Data.Models
{
    [Table("Products")]
    [Comment("Описва продукт")]
    public class Product : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(Constants.STRING_DB_MAX_LENGTH_100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(Constants.STRING_DB_MAX_LENGTH_500)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinPrice { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime UpdatedOn { get; set; }
    }
}
