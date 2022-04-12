using System;
using System.ComponentModel.DataAnnotations;

namespace ITServiceApp.Models.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [StringLength(128)]
        public string CreatedUser { get; set; }

        public DateTime? UpdatedDate { get; set; }
        [StringLength(128)]
        public string UpdateUser { get; set; }
    }
}
