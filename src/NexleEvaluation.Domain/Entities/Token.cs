using NexleEvaluation.Domain.Entities.Base;
using NexleEvaluation.Domain.Entities.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexleEvaluation.Domain.Entities
{
    public class Token: IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [MaxLength(250)]
        [Column(TypeName = "varchar(250)")]
        public string RefreshToken { get; set; }

        [Required]
        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string ExpiresIn { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public User User { get; set; }
    }
}
