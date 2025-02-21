﻿using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Entities
{
    public sealed class DepratamentSettingsViewModel
    {
        public int DepartamentSettingsId { get; set; }

        [Required(ErrorMessage = "Należy wpisać dzienny wymiar pracy")]
        [Range(typeof(TimeSpan), "01:00:00", "12:00:00", ErrorMessage = "Minmalna wartość to 1h, maksymalna 12h")]
        public TimeSpan WorkDayLength { get; set; }

        [Required(ErrorMessage = "Należy wpisać maksymalny tygodniowy wymiar pracy")]
        [Range(typeof(TimeSpan), "12:00:00", "84:00:00", ErrorMessage = "Minmalna wartość to 12h, maksymalna 84h")]
        public TimeSpan MaximumWeekWorkTimeLength { get; set; }

        [Required(ErrorMessage = "Należy wpisać maksymalną przerwę między dyżurami")]
        [Range(typeof(TimeSpan), "12:00:00", "100:00:00", ErrorMessage = "Minmalna wartość to 12h, maksymalna 84h")]
        public TimeSpan MinimalShiftBreak { get; set; }

        [Required(ErrorMessage = "Należy wpisać ilość godzin świątecznych należną za zmianę dzienną")]
        [Range(typeof(TimeSpan), "00:00:00", "12:00:00", ErrorMessage = "Minmalna wartość to 0h, maksymalna 12h")]
        public TimeSpan DayShiftHolidayEligibleHours { get; set; }

        [Required(ErrorMessage = "Należy wpisać ilość godzin świątecznych należną za zmianę nocną")]
        [Range(typeof(TimeSpan), "00:00:00", "12:00:00", ErrorMessage = "Minmalna wartość to 0h, maksymalna 12h")]
        public TimeSpan NightShiftHolidayEligibleHours { get; set; }

        [Required(ErrorMessage = "Należy wpisać docelową liczbę pracowników na zmianie")]
        [Range(1, 100, ErrorMessage = "Minmalna wartość to 1 pracownik, maksymalna 100")]
        public int TargetMinNumberOfNursesOnShift { get; set; }

        [Required(ErrorMessage = "Należy wpisać docelową minimalną długość zmiany porannej")]
        [Range(typeof(TimeSpan), "01:00:00", "12:00:00", ErrorMessage = "Minmalna wartość to 1h, maksymalna 12h")]
        public TimeSpan TargetMinimalMorningShiftLenght { get; set; }

        [Required(ErrorMessage = "Należy wpisać domyślną liczbę prób generatora")]
        [Range(1, 60, ErrorMessage = "Liczba prób musi być większa od 1 i nie większa od 60")]
        public int DefaultGeneratorRetryValue { get; set; }
        [Required(ErrorMessage = "Należy wpisać domyślny maksymalny czas generowania")]
        [Range(1, 60, ErrorMessage = "Maksymalny czas musi być większy niż 1 sekunda i mniejszy niż 60 sekund")]
        public int DefaultGeneratorTimeOut { get; set; }
        public bool UseTeams { get; set; }
    }
}
