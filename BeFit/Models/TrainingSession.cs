using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class TrainingSession
    {
        [Display(
            Name = "Id sesji",
            Description = "Unikalny identyfikator sesji treningowej"
        )]
        public int Id { get; set; }

        [Display(
            Name = "Początek treningu",
            Description = "Data i godzina rozpoczęcia sesji treningowej"
        )]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Display(
            Name = "Koniec treningu",
            Description = "Data i godzina zakończenia sesji treningowej"
        )]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        [Display(
            Name = "Użytkownik",
            Description = "Użytkownik, do którego należy sesja treningowa"
        )]
        public string ApplicationUserId { get; set; } = string.Empty;

        public ApplicationUser? ApplicationUser { get; set; }

        [Display(
            Name = "Wykonane ćwiczenia",
            Description = "Ćwiczenia wykonane w ramach tej sesji"
        )]
        public ICollection<PerformedExercise>? PerformedExercises { get; set; }
    }
}