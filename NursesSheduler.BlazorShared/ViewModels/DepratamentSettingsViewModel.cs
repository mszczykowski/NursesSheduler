﻿using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class DepratamentSettingsViewModel
    {
        public int DepartamentSettingsId { get; set; }

        [Required(ErrorMessage = "Należy wpisać dzienny wymiar pracy")]
        [Range(typeof(TimeSpan), "01:00:00", "12:00:00", ErrorMessage = "Minmalna wartość to 1h, maksymalna 12h")]
        public TimeSpan WorkingTime { get; set; }

        [Required(ErrorMessage = "Należy wpisać maksymalny tygodniowy wymiar pracy")]
        [Range(typeof(TimeSpan), "07:00:00", "84:00:00", ErrorMessage = "Minmalna wartość to 7h, maksymalna 84h")]
        public TimeSpan MaximalWeekWorkingTime { get; set; }

        [Required(ErrorMessage = "Należy wpisać maksymalną przerwę między dyżurami")]
        [Range(typeof(TimeSpan), "00:00:00", "100:00:00", ErrorMessage = "Minmalna wartość to 0h, maksymalna 100h")]
        public TimeSpan MinmalShiftBreak { get; set; }

        [Required(ErrorMessage = "Należy wybrać pierwszy miesiąc pierwszego kwartału")]
        [Range(1, 12, ErrorMessage = "Wartość musi być miesiącem")]
        public int FirstQuarterStart { get; set; }

        [Required(ErrorMessage = "Należy wpisać godzinę rozpoczęcia pierwszej zmiany")]
        [RegularExpression("^(0?[0-9]|1[0-1]):[0-5][0-9]$", ErrorMessage = "Pierwsza zmiana może zaczynać się tylko przed 12:00")]
        public TimeOnly FirstShiftStartTime { get; set; }

        [Required(ErrorMessage = "Należy wpisać docelową liczbę pracowników na zmianie")]
        [Range(1, 100, ErrorMessage = "Minmalna wartość to 1 pracownik, maksymalna 100")]
        public int TargetNumberOfNursesOnShift { get; set; }

        [Required(ErrorMessage = "Należy wpisać docelową minimalną długość zmiany porannej")]
        [Range(typeof(TimeSpan), "01:00:00", "12:00:00", ErrorMessage = "Minmalna wartość to 1h, maksymalna 12h")]
        public TimeSpan TargetMinimalMorningShiftLenght { get; set; }

        [Required(ErrorMessage = "Należy wpisać domyślną liczbę prób generatora")]
        [Range(1, 10, ErrorMessage = "Liczba prób musi być większa od 1 i nie większa od 10")]
        public int DefaultGeneratorRetryValue { get; set; }
    }
}
