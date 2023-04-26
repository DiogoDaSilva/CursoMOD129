using System.ComponentModel.DataAnnotations;

namespace CursoMOD129.Models
{
    // Code First - Entity Framework - ORM -> Object Relational Mapper
    public class Client
    {
        public int ID { get; set; }


        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Birthday { get; set; }

        [StringLength(100)]
        [Required]
        public string Address { get; set; }

        [StringLength(20)]
        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [StringLength(50)]
        [Required]
        public string City { get; set; }
        
        [StringLength(255)]
        [Required]
        public string NIF { get; set; }

        [StringLength(255)]
        [Display(Name = "Health Care Number")]
        public string? HealthCareNumber { get; set; }
    }
}
