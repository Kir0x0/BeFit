using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BeFit.Models
{
    public class PerformedExercise
    {
        [Display(
            Name = "Id",
            Description = "Unikalny identyfikator wykonanego ćwiczenia"
        )]
        public int Id { get; set; }

        [Display(
            Name = "Sesja treningowa",
            Description = "Sesja treningowa, podczas której wykonano ćwiczenie"
        )]
        public int TrainingSessionId { get; set; }
        public TrainingSession? TrainingSession { get; set; }

        [Display(
            Name = "Typ ćwiczenia",
            Description = "Typ ćwiczenia, które zostało wykonane"
        )]
        public int ExerciseTypeId { get; set; }
        public ExerciseType? ExerciseType { get; set; }

        [Display(
            Name = "Użytkownik",
            Description = "Użytkownik, który wykonał ćwiczenie"
        )]
        [ValidateNever]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        [Display(
            Name = "Obciążenie [kg]",
            Description = "Zastosowane obciążenie w kilogramach"
        )]
        [Range(0, 1000)]
        public double Weight { get; set; }

        [Display(
            Name = "Liczba serii",
            Description = "Liczba wykonanych serii danego ćwiczenia"
        )]
        [Range(1, 100)]
        public int Sets { get; set; }

        [Display(
            Name = "Powtórzenia w serii",
            Description = "Liczba powtórzeń w każdej serii"
        )]
        [Range(1, 500)]
        public int Repetitions { get; set; }
    }
}
