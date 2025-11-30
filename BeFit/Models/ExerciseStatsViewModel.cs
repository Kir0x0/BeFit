using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseStatsViewModel
    {
        [Display(
            Name = "Ćwiczenie",
            Description = "Nazwa typu ćwiczenia"
        )]
        public string ExerciseTypeName { get; set; } = string.Empty;

        [Display(
            Name = "Liczba wykonań",
            Description = "Ile razy ćwiczenie było wykonane w ostatnich 4 tygodniach"
        )]
        public int TimesPerformed { get; set; }

        [Display(
            Name = "Łącznie powtórzeń",
            Description = "Łączna liczba powtórzeń (serie * powtórzenia)"
        )]
        public int TotalRepetitions { get; set; }

        [Display(
            Name = "Średnie obciążenie [kg]",
            Description = "Średnie zastosowane obciążenie w kilogramach"
        )]
        public double? AverageWeight { get; set; }

        [Display(
            Name = "Maksymalne obciążenie [kg]",
            Description = "Największe użyte obciążenie w kilogramach"
        )]
        public double? MaxWeight { get; set; }
    }
}
