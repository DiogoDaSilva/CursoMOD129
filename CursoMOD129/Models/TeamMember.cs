using CursoMOD129.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Birthday { get; set; }

        [Display(Name = "Address")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The field <b>{0}</b> must be a string with a minimum length of {2} and a maximum length of {1}.")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "Zip Code")]
        [StringLength(20, MinimumLength = 5)]
        [Required]
        public string ZipCode { get; set; }

        [Display(Name = "City")]
        [StringLength(50, MinimumLength = 1)]
        [Required]
        public string City { get; set; }

        [Display(Name = "VAT")]
        [StringLength(255, MinimumLength = 8)]
        [Required]
        public string NIF { get; set; }

        [Display(Name = "Work Role")]
        [Required]
        public int WorkRoleID { get; set; }

        // Navigation Property
        [ValidateNever]
        public WorkRole WorkRole {get; set; }


        [Display(Name = "Specialty")]
        public Specialty? Specialty { get; set; }

        [StringLength(255)]
        [Display(Name = "E-mail")]
        [Required]
        public string Email { get; set; }


        public bool IsSpecialtyValid(ApplicationDbContext context)
        {
            WorkRole medicWorkRole = context.WorkRoles.First(wr => wr.Name == "Medic");

            // Se não for médico, então a specialty tem de ser null
            if (this.WorkRoleID != medicWorkRole.ID)
            {
                return this.Specialty == null;
            }

            // Se for médico, então a specialty tem de estar preenchida (diferente de null)
            return this.Specialty != null;
        }
    }
}
