using System;
using System.ComponentModel.DataAnnotations;

namespace RhzLearnRest.Domains.Models
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }

        
        public Author Author { get; set; }

        public Guid AuthorId { get; set; }
    }
}
