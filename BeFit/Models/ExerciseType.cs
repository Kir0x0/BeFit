using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseType
    {
        [Display(
            Name = "Id ćwiczenia",
            Description = "Unikalny identyfikator typu ćwiczenia"
        )]
        public int Id { get; set; }

        [Display(
            Name = "Nazwa ćwiczenia",
            Description = "Nazwa ćwiczenia wykonywanego na siłowni"
        )]
        [Required]
        [StringLength(100, ErrorMessage = "Nazwa ćwiczenia może mieć maksymalnie 100 znaków.")]
        public string Name { get; set; } = string.Empty;

        [Display(
            Name = "Wykonania ćwiczenia",
            Description = "Lista wykonań tego typu ćwiczenia"
        )]
        public ICollection<PerformedExercise>? PerformedExercises { get; set; }
    }
}
