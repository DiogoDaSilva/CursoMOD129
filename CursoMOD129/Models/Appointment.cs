using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CursoMOD129.Models
{
    public class Appointment
    {
        public int ID { get; set; }

        [Display(Name = "Appointment Number")]
        [Required]
        public string Number { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }

        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        [Required]
        public DateTime Time { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int ClientID { get; set; }

        [ValidateNever]
        // FOREIGN KEY
        // Add INCLUDE
        public Client Client { get; set; }

        [Required]
        [Display(Name = "Medic")]
        public int MedicID { get; set; }

        [ValidateNever]
        // FOREIGN KEY
        // ADD INCLUDE
        public TeamMember Medic { get; set; }

        [Display(Name = "Info")]
        public string Info { get; set; }

        [Display(Name = "Done")]
        public bool IsDone { get; set; }
    }
}
