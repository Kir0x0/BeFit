using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(
            Name = "Imię",
            Description = "Imię użytkownika widoczne w systemie"
        )]
        [StringLength(100)]
        public string? FirstName { get; set; }

        [Display(
            Name = "Nazwisko",
            Description = "Nazwisko użytkownika widoczne w systemie"
        )]
        [StringLength(100)]
        public string? LastName { get; set; }

        [Display(
            Name = "Sesje treningowe",
            Description = "Sesje treningowe przypisane do użytkownika"
        )]
        public ICollection<TrainingSession>? TrainingSessions { get; set; }

        [Display(
            Name = "Wykonane ćwiczenia",
            Description = "Wszystkie wykonane ćwiczenia użytkownika"
        )]
        public ICollection<PerformedExercise>? PerformedExercises { get; set; }
    }
}
