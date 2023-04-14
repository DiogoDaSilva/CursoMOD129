using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace CursoMOD129.Models
{
    public class TeamMember
    {
        public int ID { get; set; }

        [Display(Name = "Full Name")]
        [StringLength(255, MinimumLength = 5)]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Birthday { get; set; }

        [StringLength(100, MinimumLength = 5)]
        [Required]
        public string Address { get; set; }

        [Display(Name = "Zip Code")]
        [StringLength(20, MinimumLength = 5)]
        [Required]
        public string ZipCode { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required]
        public string City { get; set; }

        [StringLength(255, MinimumLength = 8)]
        [Required]
        public string NIF { get; set; }

        [Display(Name = "Work Role")]
        [Required]
        public int WorkRoleID { get; set; }

        [ValidateNever]
        public WorkRole WorkRole {get; set; }


        public Specialty? Specialty { get; set; }
    }
}
