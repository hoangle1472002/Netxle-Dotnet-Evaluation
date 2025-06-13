using NexleEvaluation.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexleEvaluation.Domain.Entities.Identity
{
    public class User: IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(32)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(32)")]
        public string LastName { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Email { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Hash { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<Token> Tokens { get; set; }
    }
}